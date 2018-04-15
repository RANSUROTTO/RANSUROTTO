using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Web.Models.Common
{
    public class AdminHeaderLinksModel : BaseModel
    {

        /// <summary>
        /// 获取或设置是否显示进入管理员区域链接
        /// </summary>
        public bool DisplayAdminLink { get; set; }

        /// <summary>
        /// 获取或设置编辑页面路径
        /// </summary>
        public string EditPageUrl { get; set; }

    }
}