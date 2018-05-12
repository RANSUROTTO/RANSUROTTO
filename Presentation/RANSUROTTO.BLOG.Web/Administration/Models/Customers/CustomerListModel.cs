using System.Web.Mvc;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Framework.Mvc;
using System.ComponentModel.DataAnnotations;
using RANSUROTTO.BLOG.Framework.Localization;

namespace RANSUROTTO.BLOG.Admin.Models.Customers
{
    public class CustomerListModel : BaseModel
    {
        public CustomerListModel()
        {
            AvailableCustomerRoles = new List<SelectListItem>();
            SearchCustomerRoleIds = new List<long>();
        }

        [ResourceDisplayName("Admin.Customers.Customers.List.SearchEmail")]
        [AllowHtml]
        public string SearchEmail { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.List.SearchUsername")]
        [AllowHtml]
        public string SearchUsername { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.List.SearchName")]
        [AllowHtml]
        public string SearchName { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.List.SearchDateOfBirth")]
        [AllowHtml]
        public string SearchDayOfBirth { get; set; }
        [ResourceDisplayName("Admin.Customers.Customers.List.SearchDateOfBirth")]
        [AllowHtml]
        public string SearchMonthOfBirth { get; set; }
        public bool DateOfBirthEnabled { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.List.SearchCompany")]
        [AllowHtml]
        public string SearchCompany { get; set; }
        public bool CompanyEnabled { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.List.SearchPhone")]
        [AllowHtml]
        public string SearchPhone { get; set; }
        public bool PhoneEnabled { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.List.SearchIpAddress")]
        public string SearchIpAddress { get; set; }

        public IList<SelectListItem> AvailableCustomerRoles { get; set; }
        [UIHint("MultiSelect")]
        [ResourceDisplayName("Admin.Customers.Customers.List.CustomerRoles")]
        public IList<long> SearchCustomerRoleIds { get; set; }

    }
}