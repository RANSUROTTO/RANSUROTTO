using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Data.Mapping.Customers
{
    public class CustomerRoleMap : CustomEntityTypeConfiguration<CustomerRole>
    {
        public CustomerRoleMap()
        {
            this.ToTable("CustomerRole");
            this.Property(cr => cr.Name).IsRequired().HasMaxLength(255);
            this.Property(cr => cr.SystemName).HasMaxLength(255);
            this.HasMany(p => p.PermissionRecords)
                .WithMany(p => p.CustomerRoles)
                .Map(p => p.ToTable("PermissionRecord_Role_Mapping"));
        }
    }
}
