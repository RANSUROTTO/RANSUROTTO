using System.Web.Mvc;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Blogs
{
    public class CategoryListModel : BaseModel
    {

        [ResourceDisplayName("Admin.ContentManagement.Blog.Categories.List.SearchCategoryName")]
        [AllowHtml]
        public string SearchCategoryName { get; set; }

    }
}