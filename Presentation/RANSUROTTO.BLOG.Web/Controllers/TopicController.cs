using System.Web.Mvc;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    public class TopicController : BasePublicController
    {


        [ChildActionOnly]
        public virtual ActionResult HomeDescriptionBlock()
        {
            return View();
        }

    }
}