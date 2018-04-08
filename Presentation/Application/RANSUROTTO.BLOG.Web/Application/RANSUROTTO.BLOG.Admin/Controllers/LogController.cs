using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class LogController : BaseAdminController
    {
        // GET: Log
        public ActionResult Index()
        {
            return View();
        }
    }
}