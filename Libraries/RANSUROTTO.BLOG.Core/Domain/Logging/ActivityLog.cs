using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Core.Domain.Logging
{
    public class ActivityLog : BaseEntity
    {

        /// <summary>
        /// 获取或设置活动日志类型ID
        /// </summary>
        public long ActivityLogTypeId { get; set; }

        /// <summary>
        /// 获取或设置用户ID
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// 获取或设置注释信息
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 获取或设置IP地址
        /// </summary>
        public string IpAddress { get; set; }

        #region Navigation Properties

        public virtual ActivityLogType ActivityLogType { get; set; }

        public virtual Customer Customer { get; set; }

        #endregion

    }
}
