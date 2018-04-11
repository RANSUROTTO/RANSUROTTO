using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Common.Setting
{
    public class CommonSettings : ISettings
    {

        /// <summary>
        /// 获取或设置是否显示有关新的欧盟Cookie法(Cookie隐私保护)的警告
        /// </summary>
        public bool DisplayEuCookieLawWarning { get; set; }

        /// <summary>
        /// 获取或设置默认主题
        /// </summary>
        public string DefaultTheme { get; set; }

        /// <summary>
        /// 获取或设置是否记录404错误日志
        /// </summary>
        public bool Log404Errors { get; set; }

        /// <summary>
        /// 获取或设置是否呈现X-UA-Compatible标签
        /// </summary>
        public bool RenderXuaCompatible { get; set; }

        /// <summary>
        /// 获取或设置X-UA-Compatible标签值
        /// </summary>
        public string XuaCompatibleValue { get; set; }

    }
}
