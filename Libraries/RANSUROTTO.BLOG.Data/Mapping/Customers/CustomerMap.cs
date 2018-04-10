using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Data.Mapping.Customers
{
    public class CustomerMap : CustomEntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            this.ToTable("Customer");
            this.Property(u => u.Username).HasMaxLength(1000);
            this.Property(u => u.Email).HasMaxLength(1000);
            this.Property(u => u.SystemName).HasMaxLength(400);


        }
    }
}
