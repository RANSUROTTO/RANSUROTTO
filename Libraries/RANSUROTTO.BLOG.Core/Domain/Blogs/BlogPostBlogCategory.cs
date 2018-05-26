using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Blogs
{
    public class BlogPostBlogCategory : BaseEntity
    {

        public int BlogPostId { get; set; }

        public int BlogCategoryId { get; set; }

        public int DisplayOrder { get; set; }

        public virtual BlogPost BlogPost { get; set; }

        public virtual BlogCategory BlogCategory { get; set; }

    }
}
