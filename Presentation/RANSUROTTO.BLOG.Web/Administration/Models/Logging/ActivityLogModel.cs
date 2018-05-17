using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Logging
{
    public class ActivityLogModel : BaseEntityModel
    {

        [ResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.ActivityLogType")]
        public string ActivityLogTypeName { get; set; }

        [ResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.Customer")]
        public int CustomerId { get; set; }

        [ResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.Customer")]
        public string CustomerEmail { get; set; }

        [ResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.Comment")]
        public string Comment { get; set; }

        [ResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.CreatedOn")]
        public override DateTime CreatedOn { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.ActivityLog.IpAddress")]
        public string IpAddress { get; set; }

    }
}