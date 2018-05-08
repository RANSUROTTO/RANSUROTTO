using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Service.Helpers;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Framework.Extensions;
using RANSUROTTO.BLOG.Admin.Models.Logging;
using RANSUROTTO.BLOG.Framework.Controllers;
using RANSUROTTO.BLOG.Core.Domain.Logging.Enum;
using RANSUROTTO.BLOG.Services.Helpers;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class LogController : BaseAdminController
    {

        #region Fields

        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        #region Constructor

        public LogController(ILogger logger, IWorkContext workContext, ILocalizationService localizationService, IDateTimeHelper dateTimeHelper)
        {
            _logger = logger;
            _workContext = workContext;
            _localizationService = localizationService;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region List

        public virtual ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual ActionResult List()
        {
            var model = new LogListModel();
            model.AvailableLogLevels = LogLevel.Debug.ToSelectList(false).ToList();
            model.AvailableLogLevels.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult List(DataSourceRequest command, LogListModel model)
        {
            DateTime? createdOnFromValue = (model.CreatedOnFrom == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnFrom.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? createdToFromValue = (model.CreatedOnFrom == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnFrom.Value, _dateTimeHelper.CurrentTimeZone);

            LogLevel? logLevel = model.LogLevelId > 0 ? (LogLevel?)(model.LogLevelId) : null;

            var logItems = _logger.GetAllLogs(createdOnFromValue, createdToFromValue, model.Message,
                logLevel, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = logItems.Select(x => new LogModel
                {
                    Id = x.Id,
                    LogLevel = x.LogLevel.GetLocalizedEnum(_localizationService, _workContext),
                    ShortMessage = x.ShortMessage,
                    FullMessage = "",
                    IpAddress = x.IpAddress,
                    CustomerId = x.CustomerId,
                    CustomerEmail = x.Customer != null ? x.Customer.Email : null,
                    PageUrl = x.PageUrl,
                    ReferrerUrl = x.ReferrerUrl,
                    CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc)
                }),
                Total = logItems.TotalCount
            };
            return Json(gridModel);
        }

        #endregion

        #region View

        public virtual ActionResult View(long id)
        {
            var log = _logger.GetLogById(id);
            if (log == null)
                return RedirectToAction("List");

            var model = new LogModel
            {
                Id = log.Id,
                LogLevel = log.LogLevel.GetLocalizedEnum(_localizationService, _workContext),
                ShortMessage = log.ShortMessage,
                FullMessage = log.FullMessage,
                IpAddress = log.IpAddress,
                CustomerId = log.CustomerId,
                CustomerEmail = log.Customer != null ? log.Customer.Email : null,
                PageUrl = log.PageUrl,
                ReferrerUrl = log.ReferrerUrl,
                CreatedOn = _dateTimeHelper.ConvertToUserTime(log.CreatedOnUtc, DateTimeKind.Utc)
            };

            return View(model);
        }

        #endregion

        #region Delete / Clear All

        [HttpPost]
        public virtual ActionResult Delete(long id)
        {
            var log = _logger.GetLogById(id);
            if (log == null)
                return RedirectToAction("List");

            _logger.DeleteLog(log);

            SuccessNotification(_localizationService.GetResource("Admin.System.Log.Deleted"));
            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual ActionResult DeleteSelected(ICollection<long> selectedIds)
        {
            if (selectedIds != null)
            {
                _logger.DeleteLogs(_logger.GetLogByIds(selectedIds.ToArray()).ToList());
            }

            SuccessNotification(_localizationService.GetResource("Admin.System.Log.DeletedSelected"));
            return Json(new { Result = true });
        }

        [HttpPost, ActionName("List")]
        [FormValueRequired("clearall")]
        public virtual ActionResult ClearAll()
        {
            _logger.ClearLog();

            SuccessNotification(_localizationService.GetResource("Admin.System.Log.Cleared"));
            return RedirectToAction("List");
        }

        #endregion

    }
}