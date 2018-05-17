using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Framework.Localization;

namespace RANSUROTTO.BLOG.Admin.Models.Logging
{
    public class ActivityLogTypeModel : BaseEntityModel
    {

        [ResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLogType.Fields.Name")]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLogType.Fields.Enabled")]
        public bool Enabled { get; set; }

    }
}