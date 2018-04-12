using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Customers.AttributeName
{
    public class SystemCustomerAttributeNames : IAttributeNames
    {
        /// <summary>
        /// 语言标识符
        /// </summary>
        public static string LanguageId => "LanguageId";
        /// <summary>
        /// 时区标识符
        /// </summary>
        public static string TimeZoneId => "TimeZoneId";
        /// <summary>
        /// 自动检测语言
        /// </summary>
        public static string LanguageAutomaticallyDetected { get { return "LanguageAutomaticallyDetected"; } }

    }
}
