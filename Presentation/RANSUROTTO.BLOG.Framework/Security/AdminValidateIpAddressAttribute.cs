using System;
using System.Linq;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Domain.Security.Setting;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;

namespace RANSUROTTO.BLOG.Framework.Security
{

    /// <summary>
    /// 该特性用于确保请求方的IP地址是为已配置可登陆管理员区域IP列表之一
    /// </summary>
    public class AdminValidateIpAddressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext?.HttpContext == null)
                return;

            if (filterContext.IsChildAction)
                return;

            bool ok = false;
            var ipAddresses = EngineContext.Current.Resolve<SecuritySettings>().AdminAreaAllowedIpAddresses;
            if (ipAddresses != null && ipAddresses.Any())
            {
                var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                foreach (string ip in ipAddresses)
                    if (ip.Equals(webHelper.GetCurrentIpAddress(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        ok = true;
                        break;
                    }
            }
            else
            {
                //没有限制IP进入
                ok = true;
            }

            if (!ok)
            {
                //TODO 拒绝访问页面路径需要配置清晰
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                var accessDeniedPageUrl = urlHelper.Action("AccessDenied", "Security");

                if (accessDeniedPageUrl.StartsWith("/"))
                    accessDeniedPageUrl = accessDeniedPageUrl.Remove(0, 1);

                var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                var thisPageUrl = webHelper.GetThisPageUrl(false);
                var locationUrl = webHelper.GetLocation();

                if (!thisPageUrl.StartsWith($"{locationUrl}{accessDeniedPageUrl}", StringComparison.OrdinalIgnoreCase))
                {
                    //跳转至"拒绝访问"页面
                    filterContext.Result =
                        new RedirectResult(urlHelper.Action("AccessDenied", "Security"));
                }
            }
        }

    }
}
