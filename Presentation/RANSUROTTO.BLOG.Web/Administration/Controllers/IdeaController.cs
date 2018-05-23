using System;
using System.Linq;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Extensions;
using RANSUROTTO.BLOG.Admin.Models.Interesting;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Services.Helpers;
using RANSUROTTO.BLOG.Services.Interesting;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class IdeaController : BaseAdminController
    {

        #region Fields

        private readonly IIdeaService _ideaService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;

        #endregion

        #region Constructor

        public IdeaController(IIdeaService ideaService, IDateTimeHelper dateTimeHelper, ILocalizationService localizationService, ICustomerActivityService customerActivityService)
        {
            _ideaService = ideaService;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
            _customerActivityService = customerActivityService;
        }

        #endregion

        #region List

        public virtual ActionResult Index()
        {
            return RedirectToAction("Index");
        }

        public virtual ActionResult List()
        {
            var model = new IdeaListModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult List(DataSourceRequest command, IdeaListModel model)
        {
            DateTime? createOnFromValue = (model.CreatedOnFrom == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnFrom.Value, _dateTimeHelper.CurrentTimeZone);
            DateTime? createOnToValue = (model.CreatedOnTo == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnTo.Value, _dateTimeHelper.CurrentTimeZone);

            var ideas = _ideaService.GetAllIdeas(createOnFromValue, createOnToValue, command.Page - 1,
                command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = ideas.Select(idea => new IdeaModel
                {
                    Id = idea.Id,
                    Guid = idea.Guid,
                    Private = idea.Private,
                    Deleted = idea.Deleted,
                    Body = idea.Body,
                    CustomerEmail = idea.Customer != null ? idea.Customer.Email : null,
                    CreatedOn = _dateTimeHelper.ConvertToUserTime(idea.CreatedOnUtc, DateTimeKind.Utc),
                    UpdatedOn = idea.UpdatedOnUtc.HasValue
                        ? (DateTime?)_dateTimeHelper.ConvertToUserTime(idea.UpdatedOnUtc.Value, DateTimeKind.Utc)
                        : null
                }),
                Total = ideas.TotalCount
            };
            return Json(gridModel);
        }

        #endregion

        #region View / Delete

        public virtual ActionResult View(int id)
        {
            var idea = _ideaService.GetIdeaById(id);
            if (idea == null)
                return RedirectToAction("List");

            var model = idea.ToModel();
            model.CustomerEmail = idea.Customer != null ? idea.Customer.Email : null;
            model.CreatedOn = _dateTimeHelper.ConvertToUserTime(idea.CreatedOnUtc, DateTimeKind.Utc);
            model.UpdatedOn = idea.UpdatedOnUtc.HasValue
                ? (DateTime?)_dateTimeHelper.ConvertToUserTime(idea.CreatedOnUtc, DateTimeKind.Utc)
                : null;

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            var idea = _ideaService.GetIdeaById(id);
            if (idea == null)
                return RedirectToAction("List");

            _ideaService.DeleteIdea(idea);

            //activity log
            _customerActivityService.InsertActivity("DeleteIdea", _localizationService.GetResource("ActivityLog.DeleteIdea"), idea.Id);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Idea.Deleted"));

            return View();
        }

        #endregion

    }
}