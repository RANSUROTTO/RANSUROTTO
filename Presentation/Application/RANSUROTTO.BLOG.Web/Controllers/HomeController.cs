using System.Web.Mvc;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    public class HomeController : BasePublicController
    {

        public virtual ActionResult Index()
        {
            return View();
        }

    }
}