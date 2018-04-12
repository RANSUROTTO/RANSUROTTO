using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Domain.Common.Setting;
using RANSUROTTO.BLOG.Web.Factories;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    public class CommonController : BasePublicController
    {

        #region Fields

        private readonly CommonSettings _commonSettings;
        private readonly ICommonModelFactory _commonModelFactory;

        #endregion

        #region Constructor

        public CommonController(CommonSettings commonSettings, ICommonModelFactory commonModelFactory)
        {
            _commonSettings = commonSettings;
            this._commonModelFactory = commonModelFactory;
        }

        #endregion

        #region Methods

        [ChildActionOnly]
        public virtual ActionResult Favicon()
        {
            var model = _commonModelFactory.PrepareFaviconModel();
            return PartialView(model);
        }

        [ChildActionOnly]
        public virtual ActionResult JavaScriptDisabledWarning()
        {
            if (!_commonSettings.DisplayJavaScriptDisabledWarning)
                return Content("");

            return PartialView();
        }

        [ChildActionOnly]
        public virtual ActionResult AdminHeaderLinks()
        {
            var model = _commonModelFactory.PrepareAdminHeaderLinksModel();
            return PartialView(model);
        }

        #endregion

    }
}