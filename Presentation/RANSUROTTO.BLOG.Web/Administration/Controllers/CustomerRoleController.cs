using System;
using System.Linq;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Extensions;
using RANSUROTTO.BLOG.Admin.Models.Customers;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Framework.Controllers;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Services.Customers;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class CustomerRoleController : BaseAdminController
    {

        #region Fields

        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;

        #endregion

        #region Constructor

        public CustomerRoleController(ICustomerService customerService, ILocalizationService localizationService, ICustomerActivityService customerActivityService)
        {
            _customerService = customerService;
            _localizationService = localizationService;
            _customerActivityService = customerActivityService;
        }

        #endregion

        #region Methods

        public virtual ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult List(DataSourceRequest command)
        {
            var customerRoles = _customerService.GetAllCustomerRoles(true);
            var gridModel = new DataSourceResult
            {
                Data = customerRoles.Select(p => p.ToModel()),
                Total = customerRoles.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult Create()
        {
            var model = new CustomerRoleModel
            {
                //设置默认启用
                Active = true
            };

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Create(CustomerRoleModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var customerRole = model.ToEntity();
                _customerService.InsertCustomerRole(customerRole);

                _customerActivityService.InsertActivity("AddNewCustomerRole", _localizationService.GetResource("ActivityLog.AddNewCustomerRole"), customerRole.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerRoles.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = customerRole.Id }) : RedirectToAction("List");
            }
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var customerRole = _customerService.GetCustomerRoleById(id);
            if (customerRole == null)
                return RedirectToAction("List");

            var model = customerRole.ToModel();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Edit(CustomerRoleModel model, bool continueEditing)
        {
            var customerRole = _customerService.GetCustomerRoleById(model.Id);
            if (customerRole == null)
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    //确保系统角色不被禁用
                    if (customerRole.IsSystemRole && !model.Active)
                        throw new SiteException(_localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.Active.CantEditSystem"));

                    //确保系统角色系统名不被修改
                    if (customerRole.IsSystemRole && !customerRole.SystemName.Equals(model.SystemName, StringComparison.InvariantCultureIgnoreCase))
                        throw new SiteException(_localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.SystemName.CantEditSystem"));

                    customerRole = model.ToEntity(customerRole);
                    _customerService.UpdateCustomerRole(customerRole);

                    _customerActivityService.InsertActivity("EditCustomerRole", _localizationService.GetResource("ActivityLog.EditCustomerRole"), customerRole.Name);

                    SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerRoles.Updated"));
                    return continueEditing ? RedirectToAction("Edit", new { id = customerRole.Id }) : RedirectToAction("List");
                }
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = customerRole.Id });
            }
        }

        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            var customerRole = _customerService.GetCustomerRoleById(id);
            if (customerRole == null)
                return RedirectToAction("List");

            try
            {
                _customerService.DeleteCustomerRole(customerRole);

                _customerActivityService.InsertActivity("DeleteCustomerRole", _localizationService.GetResource("ActivityLog.DeleteCustomerRole"), customerRole.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerRoles.Deleted"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = customerRole.Id });
            }
        }

        #endregion

    }
}