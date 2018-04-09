using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Service.Helpers;
using RANSUROTTO.BLOG.Service.Localization;
using RANSUROTTO.BLOG.Service.Logging;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class LogController : BaseAdminController
    {

        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;

        public LogController(ILogger logger, IWorkContext workContext, ILocalizationService localizationService, IDateTimeHelper dateTimeHelper)
        {
            _logger = logger;
            _workContext = workContext;
            _localizationService = localizationService;
            _dateTimeHelper = dateTimeHelper;
        }

        #region Index

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
            var gridModel = new DataSourceResult
            {
                Data = null,
            };
            return Json(gridModel);
        }

        #endregion






    }
}