using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Web.Infrastructure.Installation;
using RANSUROTTO.BLOG.Web.Models.Install;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    public class InstallController : BasePublicController
    {

        #region Fields

        private readonly IInstallationLocalizationService _locService;

        #endregion

        #region Constructor

        public InstallController(IInstallationLocalizationService locService)
        {
            _locService = locService;
        }

        #endregion

        #region Methods

        public virtual ActionResult Index()
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            //设置页面超时时间为5分钟
            this.Server.ScriptTimeout = 300;

            var model = new InstallModel
            {
                AdminEmail = "admin@ransurotto.com",
                DataProvider = "mysql",
                MySqlAuthenticationType = "sqlauthentication",
                MySqlConnectionInfo = "sqlconnectioninfo_values"
            };

            foreach (var lang in _locService.GetAvailableLanguages())
            {
                model.AvailableLanguages.Add(new SelectListItem
                {
                    Value = Url.Action("ChangeLanguage", new { language = lang.Code }),
                    Text = lang.Name,
                    Selected = _locService.GetCurrentLanguage().Code == lang.Code,
                });
            }

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Index(InstallModel model)
        {
            return View();
        }

        public virtual ActionResult ChangeLanguage(string language)
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            //Update current language
            _locService.SaveCurrentLanguage(language);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public virtual ActionResult RestartInstall()
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            webHelper.RestartAppDomain();

            return RedirectToRoute("HomePage");
        }

        #endregion

        #region Utilities



        #endregion

    }
}