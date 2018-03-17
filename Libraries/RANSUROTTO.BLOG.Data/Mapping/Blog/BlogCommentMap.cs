using RANSUROTTO.BLOG.Core.Domain.Blog;

namespace RANSUROTTO.BLOG.Data.Mapping.Blog
{
    public class BlogCommentMap : CustomEntityTypeConfiguration<BlogComment>
    {
        public BlogCommentMap()
        {
            this.ToTable("BlogComment");
            this.HasRequired(p => p.BlogPost).WithMany().HasForeignKey(p => p.BlogPostId);
            this.HasOptional(p => p.Customer).WithMany().HasForeignKey(p => p.CustomerId);
        }
    }
}
