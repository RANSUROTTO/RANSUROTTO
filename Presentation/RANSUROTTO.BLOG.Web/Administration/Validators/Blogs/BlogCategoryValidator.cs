using FluentValidation;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Admin.Validators.Blogs
{
    public class BlogCategoryValidator : BaseValidator<BlogCategory>
    {
        public BlogCategoryValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.BlogCategorys.Fields.Name.Required"));
        }
    }
}