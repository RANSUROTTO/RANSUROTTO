using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Models.Customers;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.AttributeName;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;
using RANSUROTTO.BLOG.Core.Domain.Customers.Setting;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Framework.Controllers;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Services.Blogs;
using RANSUROTTO.BLOG.Services.Common;
using RANSUROTTO.BLOG.Services.Customers;
using RANSUROTTO.BLOG.Services.Events;
using RANSUROTTO.BLOG.Services.Helpers;
using RANSUROTTO.BLOG.Services.Helpers.Setting;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class CustomerController : BaseAdminController
    {

        #region Fields

        private readonly CustomerSettings _customerSettings;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly ICustomerService _customerService;
        private readonly IBlogService _blogService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICustomerReportService _customerReportService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public CustomerController(CustomerSettings customerSettings, DateTimeSettings dateTimeSettings, ICustomerService customerService, IBlogService blogService, ICustomerActivityService customerActivityService, ICustomerReportService customerReportService, IGenericAttributeService genericAttributeService, ICustomerRegistrationService customerRegistrationService, IDateTimeHelper dateTimeHelper, ILocalizationService localizationService, IWorkContext workContext, ICacheManager cacheManager, IEventPublisher eventPublisher)
        {
            _customerSettings = customerSettings;
            _dateTimeSettings = dateTimeSettings;
            _customerService = customerService;
            _blogService = blogService;
            _customerActivityService = customerActivityService;
            _customerReportService = customerReportService;
            _genericAttributeService = genericAttributeService;
            _customerRegistrationService = customerRegistrationService;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
            _workContext = workContext;
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Customers

        public virtual ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual ActionResult List()
        {
            var defaultRoleIds = new List<int> { _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered).Id };

            var model = new CustomerListModel
            {
                SearchCustomerRoleIds = defaultRoleIds,
                DateOfBirthEnabled = _customerSettings.DateOfBirthEnabled,
                CompanyEnabled = _customerSettings.CompanyEnabled,
                PhoneEnabled = _customerSettings.PhoneEnabled
            };

            var allRoles = _customerService.GetAllCustomerRoles(true);
            foreach (var role in allRoles)
            {
                model.AvailableCustomerRoles.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Id.ToString(),
                    Selected = defaultRoleIds.Any(x => x == role.Id)
                });
            }

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult CustomerList(DataSourceRequest command, CustomerListModel model,
            [ModelBinder(typeof(CommaSeparatedModelBinder))] int[] searchCustomerRoleIds)
        {
            var searchDayOfBirth = 0;
            int searchMonthOfBirth = 0;

            if (!string.IsNullOrWhiteSpace(model.SearchDayOfBirth))
                searchDayOfBirth = Convert.ToInt32(model.SearchDayOfBirth);
            if (!string.IsNullOrWhiteSpace(model.SearchMonthOfBirth))
                searchMonthOfBirth = Convert.ToInt32(model.SearchMonthOfBirth);

            var customers = _customerService.GetAllCustomers(
                customerRoleIds: searchCustomerRoleIds,
                email: model.SearchEmail,
                username: model.SearchUsername,
                name: model.SearchName,
                dayOfBirth: searchDayOfBirth,
                monthOfBirth: searchMonthOfBirth,
                company: model.SearchCompany,
                phone: model.SearchPhone,
                ipAddress: model.SearchIpAddress,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = customers.Select(PrepareCustomerModelForList),
                Total = customers.TotalCount
            };

            return Json(gridModel);
        }

        public virtual ActionResult Create()
        {
            var model = new CustomerModel();
            PrepareCustomerModel(model, null, false);
            //设置默认可用
            model.Active = true;
            return View(model);
        }

        [ValidateInput(false)]
        [FormValueRequired("save", "save-continue")]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Create(CustomerModel model, bool continueEditing, FormCollection form)
        {
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var cust2 = _customerService.GetCustomerByEmail(model.Email);
                if (cust2 != null)
                    ModelState.AddModelError("", _localizationService.GetResource("Admin.Customers.Customer.EmailAlreadyRegistered"));
            }
            if (!string.IsNullOrWhiteSpace(model.Username))
            {
                var cust2 = _customerService.GetCustomerByUsername(model.Username);
                if (cust2 != null)
                    ModelState.AddModelError("", _localizationService.GetResource("Admin.Customers.Customer.UsernameAlreadyRegistered"));
            }

            //验证用户角色
            var allCustomerRoles = _customerService.GetAllCustomerRoles(true);
            var newCustomerRoles = new List<CustomerRole>();
            foreach (var customerRole in allCustomerRoles)
                if (model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                    newCustomerRoles.Add(customerRole);
            var customerRolesError = ValidateCustomerRoles(newCustomerRoles);
            if (!string.IsNullOrEmpty(customerRolesError))
            {
                ModelState.AddModelError("", customerRolesError);
                ErrorNotification(customerRolesError, false);
            }

            //如果用户角色为"注册用户",需要确保输入的电子邮箱有效性
            if (newCustomerRoles.Any()
                && newCustomerRoles.FirstOrDefault(c => c.SystemName == SystemCustomerRoleNames.Registered) != null
                && !CommonHelper.IsValidEmail(model.Email))
            {
                ModelState.AddModelError("", _localizationService.GetResource("Admin.Customers.Customers.ValidEmailRequiredRegisteredRole"));
                ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.ValidEmailRequiredRegisteredRole"), false);
            }

            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Email = model.Email,
                    Username = model.Username,
                    AdminComment = model.AdminComment,
                    Active = model.Active,
                    CreatedOnUtc = DateTime.UtcNow,
                    LastActivityDateUtc = DateTime.UtcNow
                };
                _customerService.InsertCustomer(customer);

                _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Name, model.Name);
                if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.TimeZoneId, model.TimeZoneId);
                if (_customerSettings.GenderEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Gender, model.Gender);
                if (_customerSettings.DateOfBirthEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.DateOfBirth, model.DateOfBirth);
                if (_customerSettings.CompanyEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Company, model.Company);
                if (_customerSettings.PhoneEnabled)
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Phone, model.Phone);

                //用户密码
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    var changePassRequest = new ChangePasswordRequest(model.Email, false, _customerSettings.DefaultPasswordFormat, model.Password);
                    var changePassResult = _customerRegistrationService.ChangePassword(changePassRequest);
                    if (!changePassResult.Success)
                    {
                        foreach (var changePassError in changePassResult.Errors)
                            ErrorNotification(changePassError);
                    }
                }

                //用户角色
                foreach (var customerRole in newCustomerRoles)
                {
                    //确保当前用户不为"管理员"时，添加的角色将不能为"管理员"
                    if (customerRole.SystemName == SystemCustomerRoleNames.Administrators &&
                        !_workContext.CurrentCustomer.IsAdmin())
                        continue;

                    customer.CustomerRoles.Add(customerRole);
                }
                _customerService.UpdateCustomer(customer);

                _customerActivityService.InsertActivity("AddNewCustomer", _localizationService.GetResource("ActivityLog.AddNewCustomer"), customer.Id);

                SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Added"));

                if (continueEditing)
                {
                    SaveSelectedTabName();
                    return RedirectToAction("Edit", new { id = customer.Id });
                }
                return RedirectToAction("List");
            }

            PrepareCustomerModel(model, null, true);
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null || customer.Deleted)
                return RedirectToAction("List");

            var model = new CustomerModel();
            PrepareCustomerModel(model, customer, false);
            return View(model);
        }

        [ValidateInput(false)]
        [FormValueRequired("save", "save-continue")]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Edit(CustomerModel model, bool continueEditing, FormCollection form)
        {
            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null || customer.Deleted)
                return RedirectToAction("List");

            //验证用户角色
            var allCustomerRoles = _customerService.GetAllCustomerRoles(true);
            var newCustomerRoles = new List<CustomerRole>();
            foreach (var customerRole in allCustomerRoles)
                if (model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                    newCustomerRoles.Add(customerRole);
            var customerRolesError = ValidateCustomerRoles(newCustomerRoles);
            if (!string.IsNullOrEmpty(customerRolesError))
            {
                ModelState.AddModelError("", customerRolesError);
                ErrorNotification(customerRolesError, false);
            }

            //如果用户角色为"注册用户",需要确保输入的电子邮箱有效性
            if (newCustomerRoles.Any() && newCustomerRoles.FirstOrDefault(c => c.SystemName == SystemCustomerRoleNames.Registered) != null && !CommonHelper.IsValidEmail(model.Email))
            {
                ModelState.AddModelError("", _localizationService.GetResource("Admin.Customers.Customers.ValidEmailRequiredRegisteredRole"));
                ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.ValidEmailRequiredRegisteredRole"), false);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customer.AdminComment = model.AdminComment;

                    //阻止最后一个"管理员"角色用户被禁用
                    if (!customer.IsAdmin() || model.Active || SecondAdminAccountExists(customer))
                        customer.Active = model.Active;
                    else
                        ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.AdminAccountShouldExists.Deactivate"));

                    if (!string.IsNullOrWhiteSpace(model.Email))
                        _customerRegistrationService.SetEmail(customer, model.Email, false);
                    else
                        //游客用户...
                        customer.Email = model.Email;

                    if (!string.IsNullOrWhiteSpace(model.Username))
                        _customerRegistrationService.SetUsername(customer, model.Username);
                    else
                        //游客用户...
                        customer.Username = model.Username;

                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Name, model.Name);
                    if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.TimeZoneId, model.TimeZoneId);
                    if (_customerSettings.GenderEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Gender, model.Gender);
                    if (_customerSettings.DateOfBirthEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.DateOfBirth, model.DateOfBirth);
                    if (_customerSettings.CompanyEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Company, model.Company);
                    if (_customerSettings.PhoneEnabled)
                        _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Phone, model.Phone);

                    foreach (var customerRole in allCustomerRoles)
                    {
                        if (customerRole.SystemName == SystemCustomerRoleNames.Administrators &&
                            !_workContext.CurrentCustomer.IsAdmin())
                            continue;

                        if (model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                        {
                            //没有该角色身份时则添加
                            if (customer.CustomerRoles.Count(cr => cr.Id == customerRole.Id) == 0)
                                customer.CustomerRoles.Add(customerRole);
                        }
                        else
                        {
                            //确保当删除管理员角色身份时存在第二个管理员角色对象
                            if (customerRole.SystemName == SystemCustomerRoleNames.Administrators
                                && !SecondAdminAccountExists(customer))
                            {
                                ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.AdminAccountShouldExists.DeleteRole"));
                                continue;
                            }

                            //如果存在该角色身份时则清除
                            if (customer.CustomerRoles.Count(cr => cr.Id == customerRole.Id) > 0)
                                customer.CustomerRoles.Remove(customerRole);
                        }
                    }
                    _customerService.UpdateCustomer(customer);

                    _customerActivityService.InsertActivity("EditCustomer", _localizationService.GetResource("ActivityLog.EditCustomer"), customer.Id);

                    SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Updated"));

                    if (continueEditing)
                    {
                        SaveSelectedTabName();
                        return RedirectToAction("Edit", new { id = customer.Id });
                    }
                    return RedirectToAction("List");
                }
                catch (Exception exc)
                {
                    ErrorNotification(exc.Message, false);
                }
            }

            PrepareCustomerModel(model, customer, true);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("changepassword")]
        public virtual ActionResult ChangePassword(CustomerModel model)
        {
            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null)
                return RedirectToAction("List");

            //确保仅有管理员角色身份用户才可以修改其它管理员角色的密码
            if (customer.IsAdmin() && !_workContext.CurrentCustomer.IsAdmin())
            {
                ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.OnlyAdminCanChangePassword"));
                return RedirectToAction("Edit", new { id = customer.Id });
            }

            if (ModelState.IsValid)
            {
                var changePassRequest = new ChangePasswordRequest(customer.Email, false,
                    _customerSettings.DefaultPasswordFormat, model.Password);

                var changePassResult = _customerRegistrationService.ChangePassword(changePassRequest);

                if (changePassResult.Success)
                    SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.PasswordChanged"));
                else
                    foreach (var error in changePassResult.Errors)
                        ErrorNotification(error);
            }

            return RedirectToAction("Edit", new { id = customer.Id });
        }

        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null)
                return RedirectToAction("List");

            try
            {
                //阻止最后一个管理员角色用户被删除
                if (customer.IsAdmin() && !SecondAdminAccountExists(customer))
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.AdminAccountShouldExists.DeleteAdministrator"));
                    return RedirectToAction("Edit", new { id = customer.Id });
                }

                //确保管理员角色用户才允许删除管理员角色用户
                if (customer.IsAdmin() && !_workContext.CurrentCustomer.IsAdmin())
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.OnlyAdminCanDeleteAdmin"));
                    return RedirectToAction("Edit", new { id = customer.Id });
                }

                _customerService.DeleteCustomer(customer);

                _customerActivityService.InsertActivity("DeleteCustomer", _localizationService.GetResource("ActivityLog.DeleteCustomer"), customer.Id);

                SuccessNotification(_localizationService.GetResource("Admin.Customers.Customers.Deleted"));
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.Message);
                return RedirectToAction("Edit", new { id = customer.Id });
            }
        }

        #endregion

        #region Blog posts

        public virtual ActionResult ListBlogPost(DataSourceRequest command, int customerId)
        {
            var customerIds = new List<int> { customerId };
            var blogPost = _blogService.GetAllBlogPosts(command.Page - 1, command.PageSize,
                customerIds: customerIds, overridePublished: null);
            var gridModel = new DataSourceResult
            {
                Data = blogPost.Select(x =>
                {
                    var now = DateTime.UtcNow;
                    var m = new CustomerModel.BlogPostModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc),
                        UpdateOnUtc = _dateTimeHelper.ConvertToUserTime(x.UpdatedOnUtc, DateTimeKind.Utc),
                        Published = (x.AvailableStartDateUtc == null || now > x.AvailableStartDateUtc)
                            && (x.AvailableEndDateUtc == null || now < x.AvailableEndDateUtc),
                        Deleted = x.Deleted
                    };
                    return m;
                }),
                Total = blogPost.TotalCount
            };

            return Json(gridModel);
        }

        #endregion

        #region Blog comments



        #endregion

        #region  Activity logs

        [HttpPost]
        public virtual ActionResult ListActivityLog(DataSourceRequest command, int customerId)
        {
            var activityLog = _customerActivityService.GetAllActivities(null, null, customerId, 0, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = activityLog.Select(x =>
                {
                    var m = new CustomerModel.ActivityLogModel
                    {
                        Id = x.Id,
                        ActivityLogTypeName = x.ActivityLogType.Name,
                        Comment = x.Comment,
                        CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc),
                        IpAddress = x.IpAddress
                    };
                    return m;
                }),
                Total = activityLog.TotalCount
            };

            return Json(gridModel);
        }

        #endregion

        #region Reports

        public virtual ActionResult Reports()
        {
            return View();
        }

        [ChildActionOnly]
        public virtual ActionResult ReportRegisteredCustomers()
        {
            return PartialView();
        }

        [HttpPost]
        public virtual ActionResult ReportRegisteredCustomersList(DataSourceRequest command)
        {
            var model = GetReportRegisteredCustomersModel();
            var gridModel = new DataSourceResult
            {
                Data = model,
                Total = model.Count
            };

            return Json(gridModel);
        }

        #endregion

        #region Utilities

        [NonAction]
        protected virtual CustomerModel PrepareCustomerModelForList(Customer customer)
        {
            return new CustomerModel
            {
                Id = customer.Id,
                Email = customer.IsRegistered() ? customer.Email : _localizationService.GetResource("Admin.Customers.Guest"),
                Username = customer.Username,
                Name = customer.GetAttribute<string>(SystemCustomerAttributeNames.Name),
                Company = customer.GetAttribute<string>(SystemCustomerAttributeNames.Company),
                Phone = customer.GetAttribute<string>(SystemCustomerAttributeNames.Phone),
                CustomerRoleNames = GetCustomerRolesNames(customer.CustomerRoles.ToList()),
                Active = customer.Active,
                CreatedOn = _dateTimeHelper.ConvertToUserTime(customer.CreatedOnUtc, DateTimeKind.Utc),
                LastActivityDate = _dateTimeHelper.ConvertToUserTime(customer.LastActivityDateUtc, DateTimeKind.Utc)
            };
        }

        [NonAction]
        protected virtual string GetCustomerRolesNames(IList<CustomerRole> customerRoles, string separator = ",")
        {
            var sb = new StringBuilder();
            for (int i = 0; i < customerRoles.Count; i++)
            {
                sb.Append(customerRoles[i].Name);
                if (i != customerRoles.Count - 1)
                {
                    sb.Append(separator);
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        [NonAction]
        protected virtual void PrepareCustomerModel(CustomerModel model, Customer customer, bool excludeProperties)
        {
            if (customer != null)
            {
                model.Id = customer.Id;
                if (!excludeProperties)
                {
                    model.Email = customer.Email;
                    model.Username = customer.Username;
                    model.AdminComment = customer.AdminComment;
                    model.Active = customer.Active;

                    model.TimeZoneId = customer.GetAttribute<string>(SystemCustomerAttributeNames.TimeZoneId);
                    model.CreatedOn = _dateTimeHelper.ConvertToUserTime(customer.CreatedOnUtc, DateTimeKind.Utc);
                    model.LastActivityDate = _dateTimeHelper.ConvertToUserTime(customer.LastActivityDateUtc, DateTimeKind.Utc);
                    model.LastIpAddress = customer.LastIpAddress;
                    model.LastVisitedPage = customer.GetAttribute<string>(SystemCustomerAttributeNames.LastVisitedPage);

                    model.SelectedCustomerRoleIds = customer.CustomerRoles.Select(cr => cr.Id).ToList();

                    model.Name = customer.GetAttribute<string>(SystemCustomerAttributeNames.Name);
                    model.Gender = customer.GetAttribute<string>(SystemCustomerAttributeNames.Gender);
                    model.DateOfBirth = customer.GetAttribute<DateTime?>(SystemCustomerAttributeNames.DateOfBirth);
                    model.Company = customer.GetAttribute<string>(SystemCustomerAttributeNames.Company);
                    model.Phone = customer.GetAttribute<string>(SystemCustomerAttributeNames.Phone);
                }
            }

            model.AllowCustomersToSetTimeZone = _dateTimeSettings.AllowCustomersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem { Text = tzi.DisplayName, Value = tzi.Id, Selected = (tzi.Id == model.TimeZoneId) });

            model.GenderEnabled = _customerSettings.GenderEnabled;
            model.DateOfBirthEnabled = _customerSettings.DateOfBirthEnabled;
            model.CompanyEnabled = _customerSettings.CompanyEnabled;
            model.PhoneEnabled = _customerSettings.PhoneEnabled;

            //用户权限模块
            var allRoles = _customerService.GetAllCustomerRoles(true);
            var customerRole = allRoles.FirstOrDefault(c => c.SystemName == SystemCustomerRoleNames.Registered);
            if (customer == null && customerRole != null)
                model.SelectedCustomerRoleIds.Add(customerRole.Id);

            foreach (var role in allRoles)
            {
                model.AvailableCustomerRoles.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Id.ToString(),
                    Selected = model.SelectedCustomerRoleIds.Contains(role.Id)
                });
            }

            //外部站点授权记录

        }

        [NonAction]
        protected virtual string ValidateCustomerRoles(IList<CustomerRole> customerRoles)
        {
            if (customerRoles == null)
                throw new ArgumentNullException(nameof(customerRoles));

            //确保用户不同时存在于"游客"与"注册用户"角色中
            //确保用户至少需要一个"游客"或"注册用户"角色
            bool isInGuestsRole = customerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Guests) != null;
            bool isInRegisteredRole = customerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Registered) != null;
            if (isInGuestsRole && isInRegisteredRole)
                return _localizationService.GetResource("Admin.Customers.Customers.GuestsAndRegisteredRolesError");
            if (!isInGuestsRole && !isInRegisteredRole)
                return _localizationService.GetResource("Admin.Customers.Customers.AddCustomerToGuestsOrRegisteredRoleError");

            return "";
        }

        [NonAction]
        private bool SecondAdminAccountExists(Customer customer)
        {
            var customers = _customerService
                .GetAllCustomers(customerRoleIds: new[]
                {
                    _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Administrators).Id
                });

            return customers.Any(c => c.Active && c.Id != customer.Id);
        }

        [NonAction]
        protected virtual IList<RegisteredCustomerReportLineModel> GetReportRegisteredCustomersModel()
        {
            var report = new List<RegisteredCustomerReportLineModel>();
            report.Add(new RegisteredCustomerReportLineModel
            {
                Period = _localizationService.GetResource("Admin.Customers.Reports.RegisteredCustomers.Fields.Period.7days"),
                Customers = _customerReportService.GetRegisteredCustomersReport(7)
            });

            report.Add(new RegisteredCustomerReportLineModel
            {
                Period = _localizationService.GetResource("Admin.Customers.Reports.RegisteredCustomers.Fields.Period.14days"),
                Customers = _customerReportService.GetRegisteredCustomersReport(14)
            });
            report.Add(new RegisteredCustomerReportLineModel
            {
                Period = _localizationService.GetResource("Admin.Customers.Reports.RegisteredCustomers.Fields.Period.month"),
                Customers = _customerReportService.GetRegisteredCustomersReport(30)
            });
            report.Add(new RegisteredCustomerReportLineModel
            {
                Period = _localizationService.GetResource("Admin.Customers.Reports.RegisteredCustomers.Fields.Period.year"),
                Customers = _customerReportService.GetRegisteredCustomersReport(365)
            });

            return report;
        }

        #endregion

    }
}