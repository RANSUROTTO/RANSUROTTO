using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Logging
{
    public class LogModel : BaseEntityModel
    {

        /// <summary>
        /// 获取或设置日志等级
        /// </summary>
        [ResourceDisplayName("Admin.Logging.Log.Properies.LogLevel")]
        public string LogLevel { get; set; }

        /// <summary>
        /// 获取或设置短信息
        /// </summary>
        [ResourceDisplayName("Admin.Logging.Log.Properies.ShortMessage")]
        public string ShortMessage { get; set; }

        /// <summary>
        /// 获取或设置完整信息
        /// </summary>
        [ResourceDisplayName("Admin.Logging.Log.Properies.FullMessage")]
        public string FullMessage { get; set; }

        /// <summary>
        /// 获取或设置IP地址
        /// </summary>
        [ResourceDisplayName("Admin.Logging.Log.Properies.IpAddress")]
        public string IpAddress { get; set; }

        /// <summary>
        /// 获取或设置用户ID
        /// </summary>
        [ResourceDisplayName("Admin.Logging.Log.Properies.CustomerId")]
        public long? CustomerId { get; set; }

        /// <summary>
        /// 获取或设置请求Url
        /// </summary>
        [ResourceDisplayName("Admin.Logging.Log.Properies.PageUrl")]
        public string PageUrl { get; set; }

        /// <summary>
        /// 获取或设置引用Url
        /// </summary>
        [ResourceDisplayName("Admin.Logging.Log.Properies.ReferrerUrl")]
        public string ReferrerUrl { get; set; }

        /// <summary>
        /// 获取或设置用户Email
        /// </summary>
        [ResourceDisplayName("Admin.Logging.Log.Properies.CustomerEmail")]
        public string CustomerEmail { get; set; }

    }
}