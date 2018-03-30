using System.Web.Routing;

namespace RANSUROTTO.BLOG.Framework.Mvc.Routes
{
    /// <summary>
    /// 路由发布器
    /// </summary>
    public interface IRoutePublisher
    {

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes">路由集合</param>
        void RegisterRoutes(RouteCollection routes);

    }
}
