using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain
{
    public class BlogInformationSettings : ISettings
    {

        public bool BlogClosed { get; set; }

        /// <summary>
        /// 获取或设置是否显示有关新的欧盟Cookie法(Cookie隐私保护)的警告
        /// </summary>
        public bool DisplayEuCookieLawWarning { get; set; }

        public string GitHubLink { get; set; }

    }
}
