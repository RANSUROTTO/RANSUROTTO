using System.Web.Mvc;
using RANSUROTTO.BLOG.Framework.Attributes;

namespace RANSUROTTO.BLOG.Framework.Controllers
{
    [StoreIpAddress]
    [CustomerLastActivity]
    public class BaseController : Controller
    {
    }

}
