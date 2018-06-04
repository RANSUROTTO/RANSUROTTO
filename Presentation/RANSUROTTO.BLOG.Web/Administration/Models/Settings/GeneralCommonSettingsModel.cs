using System.Web.Mvc;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Settings
{
    public class GeneralCommonSettingsModel : BaseModel
    {
        public GeneralCommonSettingsModel()
        {
            BlogInformationSettings = new BlogInformationSettingsModel();
            SeoSettings = new SeoSettingsModel();
            SecuritySettings = new SecuritySettingsModel();
            CaptchaSettings = new CaptchaSettingsModel();
            LocalizationSettings = new LocalizationSettingsModel();
        }

        public BlogInformationSettingsModel BlogInformationSettings { get; set; }
        public SeoSettingsModel SeoSettings { get; set; }
        public SecuritySettingsModel SecuritySettings { get; set; }
        public CaptchaSettingsModel CaptchaSettings { get; set; }
        public LocalizationSettingsModel LocalizationSettings { get; set; }

        #region Nested classes

        public class BlogInformationSettingsModel : BaseModel
        {
            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayEuCookieLawWarning")]
            public bool DisplayEuCookieLawWarning { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.BlogClosed")]
            public bool BlogClosed { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SitemapEnabled")]
            public bool SitemapEnabled { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.GitHubLink")]
            public string GitHubLink { get; set; }

        }

        public class SeoSettingsModel : BaseModel
        {
            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PageTitleSeparator")]
            [AllowHtml]
            [NoTrim]
            public string PageTitleSeparator { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PageTitleSeoAdjustment")]
            public int PageTitleSeoAdjustment { get; set; }
            public SelectList PageTitleSeoAdjustmentValues { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DefaultTitle")]
            [AllowHtml]
            public string DefaultTitle { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DefaultMetaKeywords")]
            [AllowHtml]
            public string DefaultMetaKeywords { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DefaultMetaDescription")]
            [AllowHtml]
            public string DefaultMetaDescription { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.EnableJsBundling")]
            public bool EnableJsBundling { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.EnableCssBundling")]
            public bool EnableCssBundling { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.WwwRequirement")]
            public int WwwRequirement { get; set; }
            public SelectList WwwRequirementValues { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.ConvertNonWesternChars")]
            public bool ConvertNonWesternChars { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CanonicalUrlsEnabled")]
            public bool CanonicalUrlsEnabled { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CustomHeadTags")]
            [AllowHtml]
            public string CustomHeadTags { get; set; }

        }

        public class SecuritySettingsModel : BaseModel
        {
            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.EncryptionKey")]
            [AllowHtml]
            public string EncryptionKey { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.AdminAreaAllowedIpAddresses")]
            [AllowHtml]
            public string AdminAreaAllowedIpAddresses { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.ForceSslForAllPages")]
            public bool ForceSslForAllPages { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.EnableXSRFProtectionForAdminArea")]
            public bool EnableXsrfProtectionForAdminArea { get; set; }
            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.EnableXSRFProtectionForPublicArea")]
            public bool EnableXsrfProtectionForPublicArea { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.HoneypotEnabled")]
            public bool HoneypotEnabled { get; set; }
        }

        public class CaptchaSettingsModel
        {

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaEnabled")]
            public bool Enabled { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.ShowOnLoginPage")]
            public bool ShowOnLoginPage { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.ShowOnRegistrationPage")]
            public bool ShowOnRegistrationPage { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.ShowOnPublishBlogPostPage")]
            public bool ShowOnPublishBlogPostPage { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.ShowOnPublishBlogCommentPage")]
            public bool ShowOnPublishBlogCommentPage { get; set; }

        }

        public class LocalizationSettingsModel : BaseModel
        {

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.UseImagesForLanguageSelection")]
            public bool UseImagesForLanguageSelection { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SeoFriendlyUrlsForLanguagesEnabled")]
            public bool SeoFriendlyUrlsForLanguagesEnabled { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.AutomaticallyDetectLanguage")]
            public bool AutomaticallyDetectLanguage { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.LoadAllLocaleRecordsOnStartup")]
            public bool LoadAllLocaleRecordsOnStartup { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.LoadAllLocalizedPropertiesOnStartup")]
            public bool LoadAllLocalizedPropertiesOnStartup { get; set; }

            [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.LoadAllUrlRecordsOnStartup")]
            public bool LoadAllUrlRecordsOnStartup { get; set; }

        }

        #endregion

    }
}