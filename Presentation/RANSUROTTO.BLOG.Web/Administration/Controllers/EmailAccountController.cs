using System;
using System.Linq;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Extensions;
using RANSUROTTO.BLOG.Admin.Models.Messages;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Domain.Messages.Setting;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Framework.Controllers;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Services.Configuration;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;
using RANSUROTTO.BLOG.Services.Messages;
using RANSUROTTO.BLOG.Services.Security;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class EmailAccountController : BaseAdminController
    {

        #region Fields

        private readonly IEmailSender _emailSender;
        private readonly ISettingService _settingService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;

        #endregion

        #region Constructor

        public EmailAccountController(IEmailSender emailSender, ISettingService settingService, EmailAccountSettings emailAccountSettings, IEmailAccountService emailAccountService, IPermissionService permissionService, ILocalizationService localizationService, ICustomerActivityService customerActivityService)
        {
            _emailSender = emailSender;
            _settingService = settingService;
            _emailAccountSettings = emailAccountSettings;
            _emailAccountService = emailAccountService;
            _permissionService = permissionService;
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
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            return View();
        }

        [HttpPost]
        public virtual ActionResult List(DataSourceRequest request)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedKendoGridJson();

            var emailAccountModels = _emailAccountService.GetAllEmailAccounts()
                .Select(x => x.ToModel())
                .ToList();
            foreach (var eam in emailAccountModels)
                eam.IsDefaultEmailAccount = eam.Id == _emailAccountSettings.DefaultEmailAccountId;

            var gridModel = new DataSourceResult
            {
                Data = emailAccountModels,
                Total = emailAccountModels.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult MarkAsDefaultEmail(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            var defaultEmailAccount = _emailAccountService.GetEmailAccountById(id);
            if (defaultEmailAccount != null)
            {
                _emailAccountSettings.DefaultEmailAccountId = defaultEmailAccount.Id;
                _settingService.SaveSetting(_emailAccountSettings);
            }
            return RedirectToAction("List");
        }

        public virtual ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            var model = new EmailAccountModel { Port = 25 };
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Create(EmailAccountModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var emailAccount = model.ToEntity();
                //手动设置密码
                emailAccount.Password = model.Password;

                _emailAccountService.InsertEmailAccount(emailAccount);
                _customerActivityService.InsertActivity("AddNewEmailAccount", _localizationService.GetResource("ActivityLog.AddNewEmailAccount"), emailAccount.Id);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.EmailAccounts.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = emailAccount.Id }) : RedirectToAction("List");
            }

            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            var emailAccount = _emailAccountService.GetEmailAccountById(id);
            if (emailAccount == null)
                return RedirectToAction("List");

            return View(emailAccount.ToModel());
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual ActionResult Edit(EmailAccountModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            var emailAccount = _emailAccountService.GetEmailAccountById(model.Id);
            if (emailAccount == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                emailAccount = model.ToEntity(emailAccount);
                _emailAccountService.UpdateEmailAccount(emailAccount);

                _customerActivityService.InsertActivity("EditEmailAccount", _localizationService.GetResource("ActivityLog.EditEmailAccount"), emailAccount.Id);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.EmailAccounts.Updated"));
                return continueEditing ? RedirectToAction("Edit", new { id = emailAccount.Id }) : RedirectToAction("List");
            }

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("changepassword")]
        public virtual ActionResult ChangePassword(EmailAccountModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            var emailAccount = _emailAccountService.GetEmailAccountById(model.Id);
            if (emailAccount == null)
                return RedirectToAction("List");

            //仅更改密码项
            emailAccount.Password = model.Password;
            _emailAccountService.UpdateEmailAccount(emailAccount);

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.EmailAccounts.Fields.Password.PasswordChanged"));
            return RedirectToAction("Edit", new { id = emailAccount.Id });
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("sendtestemail")]
        public virtual ActionResult SendTestEmail(EmailAccountModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            var emailAccount = _emailAccountService.GetEmailAccountById(model.Id);
            if (emailAccount == null)
                return RedirectToAction("List");

            if (!CommonHelper.IsValidEmail(model.SendTestEmailTo))
            {
                ErrorNotification(_localizationService.GetResource("Admin.Common.WrongEmail"), false);
                return View(model);
            }

            try
            {
                if (String.IsNullOrWhiteSpace(model.SendTestEmailTo))
                    throw new SiteException("输入测试邮件地址。");

                string subject = "RANSUROTTO.BLOG 测试电子邮件功能";
                string body = "电子邮件功能正常工作。";
                _emailSender.SendEmail(emailAccount, subject, body, emailAccount.Email, emailAccount.DisplayName, model.SendTestEmailTo, null);
                SuccessNotification(_localizationService.GetResource("Admin.Configuration.EmailAccounts.SendTestEmail.Success"), false);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message, false);
            }

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageEmailAccounts))
                return AccessDeniedView();

            var emailAccount = _emailAccountService.GetEmailAccountById(id);
            if (emailAccount == null)
                return RedirectToAction("List");

            try
            {
                _emailAccountService.DeleteEmailAccount(emailAccount);
                _customerActivityService.InsertActivity("DeleteEmailAccount", _localizationService.GetResource("ActivityLog.DeleteEmailAccount"), emailAccount.Id);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.EmailAccounts.Deleted"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = emailAccount.Id });
            }
        }

        #endregion

    }
}