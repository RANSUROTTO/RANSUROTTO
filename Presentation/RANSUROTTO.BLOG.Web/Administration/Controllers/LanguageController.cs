using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Services.Logging;
using RANSUROTTO.BLOG.Admin.Extensions;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Framework.Security;
using RANSUROTTO.BLOG.Framework.Extensions;
using RANSUROTTO.BLOG.Framework.Controllers;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Admin.Models.Localization;
using RANSUROTTO.BLOG.Core.Helper;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class LanguageController : BaseAdminController
    {

        #region Fields

        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;

        #endregion

        #region Constructor

        public LanguageController(ILanguageService languageService, ILocalizationService localizationService, ICustomerActivityService customerActivityService)
        {
            _languageService = languageService;
            _localizationService = localizationService;
            _customerActivityService = customerActivityService;
        }

        #endregion

        #region Languages

        public virtual ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult List(DataSourceRequest command)
        {
            var languages = _languageService.GetAllLanguages(true);
            var gridModel = new DataSourceResult
            {
                Data = languages.Select(x => x.ToModel()),
                Total = languages.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult Create()
        {
            var model = new LanguageModel
            {
                Published = true
            };
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Create(LanguageModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var language = model.ToEntity();
                _languageService.InsertLanguage(language);
                //活动日志
                _customerActivityService.InsertActivity("AddNewLanguage", _localizationService.GetResource("ActivityLog.AddNewLanguage"), language.Id);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Languages.Added"));

                if (continueEditing)
                {
                    SaveSelectedTabName();
                    return RedirectToAction("Edit", new { id = language.Id });
                }

                return RedirectToAction("List");
            }
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var language = _languageService.GetLanguageById(id);
            if (language == null)
                return RedirectToAction("List");

            var model = language.ToModel();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Edit(LanguageModel model, bool continueEditing)
        {
            var language = _languageService.GetLanguageById(model.Id);
            if (language == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                //确保至少保留有一个已发布的语言
                var allLanguages = _languageService.GetAllLanguages();
                if (allLanguages.Count == 1 && allLanguages[0].Id == language.Id &&
                    !model.Published)
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Configuration.Languages.PublishedLanguageRequired"));
                    return RedirectToAction("Edit", new { id = language.Id });
                }

                language = model.ToEntity(language);
                _languageService.UpdateLanguage(language);
                //活动日志
                _customerActivityService.InsertActivity("EditLanguage", _localizationService.GetResource("ActivityLog.EditLanguage"), language.Id);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Languages.Updated"));

                if (continueEditing)
                {
                    SaveSelectedTabName();
                    return RedirectToAction("Edit", new { id = language.Id });
                }
                return RedirectToAction("List");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Delete(long id)
        {
            var language = _languageService.GetLanguageById(id);
            if (language == null)
                return RedirectToAction("List");

            //确保至少保留有一个已发布的语言
            var allLanguages = _languageService.GetAllLanguages();
            if (allLanguages.Count == 1 && allLanguages[0].Id == language.Id)
            {
                ErrorNotification(_localizationService.GetResource("Admin.Configuration.Languages.PublishedLanguageRequired"));
                return RedirectToAction("Edit", new { id = language.Id });
            }

            _languageService.DeleteLanguage(language);
            //活动日志
            _customerActivityService.InsertActivity("DeleteLanguage", _localizationService.GetResource("ActivityLog.DeleteLanguage"), language.Id);

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Languages.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual JsonResult GetAvailableFlagFileNames()
        {
            var flagNames = Directory
                .EnumerateFiles(CommonHelper.MapPath("~/Content/Images/flags/"), "*.png", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileName)
                .ToList();

            var availableFlagFileNames = flagNames.Select(flagName => new SelectListItem
            {
                Text = flagName,
                Value = flagName
            }).ToList();

            return Json(availableFlagFileNames);
        }

        #endregion

        #region Resources

        [HttpPost]
        [AdminAntiForgery(true)]
        public virtual ActionResult Resources(long languageId, DataSourceRequest command,
            LanguageResourcesListModel model)
        {

            var query = _localizationService
                .GetAllResourceValues(languageId)
                .OrderBy(x => x.Key)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.SearchResourceName))
                query = query.Where(l => l.Key.ToLowerInvariant().Contains(model.SearchResourceName.ToLowerInvariant()));
            if (!string.IsNullOrEmpty(model.SearchResourceValue))
                query = query.Where(l => l.Value.Value.ToLowerInvariant().Contains(model.SearchResourceValue.ToLowerInvariant()));

            var resources = query
                .Select(x => new LanguageResourceModel
                {
                    LanguageId = languageId,
                    Id = x.Value.Key,
                    Name = x.Key,
                    Value = x.Value.Value,
                });

            var gridModel = new DataSourceResult
            {
                Data = resources.PagedForCommand(command),
                Total = resources.Count()
            };

            return Json(gridModel);
        }

        [HttpPost]
        public virtual ActionResult ResourceUpdate(LanguageResourceModel model)
        {
            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }

            var resource = _localizationService.GetLocaleStringResourceById(model.Id);
            //如果资源名改变,需要确保它不会影响到其它资源
            if (!resource.ResourceName.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                var res = _localizationService.GetLocaleStringResourceByName(model.Name, model.LanguageId, false);
                if (res != null && res.Id != resource.Id)
                {
                    return Json(new DataSourceResult { Errors = string.Format(_localizationService.GetResource("Admin.Configuration.Languages.Resources.NameAlreadyExists"), res.ResourceName) });
                }
            }

            resource.ResourceName = model.Name;
            resource.ResourceValue = model.Value;
            _localizationService.UpdateLocaleStringResource(resource);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual ActionResult ResourceAdd(long languageId, [Bind(Exclude = "Id")] LanguageResourceModel model)
        {
            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }

            var res = _localizationService.GetLocaleStringResourceByName(model.Name, model.LanguageId, false);
            if (res == null)
            {
                var resource = new LocaleStringResource { LanguageId = languageId };
                resource.ResourceName = model.Name;
                resource.ResourceValue = model.Value;
                _localizationService.InsertLocaleStringResource(resource);
            }
            else
            {
                return Json(new DataSourceResult { Errors = string.Format(_localizationService.GetResource("Admin.Configuration.Languages.Resources.NameAlreadyExists"), model.Name) });
            }

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual ActionResult ResourceDelete(long id)
        {
            var resource = _localizationService.GetLocaleStringResourceById(id);
            if (resource == null)
                throw new ArgumentException("未找到具有指定ID的资源");

            _localizationService.DeleteLocaleStringResource(resource);

            return new NullJsonResult();
        }

        #endregion

        #region Export / Import

        public virtual ActionResult ExportXml(long id)
        {
            var language = _languageService.GetLanguageById(id);
            if (language == null)
                return RedirectToAction("List");

            try
            {
                var xml = _localizationService.ExportResourcesToXml(language);
                return new XmlDownloadResult(xml, "language_pack.xml");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public virtual ActionResult ImportXml(int id, FormCollection form)
        {
            var language = _languageService.GetLanguageById(id);
            if (language == null)
                return RedirectToAction("List");

            this.Server.ScriptTimeout = 300;

            try
            {
                var file = Request.Files["importxmlfile"];
                if (file != null && file.ContentLength > 0)
                {
                    using (var sr = new StreamReader(file.InputStream, Encoding.UTF8))
                    {
                        string content = sr.ReadToEnd();
                        _localizationService.ImportResourcesFromXml(language, content);
                    }

                }
                else
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Common.UploadFile"));
                    return RedirectToAction("Edit", new { id = language.Id });
                }

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Languages.Imported"));
                return RedirectToAction("Edit", new { id = language.Id });
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = language.Id });
            }
        }

        #endregion

    }
}