using FluentValidation;
using System.Globalization;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Admin.Models.Localization;

namespace RANSUROTTO.BLOG.Admin.Validators.Localization
{
    public class LanguageValidator : BaseValidator<LanguageModel>
    {
        public LanguageValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Languages.Fields.Name.Required"));
            RuleFor(x => x.LanguageCulture).Must(x =>
                {
                    try
                    {
                        //尝试创建一个文化信息对象
                        //如果"显示区域"是错误的，则将引发异常。
                        var culture = new CultureInfo(x);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                })
                .WithMessage(localizationService.GetResource("Admin.Configuration.Languages.Fields.LanguageCulture.Validation"));

            RuleFor(x => x.UniqueSeoCode).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Languages.Fields.UniqueSeoCode.Required"));
            RuleFor(x => x.UniqueSeoCode).Length(2).WithMessage(localizationService.GetResource("Admin.Configuration.Languages.Fields.UniqueSeoCode.Length"));
        }
    }
}