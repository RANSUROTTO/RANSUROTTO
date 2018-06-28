using System.Web.Mvc;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    public class KeepAliveController : Controller
    {
        public virtual ActionResult Index()
        {
            return Content("I am alive!");
        }
    }
}