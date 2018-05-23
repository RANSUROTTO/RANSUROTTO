using System.Web.Mvc;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Blogs
{
    public class BlogPostTagModel : BaseEntityModel, ILocalizedModel<BlogPostTagModelLocal>
    {
        public BlogPostTagModel()
        {
            Locales = new List<BlogPostTagModelLocal>();
        }

        [AllowHtml]
        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogPostTags.Fields.Name")]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.ContentManagement.Blog.BlogPostTags.Fields.BlogCount")]
        public int BlogCount { get; set; }

        public IList<BlogPostTagModelLocal> Locales { get; set; }

    }

    public class BlogPostTagModelLocal : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        public string Name { get; set; }

    }

}