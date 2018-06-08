using RANSUROTTO.BLOG.Core.Domain.Blogs;

namespace RANSUROTTO.BLOG.Data.Mapping.Blogs
{
    public class BlogPostCategoryMap : CustomEntityTypeConfiguration<BlogPostCategory>
    {
        public BlogPostCategoryMap()
        {
            this.ToTable("BlogPostCategory");

            this.HasRequired(bc => bc.Category)
                .WithMany()
                .HasForeignKey(bc => bc.BlogCategoryId);
            this.HasRequired(bc => bc.BlogPost)
                .WithMany(bc => bc.BlogCategories)
                .HasForeignKey(bc => bc.BlogPostId);
        }
    }
}
