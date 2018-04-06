using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Service.Helpers.Setting
{
    public class DateTimeSettings : ISettings
    {

        /// <summary>
        /// 获取或设置系统默认时区标识符
        /// </summary>
        public string DefaultStoreTimeZoneId { get; set; }

        /// <summary>
        /// 获取或设置是否允许用户进行设置时区
        /// </summary>
        public bool AllowCustomersToSetTimeZone { get; set; }

    }
}
