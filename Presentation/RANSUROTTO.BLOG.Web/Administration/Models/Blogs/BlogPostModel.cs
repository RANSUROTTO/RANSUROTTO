using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using RANSUROTTO.BLOG.Admin.Validators.Blogs;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Blogs
{
    [Validator(typeof(BlogPostValidator))]
    public class BlogPostModel : BaseEntityModel, ILocalizedModel<BlogPostModelLocalizedModel>
    {
        public BlogPostModel()
        {
            SelectedCategoryIds = new List<int>();
            AvailableCategories = new List<SelectListItem>();
            Locales = new List<BlogPostModelLocalizedModel>();
        }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.AuthorId")]
        public int AuthorId { get; set; }

        [AllowHtml]
        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Title")]
        public string Title { get; set; }

        [AllowHtml]
        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.BodyOverview")]
        public string BodyOverview { get; set; }

        [AllowHtml]
        [UIHint("RichEditor")]
        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Body")]
        public string Body { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.FormatId")]
        public int FormatId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.AllowComments")]
        public bool AllowComments { get; set; }

        [UIHint("DateTimeNullable")]
        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.AvailableStartDateUtc")]
        public DateTime? AvailableStartDateUtc { get; set; }

        [UIHint("DateTimeNullable")]
        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.AvailableEndDateUtc")]
        public DateTime? AvailableEndDateUtc { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.CreatedOn")]
        public override DateTime CreatedOn { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.MetaKeywords")]
        [AllowHtml]
        public string MetaKeywords { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.MetaDescription")]
        [AllowHtml]
        public string MetaDescription { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.MetaTitle")]
        [AllowHtml]
        public string MetaTitle { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Deleted")]
        public bool Deleted { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Published")]
        public bool Published { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.AuthorEmail")]
        public string AuthorEmail { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.BlogPostTags")]
        public string BlogPostTags { get; set; }

        public IList<BlogPostModelLocalizedModel> Locales { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Categories")]
        [UIHint("MultiSelect")]
        public IList<int> SelectedCategoryIds { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }

    }

    public class BlogPostModelLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [AllowHtml]
        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Title")]
        public string Title { get; set; }

        [AllowHtml]
        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.BodyOverview")]
        public string BodyOverview { get; set; }

        [AllowHtml]
        [UIHint("RichEditor")]
        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Body")]
        public string Body { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.MetaKeywords")]
        [AllowHtml]
        public string MetaKeywords { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.MetaDescription")]
        [AllowHtml]
        public string MetaDescription { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.MetaTitle")]
        [AllowHtml]
        public string MetaTitle { get; set; }

    }
}