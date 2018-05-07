using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Extensions;
using RANSUROTTO.BLOG.Admin.Models.Common;
using RANSUROTTO.BLOG.Core;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Framework.Controllers;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Service.Common;
using RANSUROTTO.BLOG.Service.Helpers;
using RANSUROTTO.BLOG.Service.Localization;
using RANSUROTTO.BLOG.Service.Security;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class CommonController : BaseAdminController
    {

        #region Fields

        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IMaintenanceService _maintenanceService;

        #endregion

        #region Constructor

        public CommonController(IDateTimeHelper dateTimeHelper, HttpContextBase httpContext, IWorkContext workContext, IWebHelper webHelper, ILocalizationService localizationService, ILanguageService languageService, IMaintenanceService maintenanceService)
        {
            _dateTimeHelper = dateTimeHelper;
            _httpContext = httpContext;
            _workContext = workContext;
            _webHelper = webHelper;
            _localizationService = localizationService;
            _languageService = languageService;
            _maintenanceService = maintenanceService;
        }

        #endregion

        #region Methods

        public virtual ActionResult SystemInfo()
        {
            var model = new SystemInfoModel();
            model.RansurottoVersion = RansurottoVersion.CurrentVersion;

            try
            {
                model.OperatingSystem = Environment.OSVersion.VersionString;
            }
            catch
            {
                //Ignore
            }
            try
            {
                model.AspNetInfo = RuntimeEnvironment.GetSystemVersion();
            }
            catch
            {
                //Ignore
            }
            try
            {
                model.IsFullTrust = AppDomain.CurrentDomain.IsFullyTrusted.ToString();
            }
            catch
            {
                //Ignore
            }
            model.ServerTimeZone = TimeZone.CurrentTimeZone.StandardName;
            model.ServerLocalTime = DateTime.Now;
            model.UtcTime = DateTime.UtcNow;
            model.CurrentUserTime = _dateTimeHelper.ConvertToUserTime(DateTime.Now);
            model.HttpHost = _webHelper.ServerVariables("HTTP_HOST");
            foreach (var key in _httpContext.Request.ServerVariables.AllKeys)
            {
                if (key.StartsWith("ALL_")) continue;

                model.ServerVariables.Add(new SystemInfoModel.ServerVariableModel
                {
                    Name = key,
                    Value = _httpContext.Request.ServerVariables[key]
                });
            }
            var trustLevel = CommonHelper.GetTrustLevel();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var loadedAssembly = new SystemInfoModel.LoadedAssemblyModel
                {
                    FullName = assembly.FullName,

                };
                //ensure no exception is thrown
                try
                {
                    var canGetLocation = trustLevel >= AspNetHostingPermissionLevel.High && !assembly.IsDynamic;
                    loadedAssembly.Location = canGetLocation ? assembly.Location : null;
                    loadedAssembly.IsDebug = IsDebugAssembly(assembly);
                    loadedAssembly.BuildDate =
                        canGetLocation ? (DateTime?)GetBuildDate(assembly, TimeZoneInfo.Local) : null;
                }
                catch (Exception)
                {
                    //Ignore
                }
                model.LoadedAssemblies.Add(loadedAssembly);
            }

            return View(model);
        }

        public virtual ActionResult LanguageSelector()
        {
            var model = new LanguageSelectorModel();
            model.CurrentLanguage = _workContext.WorkingLanguage.ToModel();
            model.AvailableLanguages = _languageService
                .GetAllLanguages()
                .Select(l => l.ToModel())
                .ToList();

            return PartialView(model);
        }

        public virtual ActionResult SetLanguage(long langid, string returnUrl = "")
        {
            var language = _languageService.GetLanguageById(langid);
            if (language != null)
            {
                _workContext.WorkingLanguage = language;
            }

            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.Action("Index", "Home", new { area = "Admin" });
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new { area = "Admin" });

            return Redirect(returnUrl);
        }

        public virtual ActionResult Warnings()
        {
            return View();
        }

        public virtual ActionResult Maintenance()
        {
            var model = new MaintenanceModel();
            model.DeleteGuests = new MaintenanceModel.DeleteGuestsModel
            {
                OnlyWithoutAction = true,
                EndDate = DateTime.Now.AddDays(-1)
            };
            model.DeleteExportedFiles = new MaintenanceModel.DeleteExportedFilesModel
            {
                EndDate = DateTime.Now.AddDays(-182)
            };
            return View(model);
        }

        [HttpPost, ActionName("Maintenance")]
        [FormValueRequired("delete-guests")]
        public virtual ActionResult MaintenanceDeleteGuests(MaintenanceModel model)
        {
            DateTime? startDateValue = (model.DeleteGuests.StartDate == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.DeleteGuests.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.DeleteGuests.EndDate == null) ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.DeleteGuests.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            //model.DeleteGuests.NumberOfDeletedCustomers = _customerService.DeleteGuestCustomers(startDateValue, endDateValue, model.DeleteGuests.OnlyWithoutShoppingCart);

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult BackupFiles(DataSourceRequest command)
        {
            var backupFiles = _maintenanceService.GetAllBackupFiles().ToList();

            var gridModel = new DataSourceResult
            {
                Data = backupFiles.Select(p => new
                {
                    p.Name,
                    Length = string.Format("{0:F2} Mb", p.Length / 1024f / 1024f),
                    Link = _webHelper.GetLocation(false) + "Administration/db_backups/" + p.Name
                }),
                Total = backupFiles.Count
            };
            return Json(gridModel);
        }

        [HttpPost, ActionName("Maintenance")]
        [FormValueRequired("backup-database")]
        public virtual ActionResult BackupDatabase(MaintenanceModel model)
        {
            try
            {
                _maintenanceService.BackupDatabase();
                this.SuccessNotification(_localizationService.GetResource("Admin.System.Maintenance.BackupDatabase.BackupCreated"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }

            return View(model);
        }

        [HttpPost, ActionName("Maintenance")]
        [FormValueRequired("backupFileName", "action")]
        public virtual ActionResult BackupAction(MaintenanceModel model)
        {
            var action = this.Request.Form["action"];

            var fileName = this.Request.Form["backupFileName"];
            var backupPath = _maintenanceService.GetBackupPath(fileName);

            try
            {
                switch (action)
                {
                    case "delete-backup":
                        {
                            System.IO.File.Delete(backupPath);
                            this.SuccessNotification(string.Format(_localizationService.GetResource("Admin.System.Maintenance.BackupDatabase.BackupDeleted"), fileName));
                        }
                        break;
                    case "restore-backup":
                        {
                            _maintenanceService.RestoreDatabase(backupPath);
                            this.SuccessNotification(_localizationService.GetResource("Admin.System.Maintenance.BackupDatabase.DatabaseRestored"));
                        }
                        break;
                }
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }

            return View(model);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 判断程序集是否为Debug
        /// </summary>
        /// <param name="assembly">程序集实例</param>
        /// <returns>结果</returns>
        private bool IsDebugAssembly(Assembly assembly)
        {
            var attribs = assembly.GetCustomAttributes(typeof(System.Diagnostics.DebuggableAttribute), false);

            if (attribs.Length > 0)
            {
                var attr = attribs[0] as System.Diagnostics.DebuggableAttribute;
                if (attr != null)
                {
                    return attr.IsJITOptimizerDisabled;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取程序集最后修改时间
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="target">指定返回时间的时区;如不指定则使用服务器时区</param>
        /// <returns>结果</returns>
        private DateTime GetBuildDate(Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;

            const int cPeHeaderOffset = 60;
            const int cLinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                stream.Read(buffer, 0, 2048);
            }

            var offset = BitConverter.ToInt32(buffer, cPeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + cLinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }

        #endregion

    }
}