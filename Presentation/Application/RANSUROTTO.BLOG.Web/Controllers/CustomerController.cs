using System.Web.Mvc;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    public class CustomerController : BasePublicController
    {

        #region Login / Logout

        public virtual ActionResult Login()
        {
            return View();
        }

        public virtual ActionResult Logout()
        {
            return View();
        }

        #endregion


    }
}