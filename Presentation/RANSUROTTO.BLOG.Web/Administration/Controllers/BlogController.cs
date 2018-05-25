using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Extensions;
using RANSUROTTO.BLOG.Admin.Helpers;
using RANSUROTTO.BLOG.Admin.Models.Blogs;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Core.Domain.Blogs.Enum;
using RANSUROTTO.BLOG.Framework.Extensions;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Services.Blogs;
using RANSUROTTO.BLOG.Services.Catalog;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class BlogController : BaseAdminController
    {

        #region Fields

        private readonly IBlogService _blogService;
        private readonly IBlogPostTagService _blogPostTagService;
        private readonly ICategoryService _categoryService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Constructor

        public BlogController(IBlogService blogService, IBlogPostTagService blogPostTagService, ICategoryService categoryService, ILanguageService languageService, ILocalizationService localizationService, ILocalizedEntityService localizedEntityService, ICustomerActivityService customerActivityService, ICacheManager cacheManager)
        {
            _blogService = blogService;
            _blogPostTagService = blogPostTagService;
            _categoryService = categoryService;
            _languageService = languageService;
            _localizationService = localizationService;
            _localizedEntityService = localizedEntityService;
            _customerActivityService = customerActivityService;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Blog posts

        public virtual ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [HttpGet]
        public virtual ActionResult List()
        {
            var model = new BlogPostListModel();

            /*可用类目搜索项*/
            model.AvailableCategories.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            var categories = SelectListHelper.GetBlogCategoryList(_categoryService, _cacheManager, true);
            foreach (var c in categories)
                model.AvailableCategories.Add(c);

            /*可用发布状态搜索项*/
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Catalog.Products.List.SearchPublished.All"), Value = "0" });
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Catalog.Products.List.SearchPublished.PublishedOnly"), Value = "1" });
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Catalog.Products.List.SearchPublished.UnpublishedOnly"), Value = "2" });

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult List(DataSourceRequest command, BlogPostListModel model)
        {
            var categoryIds = new List<int> { model.SearchCategoryId };
            if (model.SearchIncludeSubCategories && model.SearchCategoryId > 0)
                categoryIds.AddRange(GetChildCategoryIds(model.SearchCategoryId));

            bool? overridePublished = null;
            if (model.SearchPublishedId == 1)
                overridePublished = true;
            else if (model.SearchPublishedId == 2)
                overridePublished = false;

            var blogPosts = _blogService.GetAllBlogPosts(command.Page - 1, command.PageSize, categoryIds: categoryIds,
                keywords: model.SearchTitle, overridePublished: overridePublished, orderBy: BlogSortingEnum.CreatedOn);

            var gridModel = new DataSourceResult();
            gridModel.Data = blogPosts.Select(x =>
            {
                var m = x.ToModel();
                m.Body = "";
                m.BodyOverview = "";
                m.AuthorEmail = x.Author != null ? x.Author.Email : null;
                return m;
            });
            gridModel.Total = blogPosts.TotalCount;

            return Json(gridModel);
        }

        public virtual ActionResult Create()
        {
            var model = new BlogPostModel();
            model.AllowComments = true;

            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var blogPost = _blogService.GetBlogPostById(id);
            if (blogPost == null)
                return RedirectToAction("List");

            var model = blogPost.ToModel();
            AddLocales(_languageService, model.Locales);
            PrepareCategoryMappingModel(model, null.false);
            return View(model);
        }

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

        [NonAction]
        protected virtual void PrepareCategoryMappingModel(BlogPostModel model, BlogPost product, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!excludeProperties && product != null)
                model.SelectedCategoryIds = _categoryService.GetAllBlogCategoriesByParentCategoryId(product.Id, true).Select(c => c.CategoryId).ToList();

            var allCategories = SelectListHelper.GetBlogCategoryList(_categoryService, _cacheManager, true);
            foreach (var c in allCategories)
            {
                c.Selected = model.SelectedCategoryIds.Contains(int.Parse(c.Value));
                model.AvailableCategories.Add(c);
            }
        }

        [NonAction]
        protected virtual void UpdateLocales(BlogPost blogPost, BlogPostModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(blogPost, p => p.Title, localized.Title,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(blogPost, p => p.Title, localized.BodyOverview,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(blogPost, p => p.Title, localized.Body,
                    localized.LanguageId);
            }
        }

        [NonAction]
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

        [NonAction]
        protected virtual List<int> GetChildCategoryIds(int parentCategoryId)
        {
            var categoriesIds = new List<int>();
            var categories = _categoryService.GetAllBlogCategoriesByParentCategoryId(parentCategoryId, true);
            foreach (var category in categories)
            {
                categoriesIds.Add(category.Id);
                categoriesIds.AddRange(GetChildCategoryIds(category.Id));
            }
            return categoriesIds;
        }

        #endregion

    }
}