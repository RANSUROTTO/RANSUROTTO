using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Security
{
    public class PermissionRecordModel : BaseModel
    {
        public string Name { get; set; }
        public string SystemName { get; set; }
    }
}