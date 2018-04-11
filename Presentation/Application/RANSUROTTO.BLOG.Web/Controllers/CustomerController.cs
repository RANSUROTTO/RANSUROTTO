using System.Web.Mvc;
using RANSUROTTO.BLOG.Framework.Security;
using RANSUROTTO.BLOG.Web.Models.Customer;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    public class CustomerController : BasePublicController
    {

        #region Login / Logout

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual ActionResult Login()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }

        public virtual ActionResult Logout()
        {
            return View();
        }

        #endregion


    }
}