using System.Web.Mvc;
using System.Web.Routing;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc.Routes;

namespace RANSUROTTO.BLOG.Web.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //Home page
            routes.MapLocalizedRoute("HomePage",
                "",
                new { controller = "Home", action = "Index" },
                new[] { "RANSUROTTO.BLOG.Web.Controllers" });

            //Login page
            routes.MapLocalizedRoute("Login",
                "login/",
                new { controller = "Customer", action = "Login" },
                new[] { "RANSUROTTO.BLOG.Web.Controllers" });
            //Register page
            routes.MapLocalizedRoute("Register",
                "register/",
                new { controller = "Customer", action = "Register" },
                new[] { "RANSUROTTO.BLOG.Web.Controllers" });
            //Lsogout page
            routes.MapLocalizedRoute("Logout",
                "logout/",
                new { controller = "Customer", action = "Logout" },
                new[] { "RANSUROTTO.BLOG.Web.Controllers" });

            //Install page
            routes.MapRoute("Installation",
                "install",
                new { controller = "Install", action = "Index" },
                new[] { "RANSUROTTO.BLOG.Web.Controllers" });

            //Page not found page
            routes.MapLocalizedRoute("PageNotFound",
                "page-not-found",
                new { controller = "Common", action = "PageNotFound" },
                new[] { "RANSUROTTO.BLOG.Web.Controllers" });
        }

        public int Priority => 0;
    }
}