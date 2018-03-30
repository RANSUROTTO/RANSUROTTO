using System;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Security.Setting;
using RANSUROTTO.BLOG.Core.Infrastructure;

namespace RANSUROTTO.BLOG.Framework.Security
{

    /// <summary>
    /// 该特性用于确保进行公共区域XSRF保护处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class PublicAntiForgeryAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly bool _ignore;

        public PublicAntiForgeryAttribute(bool ignore = false)
        {
            this._ignore = ignore;
        }

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (_ignore)
                return;

            if (filterContext.IsChildAction)
                return;

            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            var securitySettings = EngineContext.Current.Resolve<SecuritySettings>();
            if (!securitySettings.EnableXsrfProtectionForPublicArea)
                return;

            var validator = new ValidateAntiForgeryTokenAttribute();
            validator.OnAuthorization(filterContext);
        }

    }
}
