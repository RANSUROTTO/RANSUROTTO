using RANSUROTTO.BLOG.Core.Domain.Blogs;

namespace RANSUROTTO.BLOG.Data.Mapping.Blogs
{
    public class BlogPostBlogCategoryMap : CustomEntityTypeConfiguration<BlogPostBlogCategory>
    {
        public BlogPostBlogCategoryMap()
        {
            this.ToTable("BlogPost_BlogCategory_Mapping");

            this.HasRequired(bc => bc.BlogCategory)
                .WithMany()
                .HasForeignKey(bc => bc.BlogCategoryId);
            this.HasRequired(bc => bc.BlogPost)
                .WithMany(bc => bc.BlogCategories)
                .HasForeignKey(bc => bc.BlogCategoryId);
        }
    }
}
