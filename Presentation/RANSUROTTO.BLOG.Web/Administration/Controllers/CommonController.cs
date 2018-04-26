using System;
using System.Linq;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Admin.Extensions;
using RANSUROTTO.BLOG.Admin.Models.Common;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Service.Localization;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class CommonController : BaseAdminController
    {

        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;

        public CommonController(ILanguageService languageService, IWorkContext workContext)
        {
            _languageService = languageService;
            _workContext = workContext;
        }

        public virtual ActionResult LanguageSelector()
        {
            var model = new LanguageSelectorModel();
            model.CurrentLanguage = _workContext.WorkingLanguage.ToModel();
            model.AvailableLanguages = _languageService
                .GetAllLanguages()
                .Select(l => l.ToModel())
                .ToList();

            return PartialView(model);
        }

        public virtual ActionResult SetLanguage(long langid, string returnUrl = "")
        {
            var language = _languageService.GetLanguageById(langid);
            if (language != null)
            {
                _workContext.WorkingLanguage = language;
            }

            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.Action("Index", "Home", new { area = "Admin" });
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new { area = "Admin" });

            return Redirect(returnUrl);
        }

    }
}