using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using RANSUROTTO.BLOG.Admin.Models.Customers;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;
using RANSUROTTO.BLOG.Core.Domain.Customers.Setting;
using RANSUROTTO.BLOG.Data.Context;
using RANSUROTTO.BLOG.Services.Customers;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Admin.Validators.Customers
{
    public class CustomerValidator : BaseValidator<CustomerModel>
    {

        public CustomerValidator(ILocalizationService localizationService,
            ICustomerService customerService,
            CustomerSettings customerSettings,
            IDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"))
                //仅针对注册用户
                .When(x => IsRegisteredCustomerRoleChecked(x, customerService));

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Common.WrongUsername"))
                //仅针对注册用户
                .When(x => IsRegisteredCustomerRoleChecked(x, customerService));

            if (customerSettings.CompanyRequired && customerSettings.CompanyEnabled)
            {
                RuleFor(x => x.Company)
                    .NotEmpty()
                    .WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Company.Required"))
                    //仅针对注册用户
                    .When(x => IsRegisteredCustomerRoleChecked(x, customerService));
            }
            if (customerSettings.PhoneRequired && customerSettings.PhoneEnabled)
            {
                RuleFor(x => x.Phone)
                    .NotEmpty()
                    .WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Phone.Required"))
                    //仅针对注册用户
                    .When(x => IsRegisteredCustomerRoleChecked(x, customerService));
            }

            SetDatabaseValidationRules<Customer>(dbContext);
        }

        /// <summary>
        /// 检查用户是否属于注册用户角色
        /// </summary>
        /// <returns>结果</returns>
        private bool IsRegisteredCustomerRoleChecked(CustomerModel model, ICustomerService customerService)
        {
            var allCustomerRoles = customerService.GetAllCustomerRoles(true);
            var newCustomerRoles = new List<CustomerRole>();
            foreach (var customerRole in allCustomerRoles)
                if (model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                    newCustomerRoles.Add(customerRole);

            bool isInRegisteredRole = newCustomerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Registered) != null;
            return isInRegisteredRole;
        }

    }
}