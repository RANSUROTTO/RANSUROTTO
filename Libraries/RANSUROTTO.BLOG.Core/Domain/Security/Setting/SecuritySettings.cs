using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Security.Setting
{

    /// <summary>
    /// 安全设置
    /// </summary>
    public class SecuritySettings : ISettings
    {

        /// <summary>
        /// 获取或设置加密时使用的默认密钥
        /// </summary>
        public string EncryptionKey { get; set; }

    }
}
