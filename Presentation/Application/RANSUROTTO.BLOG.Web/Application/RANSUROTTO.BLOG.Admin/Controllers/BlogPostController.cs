using System.Web.Mvc;
using RANSUROTTO.BLOG.Framework.Kendoui;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class BlogPostController : Controller
    {
        
        public virtual ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [HttpGet]
        public virtual ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult List(DataSourceRequest command)
        {
            return View();
        }


    }
}