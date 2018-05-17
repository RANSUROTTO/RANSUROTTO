using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Domain.Customers.Setting;
using RANSUROTTO.BLOG.Services.Customers;
using RANSUROTTO.BLOG.Services.Helpers;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Admin.Controllers
{
    public class OnlineCustomerController : BaseAdminController
    {

        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly CustomerSettings _customerSettings;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Constructor

        public OnlineCustomerController(ICustomerService customerService, IDateTimeHelper dateTimeHelper, CustomerSettings customerSettings, ILocalizationService localizationService)
        {
            _customerService = customerService;
            _dateTimeHelper = dateTimeHelper;
            _customerSettings = customerSettings;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        public virtual ActionResult List()
        {
            return View();
        }


        #endregion

    }
}