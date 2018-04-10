using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Data.Mapping.Customers
{
    public class CustomerPasswordMap : CustomEntityTypeConfiguration<CustomerPassword>
    {
        public CustomerPasswordMap()
        {
            this.ToTable("CustomerPassword");


            this.HasRequired(password => password.Customer)
                .WithMany()
                .HasForeignKey(password => password.CustomerId);

            this.Ignore(password => password.PasswordFormat);
        }
    }
}
