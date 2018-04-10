using RANSUROTTO.BLOG.Framework.Controllers;
using RANSUROTTO.BLOG.Framework.Security;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    [HttpsRequirement(SslRequirement.NoMatter)]
    public abstract class BasePublicController : BaseController
    {

    }
}