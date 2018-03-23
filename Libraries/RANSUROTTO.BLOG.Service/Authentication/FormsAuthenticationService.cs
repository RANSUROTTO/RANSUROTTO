using System;
using System.Web;
using System.Web.Security;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Setting;
using RANSUROTTO.BLOG.Service.Customers;

namespace RANSUROTTO.BLOG.Service.Authentication
{
    public class FormsAuthenticationService : IAuthenticationService
    {

        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;
        private readonly CustomerSettings _customerSettings;
        private readonly TimeSpan _expirationTimeSpan;

        private Customer _cachedCustomer;

        #endregion

        #region Constructor

        public FormsAuthenticationService(HttpContextBase httpContext, ICustomerService customerService, CustomerSettings customerSettings)
        {
            _httpContext = httpContext;
            _customerService = customerService;
            _customerSettings = customerSettings;
            _expirationTimeSpan = FormsAuthentication.Timeout;
        }

        #endregion

        #region Methods

        public void SignIn(Customer customer, bool createPersistentCookie)
        {
            throw new NotImplementedException();
        }

        public void SignOut()
        {
            throw new NotImplementedException();
        }

        public Customer GetAuthenticatedCustomer()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Utilities



        #endregion

    }
}
