using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Customers.AttributeName
{
    public class SystemCustomerAttributeNames : IAttributeNames
    {

        /// <summary>
        /// 姓名/昵称
        /// </summary>
        public static string Name => "Name";
        /// <summary>
        /// 公司
        /// </summary>
        public static string Company => "Company";
        /// <summary>
        /// 邮政编码
        /// </summary>
        public static string ZipPostalCode => "ZipPostalCode";
        /// <summary>
        /// 手机号
        /// </summary>
        public static string Phone => "Phone";
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
