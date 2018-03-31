using RANSUROTTO.BLOG.Core.Domain.Blog;

namespace RANSUROTTO.BLOG.Data.Mapping.Blog
{
    public class CategoryMap : CustomEntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            this.ToTable("Category");

            this.Property(p => p.Name).HasMaxLength(255);
            this.Property(p => p.AdminComment).HasMaxLength(1000);
        }
    }
}
