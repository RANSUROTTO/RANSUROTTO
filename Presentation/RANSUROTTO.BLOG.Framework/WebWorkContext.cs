using System;
using System.Linq;
using System.Web;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.AttributeName;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Core.Domain.Localization.Setting;
using RANSUROTTO.BLOG.Core.Fakes;
using RANSUROTTO.BLOG.Framework.Localization;
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

        /// <summary>
        /// 获取或设置当前工作区语言
        /// </summary>
        public Language WorkingLanguage
        {
            get
            {
                if (_cachedLanguage != null)
                    return _cachedLanguage;

                Language detectedLanguage = null;
                if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                {
                    //从URL中获取语言
                    detectedLanguage = GetLanguageFromUrl();
                }

                if (detectedLanguage == null && _localizationSettings.AutomaticallyDetectLanguage)
                {
                    if (!this.CurrentCustomer.GetAttribute<bool>(
                        SystemCustomerAttributeNames.LanguageAutomaticallyDetected,
                        _genericAttributeService))
                    {
                        //从浏览器设置中获取语言
                        detectedLanguage = GetLanguageFromBrowserSettings();
                        if (detectedLanguage != null)
                        {
                            _genericAttributeService.SaveAttribute(this.CurrentCustomer,
                                SystemCustomerAttributeNames.LanguageAutomaticallyDetected,
                                true);
                        }
                    }
                }
                if (detectedLanguage != null)
                {
                    //当语言被检测到,则更新用户使用的语言
                    if (this.CurrentCustomer.GetAttribute<int>(SystemCustomerAttributeNames.LanguageId,
                            _genericAttributeService) != detectedLanguage.Id)
                    {
                        _genericAttributeService.SaveAttribute(this.CurrentCustomer,
                            SystemCustomerAttributeNames.LanguageId,
                            detectedLanguage.Id);
                    }
                }

                var allLanguages = _languageService.GetAllLanguages();
                //获取当前用户使用的语言
                var languageId = this.CurrentCustomer.GetAttribute<int>(SystemCustomerAttributeNames.LanguageId, _genericAttributeService);
                var language = allLanguages.FirstOrDefault(x => x.Id == languageId);

                if (language == null)
                {
                    //如果没有找到合适的语言支持,则使用默认语言
                    language = allLanguages.FirstOrDefault();
                }

                //缓存
                _cachedLanguage = language;
                return _cachedLanguage;
            }
            set
            {
                var languageId = value != null ? value.Id : 0;
                _genericAttributeService.SaveAttribute(this.CurrentCustomer,
                    SystemCustomerAttributeNames.LanguageId,
                    languageId);

                //重置缓存
                _cachedLanguage = null;
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

                //注册用户
                if (customer == null || !customer.Active)
                {
                    customer = _authenticationService.GetAuthenticatedCustomer();
                }

                //获取游客身份用户
                if (customer == null || !customer.Active)
                {
                    var customerCookie = GetCustomerCookie();
                    if (!string.IsNullOrEmpty(customerCookie?.Value))
                    {
                        if (Guid.TryParse(customerCookie.Value, out var customerGuid))
                        {
                            var customerByCookie = _customerService.GetCustomerByGuid(customerGuid);
                            if (customerByCookie != null && !customerByCookie.IsRegistered())
                                customer = customerByCookie;
                        }
                    }
                }

                //生成游客身份角色
                if (customer == null || !customer.Active)
                {
                    customer = _customerService.InsertGuestCustomer();
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

        protected virtual HttpCookie GetCustomerCookie()
        {
            if (_httpContext?.Request == null)
                return null;

            return _httpContext.Request.Cookies[CustomerCookieName];
        }

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
                    int cookieExpires = 24 * 365;
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(CustomerCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        protected virtual Language GetLanguageFromUrl()
        {
            if (_httpContext?.Request == null)
                return null;

            string virtualPath = _httpContext.Request.AppRelativeCurrentExecutionFilePath;
            string applicationPath = _httpContext.Request.ApplicationPath;
            if (!virtualPath.IsLocalizedUrl(applicationPath, false))
                return null;

            var seoCode = virtualPath.GetLanguageSeoCodeFromUrl(applicationPath, false);
            if (String.IsNullOrEmpty(seoCode))
                return null;

            var language = _languageService
                .GetAllLanguages()
                .FirstOrDefault(l => seoCode.Equals(l.UniqueSeoCode, StringComparison.InvariantCultureIgnoreCase));

            if (language != null && language.Published)
            {
                return language;
            }

            return null;
        }

        protected virtual Language GetLanguageFromBrowserSettings()
        {
            if (_httpContext?.Request?.UserLanguages == null)
                return null;

            var userLanguage = _httpContext.Request.UserLanguages.FirstOrDefault();
            if (string.IsNullOrEmpty(userLanguage))
                return null;

            var language = _languageService
                .GetAllLanguages()
                .FirstOrDefault(l => userLanguage.StartsWith(l.LanguageCulture, StringComparison.InvariantCultureIgnoreCase));
            if (language != null && language.Published)
            {
                return language;
            }

            return null;
        }

        #endregion

    }
}
