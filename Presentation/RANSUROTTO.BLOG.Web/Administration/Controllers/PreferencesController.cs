using System;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Services.Common;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class PreferencesController : BaseAdminController
    {

        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Constructor

        public PreferencesController(IGenericAttributeService genericAttributeService,
            IWorkContext workContext)
        {
            this._genericAttributeService = genericAttributeService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        [HttpPost]
        public virtual ActionResult SavePreference(string name, bool value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer, name, value);

            return Json(new
            {
                Result = true
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}