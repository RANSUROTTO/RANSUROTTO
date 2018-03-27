using System.IO;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Service.Localization;

namespace RANSUROTTO.BLOG.Framework.ViewEngines.Razor
{

    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {

        private ILocalizationService _localizationService;
        private Localizer _localizer;

        public Localizer T
        {
            get
            {
                return _localizer ?? (_localizer = (resourceKey, args) =>
                {
                    var localizationText = _localizationService.GetResource(resourceKey);

                    if (localizationText == null)
                        return new LocalizedString(resourceKey);

                    return new LocalizedString((args == null || args.Length == 0)
                        ? localizationText
                        : string.Format(localizationText, args));
                });
            }
        }

        public override void InitHelpers()
        {
            base.InitHelpers();

            if (DataSettingsHelper.DatabaseIsInstalled())
            {
                _localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            }
        }

        /// <summary>
        /// 不需要精确的给出布局页的路径
        /// 你只需要给出正确的布局页的文件名
        /// 代码会从当前的ControllerContext匹配到匹配度最高的布局页对象并且重置路径
        /// </summary>
        public override string Layout
        {
            get
            {
                var layout = base.Layout;

                if (!string.IsNullOrEmpty(layout))
                {
                    var filename = Path.GetFileNameWithoutExtension(layout);
                    ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindView(ViewContext.Controller.ControllerContext, filename, "");

                    if (viewResult.View is RazorView)
                    {
                        layout = ((RazorView)viewResult.View).ViewPath;
                    }
                }

                return layout;
            }
            set
            {
                base.Layout = value;
            }
        }

    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }

}
