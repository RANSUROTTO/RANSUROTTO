using System.Web.Mvc;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class SecurityController : Controller
    {

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}