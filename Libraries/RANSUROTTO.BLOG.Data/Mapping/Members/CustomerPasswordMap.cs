using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Data.Mapping.Members
{
    public class CustomerPasswordMap : CustomEntityTypeConfiguration<CustomerPassword>
    {
        public CustomerPasswordMap()
        {
            this.ToTable("CustomerPassword");
            this.Ignore(p => p.PasswordFormat);

            this.HasRequired(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerId);
        }
    }
}
