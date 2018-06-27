using FluentValidation;
using RANSUROTTO.BLOG.Admin.Models.Interesting;
using RANSUROTTO.BLOG.Core.Domain.Interesting;
using RANSUROTTO.BLOG.Data.Context;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Admin.Validators.Interesting
{
    public class IdeaValidator : BaseValidator<IdeaModel>
    {
        public IdeaValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            this.RuleFor(p => p.Body).NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Idea.Fields.Body.Required"));

            SetDatabaseValidationRules<Idea>(dbContext);
        }
    }
}