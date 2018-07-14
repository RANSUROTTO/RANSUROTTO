using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Extensions;
using RANSUROTTO.BLOG.Admin.Helpers;
using RANSUROTTO.BLOG.Admin.Models.Blogs;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Core.Domain.Blogs.Enum;
using RANSUROTTO.BLOG.Framework.Controllers;
using RANSUROTTO.BLOG.Framework.Extensions;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Services.Blogs;
using RANSUROTTO.BLOG.Services.Catalog;
using RANSUROTTO.BLOG.Services.Helpers;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;
using RANSUROTTO.BLOG.Services.Security;

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
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Constructor

        public BlogController(IBlogService blogService, IBlogPostTagService blogPostTagService, ICategoryService categoryService, ILanguageService languageService, ILocalizationService localizationService, ILocalizedEntityService localizedEntityService, ICustomerActivityService customerActivityService, IPermissionService permissionService, IWorkContext workContext, IDateTimeHelper dateTimeHelper, ICacheManager cacheManager)
        {
            _blogService = blogService;
            _blogPostTagService = blogPostTagService;
            _categoryService = categoryService;
            _languageService = languageService;
            _localizationService = localizationService;
            _localizedEntityService = localizedEntityService;
            _customerActivityService = customerActivityService;
            _permissionService = permissionService;
            _workContext = workContext;
            _dateTimeHelper = dateTimeHelper;
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
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlogposts))
                return AccessDeniedView();

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
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlogposts))
                return AccessDeniedKendoGridJson();

            var categoryIds = new List<int> { model.SearchCategoryId };
            if (model.SearchIncludeSubCategories && model.SearchCategoryId > 0)
                categoryIds.AddRange(GetChildCategoryIds(model.SearchCategoryId));

            bool? overridePublished = null;
            if (model.SearchPublishedId == 1)
                overridePublished = true;
            else if (model.SearchPublishedId == 2)
                overridePublished = false;

            var blogPosts = _blogService.GetAllBlogPosts(command.Page - 1, command.PageSize, categoryIds: categoryIds,
                keywords: model.SearchTitle, overridePublished: overridePublished, showDeleted: model.ShowDeleted, orderBy: BlogSortingEnum.CreatedOnDesc);

            var utcNow = DateTime.UtcNow;
            var gridModel = new DataSourceResult();
            gridModel.Data = blogPosts.Select(x =>
            {
                var m = x.ToModel();
                m.Body = "";
                m.BodyOverview = "";
                m.AuthorEmail = x.Author != null ? x.Author.Email : null;
                m.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                m.UpdatedOn = _dateTimeHelper.ConvertToUserTime(x.UpdatedOnUtc, DateTimeKind.Utc);
                m.Published = (x.AvailableStartDateUtc == null || x.AvailableStartDateUtc < utcNow)
                              && (x.AvailableEndDateUtc == null || x.AvailableEndDateUtc > utcNow);
                return m;
            });
            gridModel.Total = blogPosts.TotalCount;

            return Json(gridModel);
        }

        public virtual ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlogposts))
                return AccessDeniedView();

            var model = new BlogPostModel();
            PrepareBlogPostModel(model, null);
            AddLocales(_languageService, model.Locales);
            PrepareCategoryMappingModel(model, null, true);
            model.AllowComments = true;

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Create(BlogPostModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlogposts))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var blogPost = model.ToEntity();
                blogPost.UpdatedOnUtc = DateTime.UtcNow;
                blogPost.AuthorId = _workContext.CurrentCustomer.Id;
                _blogService.InsertBlogPost(blogPost);
                //Locales
                UpdateLocales(blogPost, model);
                //Categories
                SaveCategoryMappings(blogPost, model);
                //Tags
                _blogPostTagService.UpdateBlogPostTags(blogPost, ParseBlogPostTags(model.BlogPostTags));

                //Activity log
                _customerActivityService.InsertActivity("AddNewBlogPost", _localizationService.GetResource("ActivityLog.AddNewBlogPost"), blogPost.Title);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Blog.Posts.Added"));

                if (continueEditing)
                {
                    SaveSelectedTabName();
                    return RedirectToAction("Edit", new { id = blogPost.Id });
                }
                return RedirectToAction("List");
            }
            PrepareCategoryMappingModel(model, null, true);
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlogposts))
                return AccessDeniedView();

            var blogPost = _blogService.GetBlogPostById(id);
            if (blogPost == null)
                return RedirectToAction("List");

            var model = blogPost.ToModel();
            PrepareBlogPostModel(model, blogPost);
            AddLocales(_languageService, model.Locales);
            PrepareCategoryMappingModel(model, blogPost, false);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Edit(BlogPostModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlogposts))
                return AccessDeniedView();

            var blogPost = _blogService.GetBlogPostById(model.Id);

            if (blogPost == null || blogPost.Deleted)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                blogPost = model.ToEntity(blogPost);
                blogPost.UpdatedOnUtc = DateTime.UtcNow;
                _blogService.UpdateBlogPost(blogPost);
                //Locales
                UpdateLocales(blogPost, model);
                //Tags
                _blogPostTagService.UpdateBlogPostTags(blogPost, ParseBlogPostTags(model.BlogPostTags));
                //Categories
                SaveCategoryMappings(blogPost, model);

                //Activity log
                _customerActivityService.InsertActivity("EditBlogPost", _localizationService.GetResource("ActivityLog.EditBlogPost"), blogPost.Title);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Blog.Posts.Updated"));

                if (continueEditing)
                {
                    SaveSelectedTabName();
                    return RedirectToAction("Edit", new { id = blogPost.Id });
                }
                return RedirectToAction("List");
            }
            PrepareCategoryMappingModel(model, blogPost, true);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlogposts))
                return AccessDeniedView();

            var blogPost = _blogService.GetBlogPostById(id);
            if (blogPost == null)
                return RedirectToAction("List");

            _blogService.DeleteBlogPost(blogPost);

            _customerActivityService.InsertActivity("DeleteBlogPost", _localizationService.GetResource("ActivityLog.EditBlogPost"), blogPost.Title);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Blog.Posts.Deleted"));

            return RedirectToAction("List");
        }

        #endregion

        #region Blog post tags

        public virtual ActionResult BlogPostTags()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlogTags))
                return AccessDeniedView();

            return View();
        }

        [HttpPost]
        public virtual ActionResult BlogPostTags(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlogTags))
                return AccessDeniedKendoGridJson();

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
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlogTags))
                return AccessDeniedView();

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
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlogTags))
                return AccessDeniedView();

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
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageBlogTags))
                return AccessDeniedView();

            var blogPostTag = _blogPostTagService.GetBlogPostTagById(id);
            if (blogPostTag == null)
                throw new ArgumentException("未找到具有指定ID的博客文章标签");

            _blogPostTagService.DeleteBlogPostTag(blogPostTag);

            return new NullJsonResult();
        }

        #endregion

        #region Utilities

        [NonAction]
        protected virtual void PrepareBlogPostModel(BlogPostModel model, BlogPost blogPost)
        {
            if (blogPost != null)
            {
                //Datetime
                model.CreatedOn = _dateTimeHelper.ConvertToUserTime(blogPost.CreatedOnUtc, DateTimeKind.Utc);
                model.UpdatedOn = _dateTimeHelper.ConvertToUserTime(blogPost.UpdatedOnUtc, DateTimeKind.Utc);

                //Tags
                var result = new StringBuilder();
                for (int i = 0; i < blogPost.BlogPostTags.Count; i++)
                {
                    var bt = blogPost.BlogPostTags.ToList()[i];
                    result.Append(bt.Name);
                    if (i != blogPost.BlogPostTags.Count - 1)
                        result.Append(", ");
                }
                model.BlogPostTags = result.ToString();
            }
        }

        [NonAction]
        protected virtual void PrepareCategoryMappingModel(BlogPostModel model, BlogPost blogPost, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!excludeProperties && blogPost != null)
                model.SelectedCategoryIds = _categoryService.GetCategoriesByBlogPostId(blogPost.Id, true).Select(c => c.BlogCategoryId).ToList();

            var allCategories = SelectListHelper.GetBlogCategoryList(_categoryService, _cacheManager, true);
            foreach (var c in allCategories)
            {
                c.Selected = model.SelectedCategoryIds.Contains(int.Parse(c.Value));
                model.AvailableCategories.Add(c);
            }
        }

        [NonAction]
        protected virtual void SaveCategoryMappings(BlogPost blogPost, BlogPostModel model)
        {
            var existingBlogCategories = _categoryService.GetCategoriesByBlogPostId(blogPost.Id, true);

            //删除未选中的
            foreach (var existingBlogCategory in existingBlogCategories)
                if (!model.SelectedCategoryIds.Contains(existingBlogCategory.BlogCategoryId))
                    _categoryService.DeleteBlogPostCategory(existingBlogCategory);

            //添加类目
            foreach (var categoryId in model.SelectedCategoryIds)
                if (existingBlogCategories.FindBlogPostCategory(blogPost.Id, categoryId) == null)
                {
                    var displayOrder = 1;
                    var existingCategoryMapping = _categoryService.GetCategoriesByBlogPostId(categoryId, showHidden: true);
                    if (existingCategoryMapping.Any())
                        displayOrder = existingCategoryMapping.Max(x => x.DisplayOrder) + 1;
                    _categoryService.InsertBlogPostCategory(new BlogPostCategory
                    {
                        BlogPostId = blogPost.Id,
                        BlogCategoryId = categoryId,
                        DisplayOrder = displayOrder
                    });
                }
        }

        [NonAction]
        protected virtual string[] ParseBlogPostTags(string blogPostTags)
        {
            var result = new List<string>();
            if (!string.IsNullOrWhiteSpace(blogPostTags))
            {
                string[] values = blogPostTags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string val1 in values)
                    if (!string.IsNullOrEmpty(val1.Trim()))
                        result.Add(val1.Trim());
            }
            return result.ToArray();
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
            var categories = _categoryService.GetAllCategoriesByParentCategoryId(parentCategoryId, true);
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