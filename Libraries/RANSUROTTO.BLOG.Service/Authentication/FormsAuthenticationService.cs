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
            var now = DateTime.UtcNow.ToLocalTime();

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                customer.Username,
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                customer.Guid.ToString(),
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);
            _cachedCustomer = customer;
        }

        public void SignOut()
        {
            _cachedCustomer = null;
            FormsAuthentication.SignOut();
        }

        public Customer GetAuthenticatedCustomer()
        {
            if (_cachedCustomer != null)
                return _cachedCustomer;

            if (_httpContext == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            var customer = GetAuthenticatedCustomerFromTicket(formsIdentity.Ticket);
            if (customer != null && customer.Active)
                _cachedCustomer = customer;

            return _cachedCustomer;
        }

        #endregion

        #region Utilities

        protected virtual Customer GetAuthenticatedCustomerFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));

            var customerGuid = Guid.Parse(ticket.UserData);

            if (customerGuid == Guid.Empty)
                return null;

            var customer = _customerService.GetCustomerByGuid(customerGuid);

            return customer;
        }

        #endregion

    }
}
