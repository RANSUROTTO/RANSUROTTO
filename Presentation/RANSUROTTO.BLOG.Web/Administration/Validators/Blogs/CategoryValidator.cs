using FluentValidation;
using RANSUROTTO.BLOG.Admin.Models.Blogs;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Admin.Validators.Blogs
{
    public class CategoryValidator : BaseValidator<CategoryModel>
    {
        public CategoryValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.Categories.Fields.Name.Required"));
        }
    }
}