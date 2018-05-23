using FluentValidation;
using RANSUROTTO.BLOG.Admin.Models.Interesting;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Admin.Validators.Interesting
{
    public class IdeaValidator : BaseValidator<IdeaModel>
    {
        public IdeaValidator(ILocalizationService localizationService)
        {
            this.RuleFor(p => p.Body).NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Idea.Fields.Body.Required"));
        }
    }
}