using System.Net;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Services.Tasks;

namespace RANSUROTTO.BLOG.Services.Common
{
    public class KeepAliveTask : ITask
    {

        private readonly IWebHelper _webHelper;

        public KeepAliveTask(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

        public void Execute()
        {
            string url = _webHelper.GetLocation() + "keepalive/index";
            using (var wc = new WebClient())
            {
                wc.DownloadString(url);
            }
        }
    }
}
