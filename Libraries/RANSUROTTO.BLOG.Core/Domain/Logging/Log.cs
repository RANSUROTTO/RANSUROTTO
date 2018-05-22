using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Logging.Enum;

namespace RANSUROTTO.BLOG.Core.Domain.Logging
{

    public class Log : BaseEntity
    {

        /// <summary>
        /// 获取或设置日志等级ID
        /// </summary>
        public int LogLevelId { get; set; }

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
        public int? CustomerId { get; set; }

        /// <summary>
        /// 获取或设置请求Url
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// 获取或设置引用Url
        /// </summary>
        public string ReferrerUrl { get; set; }

        #region Navigation Properties

        /// <summary>
        /// 获取或设置日志等级
        /// </summary>
        public LogLevel LogLevel
        {
            get
            {
                return (LogLevel)LogLevelId;
            }
            set
            {
                LogLevelId = (int)value;
            }
        }

        /// <summary>
        /// 获取或设置用户
        /// </summary>
        public virtual Customer Customer { get; set; }

        #endregion

    }

}
