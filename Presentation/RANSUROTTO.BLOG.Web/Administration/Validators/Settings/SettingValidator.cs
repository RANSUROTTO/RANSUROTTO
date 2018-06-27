using FluentValidation;
using RANSUROTTO.BLOG.Admin.Models.Settings;
using RANSUROTTO.BLOG.Core.Domain.Configuration;
using RANSUROTTO.BLOG.Data.Context;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Admin.Validators.Settings
{
    public class SettingValidator : BaseValidator<SettingModel>
    {
        public SettingValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Configuration.Settings.AllSettings.Fields.Name.Required"));

            SetDatabaseValidationRules<Setting>(dbContext);
        }
    }
}