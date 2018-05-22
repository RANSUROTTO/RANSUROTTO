using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation.Attributes;
using RANSUROTTO.BLOG.Admin.Validators.Blogs;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Blogs
{
    [Validator(typeof(BlogCategoryValidator))]
    public class BlogCategoryModel : BaseEntityModel, ILocalizedModel<BlogCategoryLocalizedModel>
    {
        public BlogCategoryModel()
        {
            AvailableCategories = new List<SelectListItem>();
            Locales = new List<BlogCategoryLocalizedModel>();
        }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }
        public string Breadcrumb { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.Description")]
        [AllowHtml]
        public string Description { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.Published")]
        public bool Published { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.AdminComment")]
        [AllowHtml]
        public string AdminComment { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.Deleted")]
        public bool Deleted { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.MetaKeywords")]
        [AllowHtml]
        public string MetaKeywords { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.MetaDescription")]
        [AllowHtml]
        public string MetaDescription { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.MetaTitle")]
        [AllowHtml]
        public string MetaTitle { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.SeName")]
        [AllowHtml]
        public string SeName { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.Parent")]
        public long ParentCategoryId { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        public IList<BlogCategoryLocalizedModel> Locales { get; set; }
    }

    public class BlogCategoryLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.Description")]
        [AllowHtml]
        public string Description { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.MetaKeywords")]
        [AllowHtml]
        public string MetaKeywords { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.MetaDescription")]
        [AllowHtml]
        public string MetaDescription { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.MetaTitle")]
        [AllowHtml]
        public string MetaTitle { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.Fields.SeName")]
        [AllowHtml]
        public string SeName { get; set; }

    }

}