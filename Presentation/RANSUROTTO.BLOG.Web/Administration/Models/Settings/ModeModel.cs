using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Settings
{
    public class ModeModel : BaseModel
    {
        public string ModeName { get; set; }
        public bool Enabled { get; set; }
    }
}