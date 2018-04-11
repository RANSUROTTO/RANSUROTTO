using System;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;
using RANSUROTTO.BLOG.Core.Domain.Customers.Setting;
using RANSUROTTO.BLOG.Framework.Security;
using RANSUROTTO.BLOG.Service.Authentication;
using RANSUROTTO.BLOG.Service.Customers;
using RANSUROTTO.BLOG.Service.Events;
using RANSUROTTO.BLOG.Service.Localization;
using RANSUROTTO.BLOG.Web.Models.Customer;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    public class CustomerController : BasePublicController
    {

        #region Fields

        private readonly ICustomerService _customerService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly CustomerSettings _customerSettings;

        #endregion

        #region Constructor

        public CustomerController(ICustomerService customerService, ICustomerRegistrationService customerRegistrationService, ILocalizationService localizationService, CustomerSettings customerSettings)
        {
            _customerService = customerService;
            _customerRegistrationService = customerRegistrationService;
            _localizationService = localizationService;
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
            return View();
        }

        #endregion


    }
}