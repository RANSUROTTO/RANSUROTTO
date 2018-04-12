using System;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;
using RANSUROTTO.BLOG.Core.Domain.Customers.Setting;
using RANSUROTTO.BLOG.Service.Events;
using RANSUROTTO.BLOG.Service.Localization;
using RANSUROTTO.BLOG.Service.Security;

namespace RANSUROTTO.BLOG.Service.Customers
{
    public class CustomerRegistrationService : ICustomerRegistrationService
    {

        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptionService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly CustomerSettings _customerSettings;

        #endregion

        #region Constructor

        public CustomerRegistrationService(ICustomerService customerService, IEncryptionService encryptionService, ILocalizationService localizationService, IWorkContext workContext, IEventPublisher eventPublisher, CustomerSettings customerSettings)
        {
            _customerService = customerService;
            _encryptionService = encryptionService;
            _localizationService = localizationService;
            _workContext = workContext;
            _eventPublisher = eventPublisher;
            _customerSettings = customerSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 用户验证
        /// </summary>
        /// <param name="usernameOrEmail">用户名/Email</param>
        /// <param name="password">登录密码</param>
        /// <returns>验证结果</returns>
        public CustomerLoginResults ValidateCustomer(string usernameOrEmail, string password)
        {
            Customer customer = null;
            switch (_customerSettings.CurrentAuthenticationType)
            {
                case AuthenticationType.Email:
                    customer = _customerService.GetCustomerByEmail(usernameOrEmail);
                    break;
                case AuthenticationType.Username:
                    customer = _customerService.GetCustomerByUsername(usernameOrEmail);
                    break;
                case AuthenticationType.UsernameOrEmail:
                    customer = _customerService.GetCustomerByUsernameOrEmail(usernameOrEmail);
                    break;
            }

            if (customer == null)
                return CustomerLoginResults.CustomerNotExist;

            if (!customer.Active)
                return CustomerLoginResults.NotActive;

            if (!customer.IsRegistered())
                return CustomerLoginResults.NotRegistered;

            if (customer.CannotLoginUntilDateUtc.HasValue && customer.CannotLoginUntilDateUtc.Value > DateTime.UtcNow)
                return CustomerLoginResults.LockedOut;

            if (!PasswordsMatch(_customerService.GetCurrentPassword(customer.Id), password))
            {
                //密码错误
                customer.FailedLoginAttempts++;
                if (_customerSettings.FailedPasswordAllowedAttempts > 0 &&
                    customer.FailedLoginAttempts >= _customerSettings.FailedPasswordAllowedAttempts)
                {
                    //锁定用户
                    customer.CannotLoginUntilDateUtc = DateTime.UtcNow.AddMinutes(_customerSettings.FailedPasswordLockoutMinutes);
                    customer.FailedLoginAttempts = 0;
                }
                _customerService.UpdateCustomer(customer);

                return CustomerLoginResults.WrongPassword;
            }

            customer.FailedLoginAttempts = 0;
            customer.CannotLoginUntilDateUtc = null;
            customer.LastLoginDateUtc = DateTime.UtcNow;
            _customerService.UpdateCustomer(customer);

            return CustomerLoginResults.Successful;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 检查输入的密码是否和保存的密码相匹配
        /// </summary>
        /// <param name="customerPassword">保存的用户密码</param>
        /// <param name="enteredPassword">输入的用户密码</param>
        /// <returns>结果</returns>
        protected bool PasswordsMatch(CustomerPassword customerPassword, string enteredPassword)
        {
            if (customerPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var enteredEncryptPassword = string.Empty;
            switch (customerPassword.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    enteredEncryptPassword = enteredPassword;
                    break;
                case PasswordFormat.Encrypted:
                    enteredEncryptPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
                case PasswordFormat.Hashed:
                    enteredEncryptPassword = _encryptionService.CreatePasswordHash(enteredPassword, customerPassword.PasswordSalt, _customerSettings.HashedPasswordFormat);
                    break;
            }
            return customerPassword.Password.Equals(enteredEncryptPassword);
        }

        #endregion

    }
}
