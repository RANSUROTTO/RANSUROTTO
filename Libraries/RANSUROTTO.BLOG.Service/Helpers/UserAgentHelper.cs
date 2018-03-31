using System.Web;
using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Service.Helpers
{
    public class UserAgentHelper : IUserAgentHelper
    {

        private readonly WebConfig _webConfig;
        private readonly HttpContextBase _httpContext;
        private static readonly object _locker = new object();

        public UserAgentHelper(WebConfig webConfig, HttpContextBase httpContext)
        {
            _webConfig = webConfig;
            _httpContext = httpContext;
        }

        public bool IsSearchEngine()
        {
            //TODO IsSearchEngine NotImplementedException
            throw new System.NotImplementedException();
        }

    }
}
