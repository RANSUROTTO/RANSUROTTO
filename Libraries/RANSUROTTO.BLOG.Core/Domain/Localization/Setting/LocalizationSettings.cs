using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Localization.Setting
{
    public class LocalizationSettings : ISettings
    {

        /// <summary>
        /// 管理员区域默认使用的语言的标识符
        /// </summary>
        public long DefaultAdminLanguageId { get; set; }

        /// <summary>
        /// 标识是否开启对URL进行多语言化SEO优化
        /// </summary>
        public bool SeoFriendlyUrlsForLanguagesEnable { get; set; }

        /// <summary>
        /// 标识是否通过客户区域（浏览器设置）检测当前语言
        /// </summary>
        public bool AutomaticallyDetectLanguage { get; set; }

        /// <summary>
        /// 标识是否在应用程序启动时加载所有语言资源
        /// </summary>
        public bool LoadAllLocaleRecordsOnStartup { get; set; }

        /// <summary>
        /// 标识是否在应用程序启动时加载所有语言属性
        /// </summary>
        public bool LoadAllLocalizedPropertiesOnStartup { get; set; }

        /// <summary>
        /// 标识是否在用程序与启动时加载所有SEO友好名称
        /// </summary>
        public bool LoadAllUrlRecordsOnStartup { get; set; }

        /// <summary>
        /// 标识是否忽略管理员区域RTL(Right to Left)语言属性
        /// </summary>
        public bool IgnoreRtlPropertyForAdminArea { get; set; }

    }
}
