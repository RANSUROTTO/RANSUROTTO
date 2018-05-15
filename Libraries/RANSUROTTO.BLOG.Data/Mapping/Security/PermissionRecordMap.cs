using RANSUROTTO.BLOG.Core.Domain.Security;

namespace RANSUROTTO.BLOG.Data.Mapping.Security
{
    public class PermissionRecordMap : CustomEntityTypeConfiguration<PermissionRecord>
    {
        public PermissionRecordMap()
        {
            this.ToTable("PermissionRecord");

            this.Property(p => p.Name).IsRequired();
            this.Property(p => p.SystemName).IsRequired().HasMaxLength(255);
            this.Property(p => p.Category).IsRequired().HasMaxLength(255);

            this.HasMany(p => p.CustomerRoles)
                .WithMany(p => p.PermissionRecords)
                .Map(m => m.ToTable("PermissionRecord_Role_Mapping"));
        }
    }
}
