using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Data.Mapping.Localization
{

    public class LanguageMap : CustomEntityTypeConfiguration<Language>
    {
        public LanguageMap()
        {
            this.ToTable("Language");
            this.Property(p => p.Name).IsRequired().HasMaxLength(100);
            this.Property(p => p.LanguageCulture).IsRequired().HasMaxLength(20);
            this.Property(p => p.UniqueSeoCode).HasMaxLength(2);
        }
    }

}
