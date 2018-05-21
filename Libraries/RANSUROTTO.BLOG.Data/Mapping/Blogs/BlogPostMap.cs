using RANSUROTTO.BLOG.Core.Domain.Blogs;

namespace RANSUROTTO.BLOG.Data.Mapping.Blogs
{

    public class BlogPostMap : CustomEntityTypeConfiguration<BlogPost>
    {
        public BlogPostMap()
        {
            this.ToTable("BlogPost");
            this.Ignore(p => p.Format);

            this.Property(p => p.Title).IsRequired().HasMaxLength(1000);
            this.Property(p => p.Body).IsRequired();
            this.Property(p => p.BodyOverview).HasMaxLength(2000);

            this.HasRequired(p => p.BlogCategory)
                .WithMany()
                .HasForeignKey(p => p.BlogCategoryId);
            this.HasRequired(p => p.Author)
                .WithMany()
                .HasForeignKey(p => p.AuthorId);
            this.HasRequired(p => p.Language)
                .WithMany()
                .HasForeignKey(p => p.LanguageId);
            this.HasMany(p => p.BlogComments)
                .WithRequired(p => p.BlogPost)
                .HasForeignKey(p => p.BlogPostId);

            this.Ignore(p => p.Format);
        }
    }

}
