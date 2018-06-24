using System.IO;
using System.Web;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Framework.UI;
using RANSUROTTO.BLOG.Services.Security;
using RANSUROTTO.BLOG.Web.Models.Common;

namespace RANSUROTTO.BLOG.Web.Factories
{
    public class CommonModelFactory : ICommonModelFactory
    {

        #region Fields

        private readonly IWorkContext _workContext;
        private readonly HttpContextBase _httpContext;
        private readonly IWebHelper _webHelper;
        private readonly IPageHeadBuilder _pageHeadBuilder;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Constructor

        public CommonModelFactory(IWorkContext workContext, HttpContextBase httpContext, IWebHelper webHelper, IPageHeadBuilder pageHeadBuilder, IPermissionService permissionService)
        {
            _workContext = workContext;
            _httpContext = httpContext;
            _webHelper = webHelper;
            _pageHeadBuilder = pageHeadBuilder;
            _permissionService = permissionService;
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
            var customer = _workContext.CurrentCustomer;

            var model = new AdminHeaderLinksModel
            {
                DisplayAdminLink = _permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel),
                EditPageUrl = _pageHeadBuilder.GetEditPageUrl()
            };

            return model;
        }

        #endregion

    }
}