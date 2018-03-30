using System.Web.Routing;

namespace RANSUROTTO.BLOG.Framework.Mvc.Routes
{

    /// <summary>
    /// 路由提供者
    /// </summary>
    public interface IRouteProvider
    {

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes">路由集合</param>
        void RegisterRoutes(RouteCollection routes);

        /// <summary>
        /// 优先级别
        /// </summary>
        int Priority { get; }

    }
}
