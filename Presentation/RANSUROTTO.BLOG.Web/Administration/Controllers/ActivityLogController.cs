using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Admin.Extensions;
using RANSUROTTO.BLOG.Admin.Models.Logging;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Services.Helpers;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class ActivityLogController : BaseAdminController
    {

        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Constructor

        public ActivityLogController(ICustomerActivityService customerActivityService, IDateTimeHelper dateTimeHelper, ILocalizationService localizationService)
        {
            _customerActivityService = customerActivityService;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
        }

        #endregion

        #region Activity logs

        public virtual ActionResult ListLogs()
        {
            var model = new ActivityLogListModel();

            model.ActivityLogType.Add(new SelectListItem
            {
                Value = "0",
                Text = "All"
            });

            foreach (var at in _customerActivityService.GetAllActivityTypes())
            {
                model.ActivityLogType.Add(new SelectListItem
                {
                    Value = at.Id.ToString(),
                    Text = at.Name
                });
            }

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult ListLogs(DataSourceRequest command, ActivityLogListModel model)
        {

            DateTime? startDateValue = (model.CreatedOnFrom == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnFrom.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.CreatedOnTo == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnTo.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            var activityLog = _customerActivityService.GetAllActivities(startDateValue, endDateValue, null, model.ActivityLogTypeId, command.Page - 1, command.PageSize, model.IpAddress);
            var gridModel = new DataSourceResult
            {
                Data = activityLog.Select(x =>
                {
                    var m = x.ToModel();
                    m.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                    return m;
                }),
                Total = activityLog.TotalCount
            };

            return Json(gridModel);
        }

        public virtual ActionResult AcivityLogDelete(int id)
        {
            var activityLog = _customerActivityService.GetActivityById(id);
            if (activityLog == null)
            {
                throw new ArgumentException("未找到具有指定ID的活动日志");
            }
            _customerActivityService.DeleteActivity(activityLog);

            _customerActivityService.InsertActivity("DeleteActivityLog", _localizationService.GetResource("ActivityLog.DeleteActivityLog"));

            return new NullJsonResult();
        }

        public virtual ActionResult ClearAll()
        {
            _customerActivityService.ClearAllActivities();

            _customerActivityService.InsertActivity("DeleteActivityLog", _localizationService.GetResource("ActivityLog.DeleteActivityLog"));

            return RedirectToAction("ListLogs");
        }

        #endregion

        #region Activity log types

        public virtual ActionResult ListTypes()
        {
            var model = _customerActivityService
                .GetAllActivityTypes()
                .Select(x => x.ToModel())
                .ToList();

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult SaveTypes(FormCollection form)
        {
            _customerActivityService.InsertActivity("EditActivityLogTypes", _localizationService.GetResource("ActivityLog.EditActivityLogTypes"));

            string formKey = "checkbox_activity_types";
            var checkedActivityTypes = form[formKey] != null ? form[formKey].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList() : new List<long>();

            var activityTypes = _customerActivityService.GetAllActivityTypes();
            foreach (var activityType in activityTypes)
            {
                activityType.Enabled = checkedActivityTypes.Contains(activityType.Id);
                _customerActivityService.UpdateActivityType(activityType);
            }

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.ActivityLog.ActivityLogType.Updated"));
            return RedirectToAction("ListTypes");
        }

        #endregion

    }
}