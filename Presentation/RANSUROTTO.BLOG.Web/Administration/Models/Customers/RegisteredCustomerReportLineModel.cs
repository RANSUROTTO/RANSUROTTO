using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Framework.Localization;

namespace RANSUROTTO.BLOG.Admin.Models.Customers
{
    public class RegisteredCustomerReportLineModel : BaseModel
    {

        [ResourceDisplayName("Admin.Customers.Reports.RegisteredCustomers.Fields.Period")]
        public string Period { get; set; }


        [ResourceDisplayName("Admin.Customers.Reports.RegisteredCustomers.Fields.Customers")]
        public int Customers { get; set; }

    }
}