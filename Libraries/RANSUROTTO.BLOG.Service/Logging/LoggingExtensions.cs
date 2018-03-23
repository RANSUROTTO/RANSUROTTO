using System;
using RANSUROTTO.BLOG.Core.Domain.Members;

namespace RANSUROTTO.BLOG.Service.Logging
{
    public static class LoggingExtensions
    {

        public static void Debug(this ILogger logger, string message, Exception exception = null, Customer customer = null)
        {
            throw new NotImplementedException();
        }
        public static void Information(this ILogger logger, string message, Exception exception = null, Customer customer = null)
        {
            throw new NotImplementedException();
        }
        public static void Warning(this ILogger logger, string message, Exception exception = null, Customer customer = null)
        {
            throw new NotImplementedException();
        }
        public static void Error(this ILogger logger, string message, Exception exception = null, Customer customer = null)
        {
            throw new NotImplementedException();
        }
        public static void Fatal(this ILogger logger, string message, Exception exception = null, Customer customer = null)
        {
            throw new NotImplementedException();
        }

    }
}
