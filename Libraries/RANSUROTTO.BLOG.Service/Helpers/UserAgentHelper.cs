using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Web;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;

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
            if (_httpContext == null)
                return false;

            //we put required logic in try-catch block
            //more info: http://www.nopcommerce.com/boards/t/17711/unhandled-exception-request-is-not-available-in-this-context.aspx
            try
            {
                var bowscapXmlHelper = GetBrowscapXmlHelper();

                //we cannot load parser
                if (bowscapXmlHelper == null)
                    return false;

                var userAgent = _httpContext.Request.UserAgent;
                return bowscapXmlHelper.IsCrawler(userAgent);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected virtual BrowscapXmlHelper GetBrowscapXmlHelper()
        {
            if (Singleton<BrowscapXmlHelper>.Instance != null)
                return Singleton<BrowscapXmlHelper>.Instance;

            //no database created
            if (String.IsNullOrEmpty(_webConfig.UserAgentStringsPath))
                return null;

            //prevent multi loading data
            lock (_locker)
            {
                //data can be loaded while we waited
                if (Singleton<BrowscapXmlHelper>.Instance != null)
                    return Singleton<BrowscapXmlHelper>.Instance;

                var userAgentStringsPath = CommonHelper.MapPath(_webConfig.UserAgentStringsPath);
                var crawlerOnlyUserAgentStringsPath = string.IsNullOrEmpty(_webConfig.CrawlerOnlyUserAgentStringsPath) ? string.Empty : CommonHelper.MapPath(_config.CrawlerOnlyUserAgentStringsPath);

                var browscapXmlHelper = new BrowscapXmlHelper(userAgentStringsPath, crawlerOnlyUserAgentStringsPath);
                Singleton<BrowscapXmlHelper>.Instance = browscapXmlHelper;

                return Singleton<BrowscapXmlHelper>.Instance;
            }
        }

    }
}
