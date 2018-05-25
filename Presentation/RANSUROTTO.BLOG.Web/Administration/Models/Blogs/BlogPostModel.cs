using System;
using System.Collections.Generic;
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

        public int AuthorId { get; set; }

        public string Title { get; set; }

        public string BodyOverview { get; set; }

        public string Body { get; set; }

        public int FormatId { get; set; }

        public bool AllowComments { get; set; }

        public DateTime? AvailableStartDateUtc { get; set; }

        public DateTime? AvailableEndDateUtc { get; set; }

        public DateTime UpdateOnUtc { get; set; }

        public bool Deleted { get; set; }

        public bool Published { get; set; }

        public string AuthorEmail { get; set; }

        public IList<BlogPostModelLocal> Locales { get; set; }

        public IList<int> SelectedCategoryIds { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }

    }

    public class BlogPostModelLocal : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        public string Title { get; set; }

        public string BodyOverview { get; set; }

        public string Body { get; set; }
    }
}