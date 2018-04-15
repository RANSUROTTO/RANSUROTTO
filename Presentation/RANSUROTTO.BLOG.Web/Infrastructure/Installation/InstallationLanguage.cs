using System.Collections.Generic;

namespace RANSUROTTO.BLOG.Web.Infrastructure.Installation
{

    /// <summary>
    /// 区域语言
    /// </summary>
    public class InstallationLanguage
    {
        public InstallationLanguage()
        {
            Resources = new List<InstallationLocaleResource>();
        }

        /// <summary>
        /// 语言名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 区域/语言代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 指定是否为默认语言
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 从右到左显示
        /// </summary>
        public bool IsRightToLeft { get; set; }

        /// <summary>
        /// 语言字符串资源列表
        /// </summary>
        public List<InstallationLocaleResource> Resources { get; protected set; }
    }

    /// <summary>
    /// 语言字符串资源
    /// </summary>
    public partial class InstallationLocaleResource
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

}