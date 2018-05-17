using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Extensions;
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