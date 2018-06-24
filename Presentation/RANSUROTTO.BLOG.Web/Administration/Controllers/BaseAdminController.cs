using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json.Converters;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain.Common.Setting;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Framework.Controllers;
using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Framework.Security;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    [HttpsRequirement(SslRequirement.NoMatter)]
    [AdminValidateIpAddress]
    [AdminAntiForgery]
    [AdminAuthorize]
    public abstract class BaseAdminController : BaseController
    {

        #region Initialize

        /// <summary>
        /// 初始化控制器
        /// </summary>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //将工作环境设置为管理模式
            EngineContext.Current.Resolve<IWorkContext>().IsAdmin = true;

            base.Initialize(requestContext);
        }

        #endregion

        #region On exception

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
                //记录日志
                LogException(filterContext.Exception);
            base.OnException(filterContext);
        }

        #endregion

        #region Access denied

        /// <summary>
        /// 返回跳转至"拒绝访问页"的ActionResult
        /// </summary>
        protected virtual ActionResult AccessDeniedView()
        {
            return RedirectToAction("AccessDenied", "Security", new { pageUrl = this.Request.RawUrl });
        }

        /// <summary>
        /// 返回给 KendoUi DataGrid 发生错误,消息为"拒绝访问"的JSON数据
        /// </summary>
        protected JsonResult AccessDeniedKendoGridJson()
        {
            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

            return ErrorForKendoGridJson(localizationService.GetResource("Admin.AccessDenied.Description"));
        }

        #endregion

        #region Save select tab name

        /// <summary>
        /// 保存选中选项卡的名称
        /// </summary>
        /// <param name="tabName">要保存的选项卡名称</param>
        /// <param name="persistForTheNextRequest">标识该通知是否为跨请求保留</param>
        protected virtual void SaveSelectedTabName(string tabName = "", bool persistForTheNextRequest = true)
        {
            //"GetSelectedTabName" method of \RANSUROTTO.BLOG.Framework\HtmlExtensions.cs
            if (string.IsNullOrEmpty(tabName))
            {
                tabName = this.Request.Form["selected-tab-name"];
            }

            if (!string.IsNullOrEmpty(tabName))
            {
                const string dataKey = "ransurotto.selected-tab-name";
                if (persistForTheNextRequest)
                {
                    TempData[dataKey] = tabName;
                }
                else
                {
                    ViewData[dataKey] = tabName;
                }
            }
        }

        #endregion

        #region JSON

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            //修复KendoUi DataGrid中日期问题
            //使用IsoDateTimeConverter来配合处理JSON
            var result = EngineContext.Current.Resolve<AdminAreaSettings>().UseIsoDateTimeConverterInJson
                ? new ConverterJsonResult(new IsoDateTimeConverter()) : new JsonResult();

            result.Data = data;
            result.ContentType = contentType;
            result.ContentEncoding = contentEncoding;
            result.JsonRequestBehavior = behavior;

            result.MaxJsonLength = int.MaxValue;

            return result;
        }

        #endregion

    }
}