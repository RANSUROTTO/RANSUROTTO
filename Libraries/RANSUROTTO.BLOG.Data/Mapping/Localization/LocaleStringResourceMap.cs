using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Data.Mapping.Localization
{

    public class LocaleStringResourceMap : CustomEntityTypeConfiguration<LocaleStringResource>
    {
        public LocaleStringResourceMap()
        {
            this.ToTable("LocaleStringResource");
            this.Property(lsr => lsr.ResourceName).IsRequired().HasMaxLength(200);
            this.Property(lsr => lsr.ResourceValue).IsRequired();

            this.HasRequired(lsr => lsr.Language)
                .WithMany(l => l.LocaleStringResources)
                .HasForeignKey(lsr => lsr.LanguageId);
        }
    }

}
