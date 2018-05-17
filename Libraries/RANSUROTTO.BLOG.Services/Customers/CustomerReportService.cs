using System;
using System.Linq;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;
using RANSUROTTO.BLOG.Services.Helpers;

namespace RANSUROTTO.BLOG.Services.Customers
{
    public class CustomerReportService : ICustomerReportService
    {

        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ICustomerService _customerService;
        private readonly IRepository<Customer> _customerRepository;

        public CustomerReportService(IDateTimeHelper dateTimeHelper, ICustomerService customerService, IRepository<Customer> customerRepository)
        {
            _dateTimeHelper = dateTimeHelper;
            _customerService = customerService;
            _customerRepository = customerRepository;
        }

        public virtual int GetRegisteredCustomersReport(int days)
        {
            DateTime date = _dateTimeHelper.ConvertToUserTime(DateTime.Now).AddDays(-days);

            var registeredCustomerRole = _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered);
            if (registeredCustomerRole == null)
                return 0;

            var query = from c in _customerRepository.Table
                        from cr in c.CustomerRoles
                        where !c.Deleted &&
                              cr.Id == registeredCustomerRole.Id &&
                              c.CreatedOnUtc >= date
                        select c;

            int count = query.Count();
            return count;
        }

    }
}
