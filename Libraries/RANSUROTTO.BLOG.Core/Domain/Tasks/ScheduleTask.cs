using System;
using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Tasks
{

    /// <summary>
    /// 计划任务
    /// </summary>
    public class ScheduleTask : BaseEntity
    {

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置运行周期(秒为单位)
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// 获取或设置适当的IScheduleTask类的类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置任务是否已启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 获取或设置标识任务执行中遇到错误时是否停止
        /// </summary>
        public bool StopOnError { get; set; }

        /// <summary>
        /// 获取或设置租用此任务的计算机名称（实例）, 它在Web场中运行时使用（确保只在一台机器上运行任务）, 未在Web场中运行时可能为空
        /// </summary>
        public string LeasedByMachineName { get; set; }

        /// <summary>
        /// 获取或设置日期时间，直到某个机器（实例）租用任务。 它在Web场中运行时使用（确保只在一台机器上运行任务）
        /// </summary>
        public DateTime? LeasedUntilUtc { get; set; }

        /// <summary>
        /// 获取或设置最后开始执行Utc时间
        /// </summary>
        public DateTime? LastStartUtc { get; set; }

        /// <summary>
        /// 获取或设置最后执行完毕Utc时间(无论成功或者失败)
        /// </summary>
        public DateTime? LastEndUtc { get; set; }

        /// <summary>
        /// 获取或设置最后执行成功完毕Utc时间
        /// </summary>
        public DateTime? LastSuccessUtc { get; set; }

    }
}
