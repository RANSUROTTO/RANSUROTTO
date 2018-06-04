using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Common.Setting
{
    public class CommonSettings : ISettings
    {

        public bool SitemapEnabled { get; set; }

        /// <summary>
        /// 获取或设置是否启用存储过程（如果可以使用的话，应使用）
        /// </summary>
        public bool UseStoredProceduresIfSupported { get; set; }

        /// <summary>
        /// 获取或设置是否使用存储过程来获取类目（这比LINQ要快）
        /// </summary>
        public bool UseStoredProcedureForLoadingCategories { get; set; }
        
        /// <summary>
        /// 获取或设置Javascript被禁用时是否显示警告
        /// </summary>
        public bool DisplayJavaScriptDisabledWarning { get; set; }

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
