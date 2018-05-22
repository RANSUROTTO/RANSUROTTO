using System;
using System.Web.Mvc;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Framework.Mvc;
using System.ComponentModel.DataAnnotations;
using RANSUROTTO.BLOG.Framework.Localization;

namespace RANSUROTTO.BLOG.Admin.Models.Logging
{
    public class ActivityLogListModel : BaseModel
    {
        public ActivityLogListModel()
        {
            ActivityLogType = new List<SelectListItem>();
        }

        [ResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.CreatedOnFrom")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [ResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.CreatedOnTo")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [ResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.ActivityLogType")]
        public int ActivityLogTypeId { get; set; }

        [ResourceDisplayName("Admin.Customers.Customers.ActivityLog.IpAddress")]
        public string IpAddress { get; set; }

        [ResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.ActivityLogType")]
        public IList<SelectListItem> ActivityLogType { get; set; }

    }
}