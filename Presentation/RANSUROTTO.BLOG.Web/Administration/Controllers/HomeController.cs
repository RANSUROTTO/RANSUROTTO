using System.Web.Mvc;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {

        public virtual ActionResult Index()
        {
            return View();
        }


    }
}