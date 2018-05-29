using System;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Core.Domain.Blogs
{
    /// <summary>
    /// 博客类目
    /// </summary>
    public class Category : BaseEntity, ILocalizedEntity
    {

        /// <summary>
        /// 获取或设置类目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置发布状态
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// 获取或设置顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 获取或设置类目描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置管理员赋予的备注
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// 获取或设置最后修改时的UTC时间
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// 获取或设置该类目是否已被删除
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 获取或设置 Meta Keywords
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// 获取或设置 Meta Description
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// 获取或设置 Meta Title
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// 获取或设置 SEO 友好名称
        /// </summary>
        public string SeName { get; set; }

        /// <summary>
        /// 获取或设置父级类目标识符
        /// </summary>
        public int ParentCategoryId { get; set; }

    }
}
