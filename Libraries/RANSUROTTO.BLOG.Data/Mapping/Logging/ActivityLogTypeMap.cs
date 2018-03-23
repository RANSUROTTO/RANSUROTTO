using RANSUROTTO.BLOG.Core.Domain.Logging;

namespace RANSUROTTO.BLOG.Data.Mapping.Logging
{
    public class ActivityLogTypeMap : CustomEntityTypeConfiguration<ActivityLogType>
    {
        public ActivityLogTypeMap()
        {
            this.ToTable("ActivityLogType");
            this.Property(p => p.SystemKeyword).IsRequired().HasMaxLength(100);
            this.Property(p => p.Name).IsRequired().HasMaxLength(200);
        }
    }
}
