using System;
using RANSUROTTO.BLOG.Core.Domain.Logging.Enum;
using RANSUROTTO.BLOG.Core.Domain.Members;

namespace RANSUROTTO.BLOG.Service.Logging
{
    public static class LoggingExtensions
    {

        public static void Debug(this ILogger logger, string message, Exception exception = null, Customer customer = null)
        {
            FilteredLog(logger, LogLevel.Debug, message, exception, customer);
        }
        public static void Information(this ILogger logger, string message, Exception exception = null, Customer customer = null)
        {
            FilteredLog(logger, LogLevel.Information, message, exception, customer);
        }
        public static void Warning(this ILogger logger, string message, Exception exception = null, Customer customer = null)
        {
            FilteredLog(logger, LogLevel.Warning, message, exception, customer);
        }
        public static void Error(this ILogger logger, string message, Exception exception = null, Customer customer = null)
        {
            FilteredLog(logger, LogLevel.Error, message, exception, customer);
        }
        public static void Fatal(this ILogger logger, string message, Exception exception = null, Customer customer = null)
        {
            FilteredLog(logger, LogLevel.Fatal, message, exception, customer);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logger">日志业务层实例</param>
        /// <param name="level">日志级别</param>
        /// <param name="message">日志消息</param>
        /// <param name="exception">记录异常</param>
        /// <param name="customer">触发用户</param>
        private static void FilteredLog(ILogger logger, LogLevel level, string message, Exception exception = null, Customer customer = null)
        {
            //忽略线程手动中止异常
            if (exception is System.Threading.ThreadAbortException)
                return;

            string fullMessage = exception + "";
            logger.InsertLog(level, message, fullMessage, customer);

        }

    }
}
