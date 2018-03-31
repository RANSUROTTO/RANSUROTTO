using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Framework
{
    public class WebWorkContext : IWorkContext
    {
        public Language WorkingLanguage { get; set; }
        public Customer CurrentCustomer { get; set; }
        public bool IsAdmin { get; set; }
    }
}
