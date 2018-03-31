using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Common.Setting;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Framework.Mvc.Routes;
using RANSUROTTO.BLOG.Service.Logging;
using RANSUROTTO.BLOG.Service.Tasks;

namespace RANSUROTTO.BLOG.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {

        /// <summary>
        /// 应用程序启动事件
        /// </summary>
        protected void Application_Start()
        {
            //目前大多数API提供商都需要TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //禁用 "X-AspNetMvc-Version" 请求头
            MvcHandler.DisableMvcResponseHeader = true;

            //初始化核心上下文
            EngineContext.Initialize(false);

            bool databaseInstalled = DataSettingsHelper.DatabaseIsInstalled();
            if (databaseInstalled)
            {
                //TODO ViewEngines
            }

            //在默认ModelMetadataProvider上添加新的功能
            ModelMetadataProviders.Current = new BaseMetadataProvider();

            //注册所有路由
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
            if (databaseInstalled)
            {
                //启动计划任务
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();

                //TODO miniprofiler

                try
                {
                    //记录应用程序启动日志
                    var logger = EngineContext.Current.Resolve<ILogger>();
                    logger.Information("Application started", null, null);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// 路由注册
        /// </summary>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var routePublisher = EngineContext.Current.Resolve<IRoutePublisher>();
            routePublisher.RegisterRoutes(routes);

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "RANSUROTTO.BLOG.Web.Controllers" });
        }

        /// <summary>
        /// 程序全局异常事件
        /// </summary>
        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            //log error
            LogException(exception);

            //process 404 HTTP errors
            //TODO 未完成
            /*var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                if (!webHelper.IsStaticResource(this.Request))
                {
                    Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;

                    // Call target Controller and pass the routeData.
                    IController errorController = EngineContext.Current.Resolve<CommonController>();

                    var routeData = new RouteData();
                    routeData.Values.Add("controller", "Common");
                    routeData.Values.Add("action", "PageNotFound");

                    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }*/
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="exc">异常</param>
        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            var httpException = exc as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404 &&
                !EngineContext.Current.Resolve<CommonSettings>().Log404Errors)
                return;

            try
            {
                var logger = EngineContext.Current.Resolve<ILogger>();
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                logger.Error(exc.Message, exc, workContext.CurrentCustomer);
            }
            catch (Exception)
            {
                // ignored
            }
        }

    }
}
