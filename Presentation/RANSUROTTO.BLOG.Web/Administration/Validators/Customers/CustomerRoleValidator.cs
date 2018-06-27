using FluentValidation;
using RANSUROTTO.BLOG.Admin.Models.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Data.Context;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Admin.Validators.Customers
{
    public class CustomerRoleValidator : BaseValidator<CustomerRoleModel>
    {
        public CustomerRoleValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.Name.Required"));
            RuleFor(x => x.SystemName).NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.SystemName.Required"));

            SetDatabaseValidationRules<CustomerRole>(dbContext);
        }
    }
}