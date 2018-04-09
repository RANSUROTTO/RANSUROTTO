using System;
using System.IO;
using System.Web.Mvc;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Framework.UI;
using RANSUROTTO.BLOG.Service.Logging;
using RANSUROTTO.BLOG.Framework.Kendoui;
using RANSUROTTO.BLOG.Core.Infrastructure;

namespace RANSUROTTO.BLOG.Framework.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [StoreIpAddress]
    [CustomerLastActivity]
    public abstract class BaseController : Controller
    {

        #region Render partial view to string

        public virtual string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        public virtual string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        public virtual string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        /// <summary>
        /// 读取分部视图转为html字符串
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="model">视图模型</param>
        /// <returns>视图html字符串</returns>
        public virtual string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");

            this.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion

        #region Logger

        /// <summary>
        /// 异常日志记录
        /// </summary>
        /// <param name="exc">异常</param>
        protected void LogException(Exception exc)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var logger = EngineContext.Current.Resolve<ILogger>();

            var customer = workContext.CurrentCustomer;
            logger.Error(exc.Message, exc, customer);
        }


        #endregion

        #region Notification

        /// <summary>
        /// 显示成功通知(绿色标识)
        /// </summary>
        /// <param name="message">消息及信息</param>
        /// <param name="persistForTheNextRequest">标识该通知是否为跨请求保留</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }
        /// <summary>
        /// 显示警告通知(黄色标识)
        /// </summary>
        /// <param name="message">消息及信息</param>
        /// <param name="persistForTheNextRequest">标识该通知是否为跨请求保留</param>
        protected virtual void WarningNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Warning, message, persistForTheNextRequest);
        }

        /// <summary>
        /// 显示错误通知(红色标识)
        /// </summary>
        /// <param name="message">消息及信息</param>
        /// <param name="persistForTheNextRequest">标识该通知是否为跨请求保留</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }
        /// <summary>
        /// 显示错误通知(红色标识)
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="persistForTheNextRequest">标识该通知是否为跨请求保留</param>
        /// <param name="logException">标识是否记录该异常日志</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            if (logException)
                LogException(exception);
            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }

        /// <summary>
        /// 显示通知
        /// </summary>
        /// <param name="type">通知类型</param>
        /// <param name="message">消息及信息</param>
        /// <param name="persistForTheNextRequest">标识该通知是否为跨请求保留</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = $"ransurotto.notifications.{type}";
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }

        #endregion

        #region KendoUI

        /// <summary>
        /// 返回给 KendoUI DataGrid 带错误消息的JSON数据
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>JSON数据</returns>
        protected JsonResult ErrorForKendoGridJson(string errorMessage)
        {
            var gridModel = new DataSourceResult
            {
                Errors = errorMessage
            };

            return Json(gridModel);
        }

        #endregion

        #region Display edit page url

        /// <summary>
        /// 显示"编辑"链接在
        /// </summary>
        /// <param name="editPageUrl">编辑页面Url</param>
        protected virtual void DisplayEditLink(string editPageUrl)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddEditPageUrl(editPageUrl);
        }

        #endregion

    }

}
