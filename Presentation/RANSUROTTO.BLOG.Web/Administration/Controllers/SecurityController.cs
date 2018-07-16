using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Services.Logging;
using RANSUROTTO.BLOG.Services.Security;
using RANSUROTTO.BLOG.Services.Customers;
using RANSUROTTO.BLOG.Admin.Models.Security;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Admin.Models.Customers;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class SecurityController : BaseAdminController
    {

        #region Fields

        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;
        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Constructor

        public SecurityController(ILogger logger, IWorkContext workContext, IPermissionService permissionService, ICustomerService customerService, ILocalizationService localizationService)
        {
            _logger = logger;
            _workContext = workContext;
            _permissionService = permissionService;
            _customerService = customerService;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        public virtual ActionResult AccessDenied(string pageUrl)
        {
            var currentCustomer = _workContext.CurrentCustomer;
            if (currentCustomer == null || currentCustomer.IsGuest())
            {
                _logger.Information(string.Format("Access denied to anonymous request on {0}", pageUrl));
                return View();
            }

            _logger.Information(string.Format("Access denied to user #{0} '{1}' on {2}", currentCustomer.Email, currentCustomer.Email, pageUrl));

            return View();
        }

        public virtual ActionResult Permissions()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcl))
                return AccessDeniedView();

            var model = new PermissionMappingModel();

            var permissionRecords = _permissionService.GetAllPermissionRecords();
            var customerRoles = _customerService.GetAllCustomerRoles(true);

            // 填充所有权限记录项
            foreach (var pr in permissionRecords)
            {
                model.AvailablePermissions.Add(new PermissionRecordModel
                {
                    //Name = pr.Name,
                    Name = pr.GetLocalizedPermissionName(_localizationService, _workContext),
                    SystemName = pr.SystemName
                });
            }
            // 填充所有权限角色项
            foreach (var cr in customerRoles)
            {
                model.AvailableCustomerRoles.Add(new CustomerRoleModel
                {
                    Id = cr.Id,
                    Name = cr.Name
                });
            }
            // 填充权限记录和权限角色匹配项
            foreach (var pr in permissionRecords)
                foreach (var cr in customerRoles)
                {
                    bool allowed = pr.CustomerRoles.Count(x => x.Id == cr.Id) > 0;
                    if (!model.Allowed.ContainsKey(pr.SystemName))
                        model.Allowed[pr.SystemName] = new Dictionary<long, bool>();
                    model.Allowed[pr.SystemName][cr.Id] = allowed;
                }

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Permissions(FormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcl))
                return AccessDeniedKendoGridJson();

            var permissionRecords = _permissionService.GetAllPermissionRecords();
            var customerRoles = _customerService.GetAllCustomerRoles(true);

            foreach (var cr in customerRoles)
            {
                string formKey = "allow_" + cr.Id;
                var permissionRecordSystemNamesToRestrict = form[formKey] != null ?
                    form[formKey].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                    : new List<string>();

                foreach (var pr in permissionRecords)
                {
                    /*遍历权限记录项,如果选中有则添加匹配项,没有选中则移除匹配项*/
                    bool allow = permissionRecordSystemNamesToRestrict.Contains(pr.SystemName);
                    if (allow)
                    {
                        if (pr.CustomerRoles.FirstOrDefault(x => x.Id == cr.Id) == null)
                        {
                            pr.CustomerRoles.Add(cr);
                            _permissionService.UpdatePermissionRecord(pr);
                        }
                    }
                    else
                    {
                        if (pr.CustomerRoles.FirstOrDefault(x => x.Id == cr.Id) != null)
                        {
                            pr.CustomerRoles.Remove(cr);
                            _permissionService.UpdatePermissionRecord(pr);
                        }
                    }
                }
            }

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.ACL.Updated"));
            return RedirectToAction("Permissions");
        }

        #endregion

    }
}