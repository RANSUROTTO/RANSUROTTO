using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Core.Domain.Blogs
{
    /// <summary>
    /// 博客标签
    /// </summary>
    public class BlogPostTag : BaseEntity, ILocalizedEntity
    {

        private ICollection<BlogPost> _blogPosts;

        /// <summary>
        /// 获取或设置标签名字
        /// </summary>
        public string Name { get; set; }

        public virtual ICollection<BlogPost> BlogPosts
        {
            get { return _blogPosts ?? (_blogPosts = new List<BlogPost>()); }
            set { _blogPosts = value; }
        }

    }
}
