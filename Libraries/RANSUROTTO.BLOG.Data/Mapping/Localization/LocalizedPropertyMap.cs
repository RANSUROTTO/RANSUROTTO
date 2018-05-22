using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Data.Mapping.Localization
{
    public class LocalizedPropertyMap : CustomEntityTypeConfiguration<LocalizedProperty>
    {
        public LocalizedPropertyMap()
        {
            this.ToTable("LocalizedProperty");

            this.Property(lp => lp.LocaleKeyGroup).IsRequired().HasMaxLength(400);
            this.Property(lp => lp.LocaleKey).IsRequired().HasMaxLength(400);
            this.Property(lp => lp.LocaleValue).IsRequired();

            this.HasRequired(lp => lp.Language)
                .WithMany()
                .HasForeignKey(lp => lp.LanguageId);
        }
    }
}
