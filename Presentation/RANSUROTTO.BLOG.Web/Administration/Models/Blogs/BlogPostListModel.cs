using System.Collections.Generic;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Blogs
{
    public class BlogPostListModel : BaseModel
    {
        public BlogPostListModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailablePublishedOptions = new List<SelectListItem>();
        }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.List.SearchTitle")]
        public string SearchTitle { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.List.SearchCategory")]
        public int SearchCategoryId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.List.SearchIncludeSubCategories")]
        public bool SearchIncludeSubCategories { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.List.SearchPublished")]
        public int SearchPublishedId { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.Posts.List.ShowDeleted")]
        public bool ShowDeleted { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> AvailablePublishedOptions { get; set; }

    }
}