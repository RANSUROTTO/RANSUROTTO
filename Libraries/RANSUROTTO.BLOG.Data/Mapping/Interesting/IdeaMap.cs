using RANSUROTTO.BLOG.Core.Domain.Interesting;

namespace RANSUROTTO.BLOG.Data.Mapping.Interesting
{
    public class IdeaMap : CustomEntityTypeConfiguration<Idea>
    {
        public IdeaMap()
        {
            this.ToTable("Idea");

            this.Property(p => p.Body).HasMaxLength(500);

            this.HasRequired(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerId);
            this.HasMany(p => p.Pictures)
                .WithMany()
                .Map(p => p.ToTable("Idea_Picture_Mapping"));
        }
    }
}
