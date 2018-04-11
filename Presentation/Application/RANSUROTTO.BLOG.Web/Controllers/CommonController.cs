using System.Web.Mvc;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    public class CommonController : BasePublicController
    {

        public virtual ActionResult Favicon()
        {
            return PartialView();
        }

    }
}