using System.Web.Mvc;

namespace RANSUROTTO.BLOG.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", area = "Admin", id = "" },
                new[] { "RANSUROTTO.BLOG.Admin.Controllers" }
                );
        }

        public override string AreaName => "Admin";
    }
}