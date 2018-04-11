using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Security.Setting
{

    /// <summary>
    /// 安全设置
    /// </summary>
    public class SecuritySettings : ISettings
    {

        /// <summary>
        /// 获取或设置标识是否启用SSL
        /// </summary>
        public bool SslEnabled { get; set; }

        /// <summary>
        /// 获取或设置标识是否强制所有页面使用SSL
        /// </summary>
        public bool ForceSslForAllPages { get; set; }

        /// <summary>
        /// 获取或设置加密时使用的默认密钥
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// 获取或设置允许进入管理员区域的IP地址列表
        /// </summary>
        public List<string> AdminAreaAllowedIpAddresses { get; set; }

        /// <summary>
        /// 获取或设置标识是否启动管理员区域XSRF保护
        /// </summary>
        public bool EnableXsrfProtectionForAdminArea { get; set; }

        /// <summary>
        /// 获取或设置标识是否启动公共区域XSRF保护
        /// </summary>
        public bool EnableXsrfProtectionForPublicArea { get; set; }

    }
}
