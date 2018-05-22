using FluentValidation;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Admin.Models.Localization;

namespace RANSUROTTO.BLOG.Admin.Validators.Localization
{
    public class LanguageResourceValidator : BaseValidator<LanguageResourceModel>
    {
        public LanguageResourceValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Languages.Resources.Fields.Name.Required"));
            RuleFor(x => x.Value).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Languages.Resources.Fields.Value.Required"));
        }
    }
}