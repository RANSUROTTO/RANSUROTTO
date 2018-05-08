using FluentValidation;
using RANSUROTTO.BLOG.Admin.Models.Tasks;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Admin.Validators.Tasks
{
    public class ScheduleTaskValidator : BaseValidator<ScheduleTaskModel>
    {

        public ScheduleTaskValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.System.ScheduleTasks.Name.Required"));
            RuleFor(x => x.Seconds)
                .GreaterThan(0)
                .WithMessage(localizationService.GetResource("Admin.System.ScheduleTasks.Seconds.Positive"));
        }

    }
}