using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Localization
{
    public class LanguageModel : BaseEntityModel
    {

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        [ResourceDisplayName("Admin.Localization.Language.Properies.Name")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置语言文化
        /// </summary>
        [ResourceDisplayName("Admin.Localization.Language.Properies.LanguageCulture")]
        public string LanguageCulture { get; set; }

        /// <summary>
        /// 获取或设置唯一的SEO代码
        /// </summary>
        [ResourceDisplayName("Admin.Localization.Language.Properies.UniqueSeoCode")]
        public string UniqueSeoCode { get; set; }

        /// <summary>
        /// 获取或设置显示顺序
        /// </summary>
        [ResourceDisplayName("Admin.Localization.Language.Properies.DisplayOrder")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 获取或设置该语言是否支持“从右到左”显示方式
        /// </summary>
        [ResourceDisplayName("Admin.Localization.Language.Properies.Rtl")]
        public bool Rtl { get; set; }

        /// <summary>
        /// 获取或设置该语言是否已发布
        /// </summary>
        [ResourceDisplayName("Admin.Localization.Language.Properies.Published")]
        public bool Published { get; set; }

    }
}