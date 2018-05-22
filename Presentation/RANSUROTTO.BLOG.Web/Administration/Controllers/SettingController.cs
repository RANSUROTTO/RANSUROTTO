using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Models.Settings;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Services.Common;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class SettingController : BaseAdminController
    {

        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Constructor

        public SettingController(IGenericAttributeService genericAttributeService, IWorkContext workContext)
        {
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        [ChildActionOnly]
        public virtual ActionResult Mode(string modeName = "settings-advanced-mode")
        {
            var model = new ModeModel
            {
                ModeName = modeName,
                Enabled = _workContext.CurrentCustomer.GetAttribute<bool>(modeName)
            };
            return PartialView(model);
        }

        #endregion

    }
}