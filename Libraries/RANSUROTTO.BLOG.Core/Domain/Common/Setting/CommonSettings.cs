using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Common.Setting
{
    public class CommonSettings : ISettings
    {

        /// <summary>
        /// 获取或设置默认主题
        /// </summary>
        public string DefaultTheme { get; set; }

        /// <summary>
        /// 获取或设置是否记录404错误日志
        /// </summary>
        public bool Log404Errors { get; set; }



    }
}
