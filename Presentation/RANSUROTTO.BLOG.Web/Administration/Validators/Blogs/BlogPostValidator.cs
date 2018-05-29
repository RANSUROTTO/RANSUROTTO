using FluentValidation;
using RANSUROTTO.BLOG.Admin.Models.Blogs;
using RANSUROTTO.BLOG.Framework.Validators;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Admin.Validators.Blogs
{
    public class BlogPostValidator : BaseValidator<BlogPostModel>
    {
        public BlogPostValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.Posts.Fields.Title.Required"));

            RuleFor(x => x.Body)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.Posts.Fields.Body.Required"));

            RuleFor(x => x.BlogPostTags)
                .Must(x => x == null || !x.Contains("."))
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.Posts.Fields.Tags.NoDots"));

        }
    }
}