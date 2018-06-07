using System;
using System.Web.Mvc;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Framework.Mvc;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using RANSUROTTO.BLOG.Admin.Validators.Customers;
using RANSUROTTO.BLOG.Framework.Localization;

namespace RANSUROTTO.BLOG.Admin.Models.Customers
{
    [Validator(typeof(CustomerValidator))]
    public class CustomerModel : BaseEntityModel
    {
        public CustomerModel()
        {
            AvailableTimeZones = new List<SelectListItem>();
            AvailableCustomerRoles = new List<SelectListItem>();
            SelectedCustomerRoleIds = new List<int>();
        }

        [ResourceDisplayName("Admin.Customers.Customers.Fields.Active")]
        public bool Active { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.Fields.Username")]
        [AllowHtml]
        public string Username { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.Fields.Email")]
        [AllowHtml]
        public string Email { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.Fields.Password")]
        [AllowHtml]
        [DataType(DataType.Password)]
        [NoTrim]
        public string Password { get; set; }

        public bool GenderEnabled { get; set; }
        [ResourceDisplayName("Admin.Customers.Customers.Fields.Gender")]
        public string Gender { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.Fields.Name")]
        public string Name { get; set; }

        public bool DateOfBirthEnabled { get; set; }
        [UIHint("DateNullable")]
        [ResourceDisplayName("Admin.Customers.Customers.Fields.DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        public bool CompanyEnabled { get; set; }
        [ResourceDisplayName("Admin.Customers.Customers.Fields.Company")]
        [AllowHtml]
        public string Company { get; set; }

        public bool PhoneEnabled { get; set; }
        [ResourceDisplayName("Admin.Customers.Customers.Fields.Phone")]
        [AllowHtml]
        public string Phone { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.Fields.AdminComment")]
        [AllowHtml]
        public string AdminComment { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.Fields.TimeZoneId")]
        [AllowHtml]
        public string TimeZoneId { get; set; }
        public bool AllowCustomersToSetTimeZone { get; set; }
        public IList<SelectListItem> AvailableTimeZones { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.Fields.LastActivityDate")]
        public DateTime LastActivityDate { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.Fields.IPAddress")]
        public string LastIpAddress { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.Fields.LastVisitedPage")]
        public string LastVisitedPage { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.Fields.CustomerRoles")]
        public string CustomerRoleNames { get; set; }
        public List<SelectListItem> AvailableCustomerRoles { get; set; }
        [UIHint("MultiSelect")]
        [ResourceDisplayName("Admin.Customers.Customers.Fields.CustomerRoles")]
        public IList<int> SelectedCustomerRoleIds { get; set; }

        public class ActivityLogModel : BaseEntityModel
        {
            [ResourceDisplayName("Admin.Customers.Customers.ActivityLog.ActivityLogType")]
            public string ActivityLogTypeName { get; set; }
            [ResourceDisplayName("Admin.Customers.Customers.ActivityLog.Comment")]
            public string Comment { get; set; }
            [ResourceDisplayName("Admin.Customers.Customers.ActivityLog.IpAddress")]
            public string IpAddress { get; set; }
            [ResourceDisplayName("Admin.Customers.Customers.ActivityLog.CreatedOn")]
            public override DateTime CreatedOn { get; set; }
        }

        public class BlogPostModel : BaseEntityModel
        {
            [ResourceDisplayName("Admin.Customers.Customers.BlogPost.Title")]
            public string Title { get; set; }
            [ResourceDisplayName("Admin.Customers.Customers.BlogPost.Published")]
            public bool Published { get; set; }
            [ResourceDisplayName("Admin.Customers.Customers.BlogPost.CreatedOn")]
            public override DateTime CreatedOn { get; set; }
            [ResourceDisplayName("Admin.Customers.Customers.BlogPost.UpdateOn")]
            public DateTime UpdateOnUtc { get; set; }
            [ResourceDisplayName("Admin.Customers.Customers.BlogPost.Deleted")]
            public bool Deleted { get; set; }
        }

    }
}