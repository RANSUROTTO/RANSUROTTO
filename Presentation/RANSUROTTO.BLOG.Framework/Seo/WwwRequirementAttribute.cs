using System;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Seo.Enum;
using RANSUROTTO.BLOG.Core.Domain.Seo.Setting;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;

namespace RANSUROTTO.BLOG.Framework.Seo
{
    /// <summary>
    /// 确保请求页面Url经过特定的www前缀设定处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class WwwRequirementAttribute : FilterAttribute, IAuthorizationFilter
    {
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException(nameof(filterContext));

            if (filterContext.IsChildAction)
                return;

            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            if (filterContext.HttpContext.Request.IsLocal)
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            var seoSettings = EngineContext.Current.Resolve<SeoSettings>();

            switch (seoSettings.WwwRequirement)
            {
                case WwwRequirement.WithWww:
                    {
                        var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                        string url = webHelper.GetThisPageUrl(true);
                        var currentConnectionSecured = webHelper.IsCurrentConnectionSecured();
                        if (currentConnectionSecured)
                        {
                            bool startsWith3W = url.StartsWith("https://www.", StringComparison.OrdinalIgnoreCase);
                            if (!startsWith3W)
                            {
                                url = url.Replace("https://", "https://www.");

                                //301 (permanent) redirection
                                filterContext.Result = new RedirectResult(url, true);
                            }
                        }
                        else
                        {
                            bool startsWith3W = url.StartsWith("http://www.", StringComparison.OrdinalIgnoreCase);
                            if (!startsWith3W)
                            {
                                url = url.Replace("http://", "http://www.");

                                //301 (permanent) redirection
                                filterContext.Result = new RedirectResult(url, true);
                            }
                        }
                    }
                    break;
                case WwwRequirement.WithoutWww:
                    {
                        var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                        string url = webHelper.GetThisPageUrl(true);
                        var currentConnectionSecured = webHelper.IsCurrentConnectionSecured();
                        if (currentConnectionSecured)
                        {
                            bool startsWith3W = url.StartsWith("https://www.", StringComparison.OrdinalIgnoreCase);
                            if (startsWith3W)
                            {
                                url = url.Replace("https://www.", "https://");

                                //301 (permanent) redirection
                                filterContext.Result = new RedirectResult(url, true);
                            }
                        }
                        else
                        {
                            bool startsWith3W = url.StartsWith("http://www.", StringComparison.OrdinalIgnoreCase);
                            if (startsWith3W)
                            {
                                url = url.Replace("http://www.", "http://");

                                //301 (permanent) redirection
                                filterContext.Result = new RedirectResult(url, true);
                            }
                        }
                    }
                    break;
                case WwwRequirement.NoMatter:
                    break;
                default:
                    throw new SiteException("不支持的WwwRequirement参数");
            }

        }
    }
}
