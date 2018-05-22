using RANSUROTTO.BLOG.Core.Domain.Media;

namespace RANSUROTTO.BLOG.Data.Mapping.Media
{
    public class PictureMap : CustomEntityTypeConfiguration<Picture>
    {
        public PictureMap()
        {
            this.ToTable("Picture");
            this.Property(p => p.MimeType).IsRequired().HasMaxLength(40);
            this.Property(p => p.SeoFilename).HasMaxLength(300);
        }
    }
}
