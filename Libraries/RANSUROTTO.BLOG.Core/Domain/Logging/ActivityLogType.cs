using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Logging
{
    /// <summary>
    /// 活动日志类型
    /// </summary>
    public class ActivityLogType : BaseEntity
    {

        /// <summary>
        /// 获取或设置系统关键字
        /// </summary>
        public string SystemKeyword { get; set; }

        /// <summary>
        /// 获取或设置显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置该类型是否已启用
        /// </summary>
        public bool Enabled { get; set; }

    }
}
