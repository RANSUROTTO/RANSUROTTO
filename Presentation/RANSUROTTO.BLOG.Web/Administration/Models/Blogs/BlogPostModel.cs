using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Blogs
{
    public class BlogPostModel : BaseEntityModel, ILocalizedModel<BlogPostModelLocal>
    {
        public BlogPostModel()
        {
            SelectedCategoryIds = new List<int>();
            AvailableCategories = new List<SelectListItem>();
        }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.AuthorId")]
        public int AuthorId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.BodyOverview")]
        public string BodyOverview { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Body")]
        public string Body { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.FormatId")]
        public int FormatId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.AllowComments")]
        public bool AllowComments { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.AvailableStartDateUtc")]
        public DateTime? AvailableStartDateUtc { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.AvailableEndDateUtc")]
        public DateTime? AvailableEndDateUtc { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.CreatedOn")]
        public override DateTime CreatedOn { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Deleted")]
        public bool Deleted { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Published")]
        public bool Published { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.AuthorEmail")]
        public string AuthorEmail { get; set; }

        public IList<BlogPostModelLocal> Locales { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Categories")]
        [UIHint("MultiSelect")]
        public IList<int> SelectedCategoryIds { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }

    }

    public class BlogPostModelLocal : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.BodyOverview")]
        public string BodyOverview { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.Fields.Body")]
        public string Body { get; set; }
    }
}