using FluentValidation;
using RANSUROTTO.BLOG.Admin.Models.Blogs;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Data.Context;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Admin.Validators.Blogs
{
    public class CategoryValidator : BaseValidator<CategoryModel>
    {
        public CategoryValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.Categories.Fields.Name.Required"));

            SetDatabaseValidationRules<Category>(dbContext);
        }
    }
}