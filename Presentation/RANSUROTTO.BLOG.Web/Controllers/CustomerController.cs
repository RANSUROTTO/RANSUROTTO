using System;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain.Common.Setting;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;
using RANSUROTTO.BLOG.Core.Domain.Customers.Setting;
using RANSUROTTO.BLOG.Framework.Security;
using RANSUROTTO.BLOG.Services.Authentication;
using RANSUROTTO.BLOG.Services.Customers;
using RANSUROTTO.BLOG.Services.Events;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;
using RANSUROTTO.BLOG.Web.Models.Customer;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    public class CustomerController : BasePublicController
    {

        #region Fields

        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly CommonSettings _commonSettings;
        private readonly CustomerSettings _customerSettings;

        #endregion

        #region Constructor

        public CustomerController(IWorkContext workContext, ICustomerService customerService, ICustomerRegistrationService customerRegistrationService, ICustomerActivityService customerActivityService, IAuthenticationService authenticationService, IEventPublisher eventPublisher, ILocalizationService localizationService, CommonSettings commonSettings, CustomerSettings customerSettings)
        {
            _workContext = workContext;
            _customerService = customerService;
            _customerRegistrationService = customerRegistrationService;
            _customerActivityService = customerActivityService;
            _authenticationService = authenticationService;
            _eventPublisher = eventPublisher;
            _localizationService = localizationService;
            _commonSettings = commonSettings;
            _customerSettings = customerSettings;
        }

        #endregion

        #region Login / Logout

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual ActionResult Login()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginResult =
                    _customerRegistrationService.ValidateCustomer(model.LoginName, model.Password);
                switch (loginResult)
                {
                    case CustomerLoginResults.Successful:
                        {
                            Customer customer = null;
                            switch (_customerSettings.CurrentAuthenticationType)
                            {
                                case AuthenticationType.Email:
                                    customer = _customerService.GetCustomerByEmail(model.LoginName);
                                    break;
                                case AuthenticationType.Username:
                                    customer = _customerService.GetCustomerByUsername(model.LoginName);
                                    break;
                                case AuthenticationType.UsernameOrEmail:
                                    customer = _customerService.GetCustomerByUsernameOrEmail(model.LoginName);
                                    break;
                            }

                            _authenticationService.SignIn(customer, model.RememberMe);

                            _eventPublisher.Publish(new CustomerLoggedinEvent(customer));

                            _customerActivityService.InsertActivity(customer, "Public.Login", _localizationService.GetResource("ActivityLog.Public.Login"));

                            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                                return RedirectToRoute("HomePage");

                            return Redirect(returnUrl);
                        }
                    case CustomerLoginResults.CustomerNotExist:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.CustomerNotExist"));
                        break;
                    case CustomerLoginResults.NotActive:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotActive"));
                        break;
                    case CustomerLoginResults.LockedOut:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.LockedOut"));
                        break;
                    case CustomerLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials"));
                        break;
                }
            }
            return View(model);
        }

        public virtual ActionResult Logout()
        {

            _eventPublisher.Publish(new CustomerLoggedOutEvent(_workContext.CurrentCustomer));

            _customerActivityService.InsertActivity("PublicStore.Logout", _localizationService.GetResource("ActivityLog.PublicStore.Logout"));

            _authenticationService.SignOut();

            if (_commonSettings.DisplayEuCookieLawWarning)
            {
                //不在页面跳转到主页时显示警告
                //可能是同一个人
                //我们仅需要保证下一次网站请求显示警告
                TempData["ransurotto.IgnoreEuCookieLawWarning"] = true;
            }

            return RedirectToRoute("HomePage");
        }

        #endregion

    }
}