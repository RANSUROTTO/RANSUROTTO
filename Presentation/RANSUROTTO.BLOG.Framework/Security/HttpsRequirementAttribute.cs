using System;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Security.Setting;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;

namespace RANSUROTTO.BLOG.Framework.Security
{

    /// <summary>
    /// 确保请求经过了特定的SSL选项处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class HttpsRequirementAttribute : FilterAttribute, IAuthorizationFilter
    {

        public SslRequirement SslRequirement { get; set; }

        public HttpsRequirementAttribute(SslRequirement sslRequirement)
        {
            this.SslRequirement = sslRequirement;
        }

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException(nameof(filterContext));

            if (filterContext.IsChildAction)
                return;

            if (!string.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;
            var securitySettings = EngineContext.Current.Resolve<SecuritySettings>();
            if (securitySettings.ForceSslForAllPages)
                //不管指定的值是多少,所有页面都必须是SSL
                this.SslRequirement = SslRequirement.Yes;

            switch (this.SslRequirement)
            {
                case SslRequirement.Yes:
                    {
                        var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                        var currentConnectionSecured = webHelper.IsCurrentConnectionSecured();
                        if (!currentConnectionSecured)
                        {
                            string url = webHelper.GetThisPageUrl(true, true);
                            filterContext.Result = new RedirectResult(url, true);
                        }
                    }
                    break;
                case SslRequirement.No:
                    {
                        var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                        var currentConnectionSecured = webHelper.IsCurrentConnectionSecured();
                        if (currentConnectionSecured)
                        {
                            string url = webHelper.GetThisPageUrl(true, false);
                            filterContext.Result = new RedirectResult(url, true);
                        }
                    }
                    break;
                case SslRequirement.NoMatter:
                    {
                        //do nothing
                    }
                    break;
                default:
                    throw new SiteException("Not supported SslProtected parameter");
            }
        }

    }

    /// <summary>
    /// SSL是否为必须
    /// </summary>
    public enum SslRequirement
    {
        Yes,
        No,
        NoMatter,
    }

}
