using FluentValidation;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Admin.Models.Localization;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Data.Context;

namespace RANSUROTTO.BLOG.Admin.Validators.Localization
{
    public class LanguageResourceValidator : BaseValidator<LanguageResourceModel>
    {
        public LanguageResourceValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Configuration.Languages.Resources.Fields.Name.Required"));
            RuleFor(x => x.Value)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Configuration.Languages.Resources.Fields.Value.Required"));

            SetDatabaseValidationRules<LocaleStringResource>(dbContext);
        }
    }
}