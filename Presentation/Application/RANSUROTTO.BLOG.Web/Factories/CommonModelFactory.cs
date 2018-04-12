using System.IO;
using System.Web;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Web.Models.Common;

namespace RANSUROTTO.BLOG.Web.Factories
{
    public class CommonModelFactory : ICommonModelFactory
    {

        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Constructor

        public CommonModelFactory(HttpContextBase httpContext, IWebHelper webHelper)
        {
            _httpContext = httpContext;
            _webHelper = webHelper;
        }

        #endregion

        #region Methods

        public FaviconModel PrepareFaviconModel()
        {
            var model = new FaviconModel();

            var faviconFileName = "favicon.ico";

            var localFaviconPath = System.IO.Path.Combine(_httpContext.Request.PhysicalApplicationPath, faviconFileName);
            if (!File.Exists(localFaviconPath))
                return model;

            model.FaviconUrl = _webHelper.GetLocation() + faviconFileName;
            return model;
        }

        public AdminHeaderLinksModel PrepareAdminHeaderLinksModel()
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }
}