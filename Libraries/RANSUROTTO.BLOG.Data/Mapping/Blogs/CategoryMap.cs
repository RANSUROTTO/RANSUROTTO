using RANSUROTTO.BLOG.Core.Domain.Blogs;

namespace RANSUROTTO.BLOG.Data.Mapping.Blogs
{
    public class CategoryMap : CustomEntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            this.ToTable("Category");

            this.Property(p => p.Name).IsRequired().HasMaxLength(255);
            this.Property(p => p.AdminComment).HasMaxLength(1000);
            this.Property(c => c.MetaKeywords).HasMaxLength(400);
            this.Property(c => c.MetaTitle).HasMaxLength(400);
        }
    }
}
