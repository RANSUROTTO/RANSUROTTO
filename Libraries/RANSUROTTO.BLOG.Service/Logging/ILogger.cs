using System;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Logging;
using RANSUROTTO.BLOG.Core.Domain.Logging.Enum;

namespace RANSUROTTO.BLOG.Services.Logging
{

    /// <summary>
    /// 日志业务层接口
    /// </summary>
    public interface ILogger
    {

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="log">日志</param>
        void DeleteLog(Log log);

        /// <summary>
        /// 删除多个日志对象
        /// </summary>
        /// <param name="logs">日志集合</param>
        void DeleteLogs(IList<Log> logs);

        /// <summary>
        /// 清空日志
        /// </summary>
        void ClearLog();

        /// <summary>
        /// 通过指定条件获取日志列表
        /// </summary>
        /// <param name="fromUtc">该UTC时间后创建的日志; NULL为不限制</param>
        /// <param name="toUtc">该UTC时间前创建的日志; NULL为不限制</param>
        /// <param name="message">消息</param>
        /// <param name="logLevel">日志级别; NULL为不限制</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>日志列表</returns>
        IPagedList<Log> GetAllLogs(DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = "", LogLevel? logLevel = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 通过标识符获取日志
        /// </summary>
        /// <param name="logId">日志标识符</param>
        /// <returns>日志</returns>
        Log GetLogById(long logId);

        /// <summary>
        /// 通过标识符列表获取日志列表
        /// </summary>
        /// <param name="logIds">日志标识符列表</param>
        /// <returns>日志列表</returns>
        IList<Log> GetLogByIds(long[] logIds);

        /// <summary>
        /// 插入一条日志
        /// </summary>
        /// <param name="logLevel">日志等级</param>
        /// <param name="shortMessage">简短消息</param>
        /// <param name="fullMessage">完整消息</param>
        /// <param name="customer">触发日志记录的用户</param>
        /// <returns>日志</returns>
        Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", Customer customer = null);

    }
}
