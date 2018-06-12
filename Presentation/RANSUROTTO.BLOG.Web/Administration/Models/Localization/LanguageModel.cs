using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Localization
{
    public class LanguageModel : BaseEntityModel
    {

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        [ResourceDisplayName("Admin.Configuration.Languages.Fields.Name")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置语言文化
        /// </summary>
        [ResourceDisplayName("Admin.Configuration.Languages.Fields.LanguageCulture")]
        public string LanguageCulture { get; set; }

        /// <summary>
        /// 获取或设置唯一的SEO代码
        /// </summary>
        [ResourceDisplayName("Admin.Configuration.Languages.Fields.UniqueSeoCode")]
        public string UniqueSeoCode { get; set; }

        /// <summary>
        /// 获取或设置显示顺序
        /// </summary>
        [ResourceDisplayName("Admin.Configuration.Languages.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 获取或设置国旗图片文件名
        /// </summary>
        [ResourceDisplayName("Admin.Configuration.Languages.Fields.FlagImageFileName")]
        public string FlagImageFileName { get; set; }

        /// <summary>
        /// 获取或设置该语言是否支持“从右到左”显示方式
        /// </summary>
        [ResourceDisplayName("Admin.Configuration.Languages.Fields.Rtl")]
        public bool Rtl { get; set; }

        /// <summary>
        /// 获取或设置该语言是否已发布
        /// </summary>
        [ResourceDisplayName("Admin.Configuration.Languages.Fields.Published")]
        public bool Published { get; set; }

        // search
        public LanguageResourcesListModel Search { get; set; }
    }
}