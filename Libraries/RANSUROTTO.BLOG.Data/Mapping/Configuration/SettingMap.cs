using RANSUROTTO.BLOG.Core.Domain.Configuration;

namespace RANSUROTTO.BLOG.Data.Mapping.Configuration
{

    public class SettingMap : CustomEntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            this.ToTable("Setting");

            this.Property(p => p.Name).IsRequired().HasMaxLength(200);
            this.Property(p => p.Value).IsRequired().HasMaxLength(2000);
        }
    }

}
