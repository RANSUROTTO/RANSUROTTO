using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Data.Mapping.Members
{

    public class CustomerMap : CustomEntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            this.ToTable("Customer");
            this.Property(p => p.Username).HasMaxLength(1000);
            this.Property(p => p.Email).HasMaxLength(1000);
        }
    }

}
