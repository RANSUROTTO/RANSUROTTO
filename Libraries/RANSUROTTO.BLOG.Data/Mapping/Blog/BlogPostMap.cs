using RANSUROTTO.BLOG.Core.Domain.Blog;

namespace RANSUROTTO.BLOG.Data.Mapping.Blog
{

    public class BlogPostMap : CustomEntityTypeConfiguration<BlogPost>
    {
        public BlogPostMap()
        {
            this.ToTable("BlogPost");
            this.Property(p => p.Title).IsRequired().HasMaxLength(1000);
            this.Property(p => p.Body).IsRequired();
            this.Property(p => p.BodyOverview).HasMaxLength(2000);

            this.HasRequired(p => p.Language)
                .WithMany()
                .HasForeignKey(p => p.LanguageId);
        }
    }

}
