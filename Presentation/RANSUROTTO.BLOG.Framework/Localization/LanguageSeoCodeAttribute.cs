using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Localization.Setting;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Service.Localization;

namespace RANSUROTTO.BLOG.Framework.Localization
{
    /// <summary>
    /// 该特性确保URL包含一个语言搜索引擎优化代码
    /// 如果LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled开启的话
    /// </summary>
    public class LanguageSeoCodeAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext?.HttpContext?.Request;
            if (request == null)
                return;

            if (filterContext.IsChildAction)
                return;

            if (!string.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            var localizationSettings = EngineContext.Current.Resolve<LocalizationSettings>();
            if (!localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                return;

            //确保路由对象为LocalizedRoute
            if (!(filterContext.RouteData?.Route is LocalizedRoute))
                return;

            //处理当前的URL
            var pageUrl = request.RawUrl;
            string applicationPath = request.ApplicationPath;

            if (pageUrl.IsLocalizedUrl(applicationPath, true))
            {
                var seoCode = pageUrl.GetLanguageSeoCodeFromUrl(applicationPath, true);
                //确保当前Url使用语言代码有效
                var languageService = EngineContext.Current.Resolve<ILanguageService>();
                var language = languageService.GetAllLanguages()
                    .FirstOrDefault(l => seoCode.Equals(l.UniqueSeoCode, StringComparison.InvariantCultureIgnoreCase));
                if (language != null && language.Published)
                {
                    return;
                }
                else
                {
                    pageUrl = pageUrl.RemoveLanguageSeoCodeFromRawUrl(applicationPath);
                    filterContext.Result = new RedirectResult(pageUrl);
                }
            }
            //添加语言代码到Url
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            pageUrl = pageUrl.AddLanguageSeoCodeToRawUrl(applicationPath, workContext.WorkingLanguage);

            filterContext.Result = new RedirectResult(pageUrl, true);
        }


    }
}
