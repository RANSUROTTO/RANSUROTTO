using RANSUROTTO.BLOG.Core.Domain.Logging;

namespace RANSUROTTO.BLOG.Data.Mapping.Logging
{
    public class ActivityLogMap : CustomEntityTypeConfiguration<ActivityLog>
    {
        public ActivityLogMap()
        {
            this.ToTable("ActivityLog");
            this.Property(p => p.Comment).IsRequired();
            this.Property(p => p.IpAddress).HasMaxLength(200);

            this.HasRequired(p => p.ActivityLogType)
                .WithMany()
                .HasForeignKey(p => p.ActivityLogTypeId);
            this.HasRequired(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerId);
        }
    }
}
