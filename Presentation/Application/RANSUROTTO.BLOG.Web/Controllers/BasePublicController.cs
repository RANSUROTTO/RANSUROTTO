using System.Web.Mvc;
using System.Web.Routing;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Framework.Controllers;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Security;
using RANSUROTTO.BLOG.Framework.Seo;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    [WwwRequirement]
    [LanguageSeoCode]
    [HttpsRequirement(SslRequirement.NoMatter)]
    public abstract class BasePublicController : BaseController
    {
        protected virtual ActionResult InvokeHttp404()
        {
            IController errorController = EngineContext.Current.Resolve<CommonController>();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");
            routeData.Values.Add("action", "PageNotFound");

            errorController.Execute(new RequestContext(this.HttpContext, routeData));

            return new EmptyResult();
        }
    }
}