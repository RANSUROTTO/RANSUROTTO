using System;
using System.Linq;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Models.Blogs;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Framework.Extensions;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Services.Blogs;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class BlogController : BaseAdminController
    {

        #region Fields

        private readonly IBlogPostTagService _blogPostTagService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ICustomerActivityService _customerActivityService;

        #endregion

        #region Constructor

        public BlogController(IBlogPostTagService blogPostTagService, ICustomerActivityService customerActivityService)
        {
            _blogPostTagService = blogPostTagService;
            _customerActivityService = customerActivityService;
        }

        #endregion

        #region Blog posts



        #endregion

        #region Blog post tags

        public virtual ActionResult BlogPostTags()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult BlogPostTags(DataSourceRequest command)
        {
            var tags = _blogPostTagService.GetAllBlogPostTags()
                .OrderByDescending(t => _blogPostTagService.GetBlogPostCount(t.Id))
                .Select(t => new BlogPostTagModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    BlogCount = _blogPostTagService.GetBlogPostCount(t.Id)
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = tags.PagedForCommand(command),
                Total = tags.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult EditBlogPostTag(int id)
        {
            var blogPostTag = _blogPostTagService.GetBlogPostTagById(id);
            if (blogPostTag == null)
                return RedirectToAction("List");

            var model = new BlogPostTagModel
            {
                Id = blogPostTag.Id,
                Name = blogPostTag.Name,
                BlogCount = _blogPostTagService.GetBlogPostCount(blogPostTag.Id)
            };

            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = blogPostTag.GetLocalized(x => x.Name, languageId, false, false);
            });

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult EditBlogPostTag(string btnId, string formId, BlogPostTagModel model)
        {
            var blogPostTag = _blogPostTagService.GetBlogPostTagById(model.Id);
            if (blogPostTag == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                blogPostTag.Name = model.Name;
                _blogPostTagService.UpdateBlogPostTag(blogPostTag);
                //Locales
                UpdateLocales(blogPostTag, model);

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                return View(model);
            }
            return View(model);
        }

        public virtual ActionResult BlogPostTagDelete(int id)
        {
            var blogPostTag = _blogPostTagService.GetBlogPostTagById(id);
            if (blogPostTag == null)
                throw new ArgumentException("未找到具有指定ID的博客文章标签");

            _blogPostTagService.DeleteBlogPostTag(blogPostTag);

            return new NullJsonResult();
        }

        #endregion

        #region Utilities

        protected virtual void UpdateLocales(BlogPostTag blogPostTag, BlogPostTagModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(blogPostTag,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }

        #endregion

        public virtual ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [HttpGet]
        public virtual ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult List(DataSourceRequest command)
        {
            return View();
        }

    }
}