using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation.Attributes;
using RANSUROTTO.BLOG.Admin.Validators.Blogs;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Blogs
{
    [Validator(typeof(CategoryValidator))]
    public class CategoryModel : BaseEntityModel, ILocalizedModel<CategoryLocalizedModel>
    {
        public CategoryModel()
        {
            AvailableCategories = new List<SelectListItem>();
            Locales = new List<CategoryLocalizedModel>();
        }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }
        public string Breadcrumb { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.Description")]
        [AllowHtml]
        public string Description { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.Published")]
        public bool Published { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.AdminComment")]
        [AllowHtml]
        public string AdminComment { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.Deleted")]
        public bool Deleted { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.MetaKeywords")]
        [AllowHtml]
        public string MetaKeywords { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.MetaDescription")]
        [AllowHtml]
        public string MetaDescription { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.MetaTitle")]
        [AllowHtml]
        public string MetaTitle { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.SeName")]
        [AllowHtml]
        public string SeName { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.Parent")]
        public long ParentCategoryId { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        public IList<CategoryLocalizedModel> Locales { get; set; }
    }

    public class CategoryLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.Description")]
        [AllowHtml]
        public string Description { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.MetaKeywords")]
        [AllowHtml]
        public string MetaKeywords { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.MetaDescription")]
        [AllowHtml]
        public string MetaDescription { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.MetaTitle")]
        [AllowHtml]
        public string MetaTitle { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.Fields.SeName")]
        [AllowHtml]
        public string SeName { get; set; }

    }

}