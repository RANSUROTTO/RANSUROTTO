using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Models.Settings;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain;
using RANSUROTTO.BLOG.Core.Domain.Common.Setting;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;
using RANSUROTTO.BLOG.Core.Domain.Localization.Setting;
using RANSUROTTO.BLOG.Core.Domain.Media.Setting;
using RANSUROTTO.BLOG.Core.Domain.Security.Setting;
using RANSUROTTO.BLOG.Core.Domain.Seo.Enum;
using RANSUROTTO.BLOG.Core.Domain.Seo.Setting;
using RANSUROTTO.BLOG.Framework.Controllers;
using RANSUROTTO.BLOG.Framework.Extensions;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Services.Common;
using RANSUROTTO.BLOG.Services.Configuration;
using RANSUROTTO.BLOG.Services.Customers;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;
using RANSUROTTO.BLOG.Services.Security;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class SettingController : BaseAdminController
    {

        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IEncryptionService _encryptionService;
        private readonly ISettingService _settingService;

        #endregion

        #region Constructor

        public SettingController(IGenericAttributeService genericAttributeService, IWorkContext workContext, ILocalizationService localizationService, ICustomerService customerService, ICustomerActivityService customerActivityService, IEncryptionService encryptionService, ISettingService settingService)
        {
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
            _localizationService = localizationService;
            _customerService = customerService;
            _customerActivityService = customerActivityService;
            _encryptionService = encryptionService;
            _settingService = settingService;
        }

        #endregion

        #region Methods

        [ChildActionOnly]
        public virtual ActionResult Mode(string modeName = "settings-advanced-mode")
        {
            var model = new ModeModel
            {
                ModeName = modeName,
                Enabled = _workContext.CurrentCustomer.GetAttribute<bool>(modeName)
            };
            return PartialView(model);
        }

        public virtual ActionResult GeneralCommon()
        {
            this.Server.ScriptTimeout = 300;

            var model = new GeneralCommonSettingsModel();
            //blog information
            var blogInformationSettings = _settingService.LoadSetting<BlogInformationSettings>();
            var commonSettings = _settingService.LoadSetting<CommonSettings>();
            model.BlogInformationSettings.BlogClosed = blogInformationSettings.BlogClosed;
            //themes
            //TODO Themes Settings
            //EU Cookie law
            model.BlogInformationSettings.DisplayEuCookieLawWarning = blogInformationSettings.DisplayEuCookieLawWarning;
            //social pages
            model.BlogInformationSettings.GitHubLink = blogInformationSettings.GitHubLink;
            //sitemap
            model.BlogInformationSettings.SitemapEnabled = commonSettings.SitemapEnabled;

            //seo settings
            var seoSettings = _settingService.LoadSetting<SeoSettings>();
            model.SeoSettings.PageTitleSeparator = seoSettings.PageTitleSeparator;
            model.SeoSettings.PageTitleSeoAdjustment = (int)seoSettings.PageTitleSeoAdjustment;
            model.SeoSettings.PageTitleSeoAdjustmentValues = seoSettings.PageTitleSeoAdjustment.ToSelectList();
            model.SeoSettings.DefaultTitle = seoSettings.DefaultTitle;
            model.SeoSettings.DefaultMetaKeywords = seoSettings.DefaultMetaKeywords;
            model.SeoSettings.DefaultMetaDescription = seoSettings.DefaultMetaDescription;
            model.SeoSettings.WwwRequirement = (int)seoSettings.WwwRequirement;
            model.SeoSettings.WwwRequirementValues = seoSettings.WwwRequirement.ToSelectList();
            model.SeoSettings.EnableJsBundling = seoSettings.EnableJsBundling;
            model.SeoSettings.EnableCssBundling = seoSettings.EnableCssBundling;
            model.SeoSettings.CustomHeadTags = seoSettings.CustomHeadTags;

            //security settings
            var securitySettings = _settingService.LoadSetting<SecuritySettings>();
            model.SecuritySettings.EncryptionKey = securitySettings.EncryptionKey;
            if (securitySettings.AdminAreaAllowedIpAddresses != null)
                for (int i = 0; i < securitySettings.AdminAreaAllowedIpAddresses.Count; i++)
                {
                    model.SecuritySettings.AdminAreaAllowedIpAddresses += securitySettings.AdminAreaAllowedIpAddresses[i];
                    if (i != securitySettings.AdminAreaAllowedIpAddresses.Count - 1)
                        model.SecuritySettings.AdminAreaAllowedIpAddresses += ",";
                }
            model.SecuritySettings.ForceSslForAllPages = securitySettings.ForceSslForAllPages;
            model.SecuritySettings.EnableXsrfProtectionForAdminArea = securitySettings.EnableXsrfProtectionForAdminArea;
            model.SecuritySettings.EnableXsrfProtectionForPublicArea = securitySettings.EnableXsrfProtectionForPublicArea;
            model.SecuritySettings.HoneypotEnabled = securitySettings.HoneypotEnabled;

            //captcha settings
            var captchaSettings = _settingService.LoadSetting<CaptchaSettings>();
            model.CaptchaSettings.Enabled = captchaSettings.Enabled;
            model.CaptchaSettings.ShowOnLoginPage = captchaSettings.ShowOnLoginPage;
            model.CaptchaSettings.ShowOnPublishBlogCommentPage = captchaSettings.ShowOnPublishBlogCommentPage;
            model.CaptchaSettings.ShowOnPublishBlogPostPage = captchaSettings.ShowOnPublishBlogPostPage;
            model.CaptchaSettings.ShowOnRegistrationPage = captchaSettings.ShowOnRegistrationPage;

            //localization
            var localizationSettings = _settingService.LoadSetting<LocalizationSettings>();
            model.LocalizationSettings.UseImagesForLanguageSelection = localizationSettings.UseImagesForLanguageSelection;
            model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled = localizationSettings.SeoFriendlyUrlsForLanguagesEnabled;
            model.LocalizationSettings.AutomaticallyDetectLanguage = localizationSettings.AutomaticallyDetectLanguage;
            model.LocalizationSettings.LoadAllLocaleRecordsOnStartup = localizationSettings.LoadAllLocaleRecordsOnStartup;
            model.LocalizationSettings.LoadAllLocalizedPropertiesOnStartup = localizationSettings.LoadAllLocalizedPropertiesOnStartup;
            model.LocalizationSettings.LoadAllUrlRecordsOnStartup = localizationSettings.LoadAllUrlRecordsOnStartup;

            return View(model);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public virtual ActionResult GeneralCommon(GeneralCommonSettingsModel model)
        {
            //blog information settings
            var blogInformationSettings = _settingService.LoadSetting<BlogInformationSettings>();
            var commonSettings = _settingService.LoadSetting<CommonSettings>();
            blogInformationSettings.BlogClosed = model.BlogInformationSettings.BlogClosed;
            //EU Cookie law
            blogInformationSettings.DisplayEuCookieLawWarning = model.BlogInformationSettings.DisplayEuCookieLawWarning;
            //social pages
            blogInformationSettings.GitHubLink = model.BlogInformationSettings.GitHubLink;
            //sitemap
            commonSettings.SitemapEnabled = model.BlogInformationSettings.SitemapEnabled;

            _settingService.SaveSetting(blogInformationSettings, settings => settings.BlogClosed, false);
            _settingService.SaveSetting(blogInformationSettings, settings => settings.DisplayEuCookieLawWarning, false);
            _settingService.SaveSetting(blogInformationSettings, settings => settings.GitHubLink, false);
            _settingService.SaveSetting(commonSettings, settings => settings.SitemapEnabled, false);
            _settingService.ClearCache();

            //seo settings
            var seoSettings = _settingService.LoadSetting<SeoSettings>();
            seoSettings.PageTitleSeparator = model.SeoSettings.PageTitleSeparator;
            seoSettings.PageTitleSeoAdjustment = (PageTitleSeoAdjustment)model.SeoSettings.PageTitleSeoAdjustment;
            seoSettings.DefaultTitle = model.SeoSettings.DefaultTitle;
            seoSettings.DefaultMetaKeywords = model.SeoSettings.DefaultMetaKeywords;
            seoSettings.DefaultMetaDescription = model.SeoSettings.DefaultMetaDescription;
            seoSettings.WwwRequirement = (WwwRequirement)model.SeoSettings.WwwRequirement;
            seoSettings.EnableJsBundling = model.SeoSettings.EnableJsBundling;
            seoSettings.EnableCssBundling = model.SeoSettings.EnableCssBundling;
            seoSettings.CustomHeadTags = model.SeoSettings.CustomHeadTags;
            _settingService.SaveSetting(seoSettings);

            //security settings
            var securitySettings = _settingService.LoadSetting<SecuritySettings>();
            if (securitySettings.AdminAreaAllowedIpAddresses == null)
                securitySettings.AdminAreaAllowedIpAddresses = new List<string>();
            securitySettings.AdminAreaAllowedIpAddresses.Clear();
            if (!string.IsNullOrEmpty(model.SecuritySettings.AdminAreaAllowedIpAddresses))
                foreach (string s in model.SecuritySettings.AdminAreaAllowedIpAddresses.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    if (!string.IsNullOrWhiteSpace(s))
                        securitySettings.AdminAreaAllowedIpAddresses.Add(s.Trim());
            securitySettings.ForceSslForAllPages = model.SecuritySettings.ForceSslForAllPages;
            securitySettings.EnableXsrfProtectionForAdminArea = model.SecuritySettings.EnableXsrfProtectionForAdminArea;
            securitySettings.EnableXsrfProtectionForPublicArea = model.SecuritySettings.EnableXsrfProtectionForPublicArea;
            securitySettings.HoneypotEnabled = model.SecuritySettings.HoneypotEnabled;
            _settingService.SaveSetting(securitySettings);

            //captcha settings
            var captchaSettings = _settingService.LoadSetting<CaptchaSettings>();
            captchaSettings.Enabled = model.CaptchaSettings.Enabled;
            captchaSettings.ShowOnLoginPage = model.CaptchaSettings.ShowOnLoginPage;
            captchaSettings.ShowOnPublishBlogCommentPage = model.CaptchaSettings.ShowOnPublishBlogCommentPage;
            captchaSettings.ShowOnPublishBlogPostPage = model.CaptchaSettings.ShowOnPublishBlogPostPage;
            captchaSettings.ShowOnRegistrationPage = model.CaptchaSettings.ShowOnRegistrationPage;
            _settingService.SaveSetting(captchaSettings);

            //localize
            var localizationSettings = _settingService.LoadSetting<LocalizationSettings>();
            localizationSettings.UseImagesForLanguageSelection = model.LocalizationSettings.UseImagesForLanguageSelection;
            if (localizationSettings.SeoFriendlyUrlsForLanguagesEnabled != model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                localizationSettings.SeoFriendlyUrlsForLanguagesEnabled = model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled;
                //clear cached values of routes
                System.Web.Routing.RouteTable.Routes.ClearSeoFriendlyUrlsCachedValueForRoutes();
            }
            localizationSettings.AutomaticallyDetectLanguage = model.LocalizationSettings.AutomaticallyDetectLanguage;
            localizationSettings.LoadAllLocaleRecordsOnStartup = model.LocalizationSettings.LoadAllLocaleRecordsOnStartup;
            localizationSettings.LoadAllLocalizedPropertiesOnStartup = model.LocalizationSettings.LoadAllLocalizedPropertiesOnStartup;
            localizationSettings.LoadAllUrlRecordsOnStartup = model.LocalizationSettings.LoadAllUrlRecordsOnStartup;
            _settingService.SaveSetting(localizationSettings);

            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("GeneralCommon");
        }

        [HttpPost, ActionName("GeneralCommon")]
        [FormValueRequired("changeencryptionkey")]
        public virtual ActionResult ChangeEncryptionKey(GeneralCommonSettingsModel model)
        {
            this.Server.ScriptTimeout = 300;

            var securitySettings = _settingService.LoadSetting<SecuritySettings>();
            try
            {
                if (model.SecuritySettings.EncryptionKey == null)
                    model.SecuritySettings.EncryptionKey = "";

                model.SecuritySettings.EncryptionKey = model.SecuritySettings.EncryptionKey.Trim();

                var newEncryptionPrivateKey = model.SecuritySettings.EncryptionKey;
                if (string.IsNullOrEmpty(newEncryptionPrivateKey) || newEncryptionPrivateKey.Length != 16)
                    throw new SiteException(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.TooShort"));

                string oldEncryptionPrivateKey = securitySettings.EncryptionKey;
                if (oldEncryptionPrivateKey == newEncryptionPrivateKey)
                    throw new SiteException(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.TheSame"));

                //更改所有该加密格式的用户密码密钥
                var customerPasswords = _customerService.GetCustomerPasswords(passwordFormat: PasswordFormat.Encrypted);
                foreach (var customerPassword in customerPasswords)
                {
                    var decryptedPassword = _encryptionService.DecryptText(customerPassword.Password, oldEncryptionPrivateKey);
                    var encryptedPassword = _encryptionService.EncryptText(decryptedPassword, newEncryptionPrivateKey);

                    customerPassword.Password = encryptedPassword;
                    _customerService.UpdateCustomerPassword(customerPassword);
                }

                securitySettings.EncryptionKey = newEncryptionPrivateKey;
                _settingService.SaveSetting(securitySettings);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.Changed"));
            }
            catch (Exception e)
            {
                ErrorNotification(e);
            }
            return RedirectToAction("GeneralCommon");
        }

        public virtual ActionResult Media()
        {
            var mediaSettings = _settingService.LoadSetting<MediaSettings>();
            var model = new MediaSettingsModel
            {
                MaximumImageSize = mediaSettings.MaximumImageSize,
                DefaultImageQuality = mediaSettings.DefaultImageQuality,
                MultipleThumbDirectories = mediaSettings.MultipleThumbDirectories,
                DefaultPictureZoomEnabled = mediaSettings.DefaultPictureZoomEnabled,
                AvatarPictureSize = mediaSettings.AvatarPictureSize
            };
            return View(model);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public virtual ActionResult Media(MediaSettingsModel model)
        {
            var mediaSettings = _settingService.LoadSetting<MediaSettings>();

            mediaSettings.MaximumImageSize = mediaSettings.MaximumImageSize;
            mediaSettings.DefaultImageQuality = mediaSettings.DefaultImageQuality;
            mediaSettings.MultipleThumbDirectories = mediaSettings.MultipleThumbDirectories;
            mediaSettings.DefaultPictureZoomEnabled = mediaSettings.DefaultPictureZoomEnabled;
            mediaSettings.AvatarPictureSize = mediaSettings.AvatarPictureSize;
            _settingService.SaveSetting(mediaSettings);

            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("Media");
        }

        #endregion

    }
}