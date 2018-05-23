using RANSUROTTO.BLOG.Core.Domain.Blogs;

namespace RANSUROTTO.BLOG.Data.Mapping.Blogs
{
    public class BlogPostTagMap : CustomEntityTypeConfiguration<BlogPostTag>
    {
        public BlogPostTagMap()
        {
            this.ToTable("ProductTag");

            this.Property(pt => pt.Name).IsRequired().HasMaxLength(400);
        }
    }
}
