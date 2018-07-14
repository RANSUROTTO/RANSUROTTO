using System;
using System.Linq;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Extensions;
using RANSUROTTO.BLOG.Admin.Helpers;
using RANSUROTTO.BLOG.Admin.Models.Blogs;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Framework.Controllers;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Services.Catalog;
using RANSUROTTO.BLOG.Services.Customers;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;
using RANSUROTTO.BLOG.Services.Security;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class CategoryController : BaseAdminController
    {

        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Constructor

        public CategoryController(ICategoryService categoryService, ICustomerService customerService, ILanguageService languageService, ILocalizedEntityService localizedEntityService, ILocalizationService localizationService, ICustomerActivityService customerActivityService, IWorkContext workContext, ICacheManager cacheManager, IPermissionService permissionService)
        {
            _categoryService = categoryService;
            _customerService = customerService;
            _languageService = languageService;
            _localizedEntityService = localizedEntityService;
            _localizationService = localizationService;
            _customerActivityService = customerActivityService;
            _workContext = workContext;
            _cacheManager = cacheManager;
            _permissionService = permissionService;
        }

        #endregion

        #region List

        public virtual ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            var model = new CategoryListModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult List(DataSourceRequest command, CategoryListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedKendoGridJson();

            var blogCategories = _categoryService.GetAllCategories(model.SearchCategoryName,
                command.Page - 1, command.PageSize, true);

            var gridModel = new DataSourceResult
            {
                Data = blogCategories.Select(x =>
                {
                    var categoryModel = x.ToModel();
                    categoryModel.Breadcrumb = x.GetFormattedBreadCrumb(_categoryService);
                    return categoryModel;
                }),
                Total = blogCategories.TotalCount
            };

            return Json(gridModel);
        }

        #endregion

        #region Create / Edit / Delete

        public virtual ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            var model = new CategoryModel();
            //locales
            AddLocales(_languageService, model.Locales);
            //categories
            PrepareAllCategoriesModel(model);
            //default values
            model.Published = true;

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Create(CategoryModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var category = model.ToEntity();
                category.CreatedOnUtc = DateTime.UtcNow;//可以忽略
                category.UpdatedOnUtc = DateTime.UtcNow;
                _categoryService.InsertCategory(category);
                //locales
                UpdateLocales(category, model);

                //actvity log
                _customerActivityService.InsertActivity("AddNewCategory", _localizationService.GetResource("ActivityLog.AddNewCategory"), category.Name);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Blog.Categories.Added"));

                if (continueEditing)
                {
                    SaveSelectedTabName();
                    return RedirectToAction("Edit", new { id = category.Id });
                }
                return RedirectToAction("List");
            }
            PrepareAllCategoriesModel(model);
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            var category = _categoryService.GetCategoryById(id);
            if (category == null || category.Deleted)
                return RedirectToAction("List");

            var model = category.ToModel();
            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = category.GetLocalized(x => x.Name, languageId, false, false);
                locale.Description = category.GetLocalized(x => x.Description, languageId, false, false);
                locale.MetaKeywords = category.GetLocalized(x => x.MetaKeywords, languageId, false, false);
                locale.MetaDescription = category.GetLocalized(x => x.MetaDescription, languageId, false, false);
                locale.MetaTitle = category.GetLocalized(x => x.MetaTitle, languageId, false, false);
                locale.SeName = category.GetLocalized(x => x.SeName, languageId, false, false);
            });
            //categories
            PrepareAllCategoriesModel(model);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Edit(CategoryModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            var category = _categoryService.GetCategoryById(model.Id);
            if (category == null || category.Deleted)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                category = model.ToEntity(category);
                category.UpdatedOnUtc = DateTime.UtcNow;
                _categoryService.UpdateCategory(category);
                //locales
                UpdateLocales(category, model);

                //activity log
                _customerActivityService.InsertActivity("EditCategory", _localizationService.GetResource("ActivityLog.EditCategory"), category.Name);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Blog.Categories.Updated"));

                if (continueEditing)
                {
                    SaveSelectedTabName();
                    return RedirectToAction("Edit", new { id = category.Id });
                }
                return RedirectToAction("List");
            }
            PrepareAllCategoriesModel(model);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            var category = _categoryService.GetCategoryById(id);
            if (category == null)
                return RedirectToAction("List");

            _categoryService.DeleteCategory(category);

            //activity log
            _customerActivityService.InsertActivity("DeleteCategory", _localizationService.GetResource("ActivityLog.DeleteCategory"), category.Name);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Blog.Categories.Deleted"));

            return RedirectToAction("List");
        }

        #endregion

        #region Utilities

        [NonAction]
        protected virtual void PrepareAllCategoriesModel(CategoryModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.AvailableCategories.Add(new SelectListItem
            {
                Text = _localizationService.GetResource("Admin.Catalog.Categories.Fields.Parent.None"),
                Value = "0"
            });

            var categories = SelectListHelper.GetBlogCategoryList(_categoryService, _cacheManager, true);
            foreach (var c in categories)
                model.AvailableCategories.Add(c);
        }

        [NonAction]
        protected virtual void UpdateLocales(Category category, CategoryModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.Description,
                    localized.Description,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.MetaKeywords,
                    localized.MetaKeywords,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.MetaDescription,
                    localized.MetaDescription,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.MetaTitle,
                    localized.MetaTitle,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.SeName,
                    localized.SeName,
                    localized.LanguageId);

            }
        }

        #endregion

    }
}