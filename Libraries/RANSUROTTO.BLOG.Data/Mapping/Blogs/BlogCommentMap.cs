using RANSUROTTO.BLOG.Core.Domain.Blogs;

namespace RANSUROTTO.BLOG.Data.Mapping.Blogs
{
    public class BlogCommentMap : CustomEntityTypeConfiguration<BlogComment>
    {
        public BlogCommentMap()
        {
            this.ToTable("BlogComment");

            this.HasRequired(p => p.BlogPost)
                .WithMany(p => p.BlogComments)
                .HasForeignKey(p => p.BlogPostId);
            this.HasOptional(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerId);
        }
    }
}
