using System.Web.Mvc;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Blogs
{
    public class BlogCategoryListModel : BaseModel
    {

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogCategorys.List.SearchCategoryName")]
        [AllowHtml]
        public string SearchCategoryName { get; set; }

    }
}