using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Domain.Seo.Enum;

namespace RANSUROTTO.BLOG.Core.Domain.Seo.Setting
{
    /// <summary>
    /// 搜索引擎优化(SEO)设定
    /// </summary>
    public class SeoSettings : ISettings
    {

        /// <summary>
        /// 获取或设置页面标题分割符
        /// </summary>
        public string PageTitleSeparator { get; set; }

        /// <summary>
        /// 获取或设置页面标题调整模式
        /// </summary>
        public PageTitleSeoAdjustment PageTitleSeoAdjustment { get; set; }

        /// <summary>
        /// 获取或设置默认页面标题
        /// </summary>
        public string DefaultTitle { get; set; }

        /// <summary>
        /// 获取或设置默认Meta关键词
        /// </summary>
        public string DefaultMetaKeywords { get; set; }

        /// <summary>
        /// 获取或设置默认Meta描述
        /// </summary>
        public string DefaultMetaDescription { get; set; }

        /// <summary>
        /// 获取或设置自定义标签节点
        /// </summary>
        public string CustomHeadTags { get; set; }

        /// <summary>
        /// 获取或设置是否启用JS文件绑捆和压缩
        /// </summary>
        public bool EnableJsBundling { get; set; }

        /// <summary>
        /// 获取或设置是否启用CSS文件绑捆和压缩
        /// </summary>
        public bool EnableCssBundling { get; set; }

        /// <summary>
        /// 获取或设置网站Url是否需要www前缀标识
        /// </summary>
        public WwwRequirement WwwRequirement { get; set; }

    }
}
