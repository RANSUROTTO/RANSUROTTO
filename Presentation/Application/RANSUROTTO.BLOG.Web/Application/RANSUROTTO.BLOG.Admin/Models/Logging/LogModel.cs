using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Logging
{
    public class LogModel : BaseEntityModel
    {

        /// <summary>
        /// 获取或设置日志等级
        /// </summary>
        public string LogLevel { get; set; }

        /// <summary>
        /// 获取或设置短信息
        /// </summary>
        public string ShortMessage { get; set; }

        /// <summary>
        /// 获取或设置完整信息
        /// </summary>
        public string FullMessage { get; set; }

        /// <summary>
        /// 获取或设置IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 获取或设置用户ID
        /// </summary>
        public long? CustomerId { get; set; }

        /// <summary>
        /// 获取或设置请求Url
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// 获取或设置引用Url
        /// </summary>
        public string ReferrerUrl { get; set; }

        /// <summary>
        /// 获取或设置用户Email
        /// </summary>
        public string CustomerEmail { get; set; }

    }
}