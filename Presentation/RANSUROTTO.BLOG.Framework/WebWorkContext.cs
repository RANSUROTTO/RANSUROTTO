using System;
using System.Web;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Core.Domain.Localization.Setting;
using RANSUROTTO.BLOG.Core.Fakes;
using RANSUROTTO.BLOG.Service.Authentication;
using RANSUROTTO.BLOG.Service.Common;
using RANSUROTTO.BLOG.Service.Customers;
using RANSUROTTO.BLOG.Service.Helpers;
using RANSUROTTO.BLOG.Service.Localization;

namespace RANSUROTTO.BLOG.Framework
{
    /// <summary>
    /// Web应用程序工作上下文
    /// </summary>
    public class WebWorkContext : IWorkContext
    {

        #region Constants

        private const string CustomerCookieName = "Ransurotto.customer";

        #endregion

        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILanguageService _languageService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IUserAgentHelper _userAgentHelper;
        private readonly LocalizationSettings _localizationSettings;

        private Customer _cachedCustomer;
        private Language _cachedLanguage;

        #endregion

        #region Constructor

        public WebWorkContext(HttpContextBase httpContext, ICustomerService customerService, IAuthenticationService authenticationService, ILanguageService languageService, IGenericAttributeService genericAttributeService, IUserAgentHelper userAgentHelper, LocalizationSettings localizationSettings)
        {
            _httpContext = httpContext;
            _customerService = customerService;
            _authenticationService = authenticationService;
            _languageService = languageService;
            _genericAttributeService = genericAttributeService;
            _userAgentHelper = userAgentHelper;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Properties

        public Language WorkingLanguage
        {
            get
            {
                if (_cachedLanguage != null)
                    return _cachedLanguage;

                Language detectedLanguage = null;

                return detectedLanguage;
            }
            set
            {

            }
        }

        /// <summary>
        /// 获取当前工作区用户
        /// </summary>
        public Customer CurrentCustomer
        {
            get
            {
                if (_cachedCustomer != null)
                    return _cachedCustomer;

                Customer customer = null;
                if (_httpContext == null || _httpContext is FakeHttpContext)
                {
                    //检查请求是否由后台任务进行
                    //这种情况下返回后台任务系统名的内置客户
                    customer = _customerService.GetCustomerBySystemName(SystemCustomerNames.BackgroundTask);
                }

                //检查请求是否由搜索引擎进行
                //这种情况下返回搜索引擎系统名的内置账户
                if (customer == null || !customer.Active)
                {
                    if (_userAgentHelper.IsSearchEngine())
                    {
                        customer = _customerService.GetCustomerBySystemName(SystemCustomerNames.SearchEngine);
                    }
                }

                if (customer == null || !customer.Active)
                {
                    customer = _authenticationService.GetAuthenticatedCustomer();
                }

                if (customer != null && customer.Active)
                {
                    SetCustomerCookie(customer.Guid);
                    _cachedCustomer = customer;
                }

                return _cachedCustomer;
            }
            set
            {
                SetCustomerCookie(value.Guid);
                _cachedCustomer = value;
            }
        }

        public bool IsAdmin { get; set; }

        #endregion

        #region Utilities

        protected virtual void SetCustomerCookie(Guid customerGuid)
        {
            if (_httpContext?.Response != null)
            {
                var cookie = new HttpCookie(CustomerCookieName);
                cookie.HttpOnly = true;
                cookie.Value = customerGuid.ToString();
                if (customerGuid == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 30;
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(CustomerCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        #endregion

    }
}
