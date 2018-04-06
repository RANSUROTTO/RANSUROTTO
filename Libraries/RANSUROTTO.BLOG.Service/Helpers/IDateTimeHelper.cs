using System;
using System.Collections.ObjectModel;
using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Service.Helpers
{
    /// <summary>
    /// 日期时间帮助类
    /// </summary>
    public interface IDateTimeHelper
    {

        /// <summary>
        /// 通过标识符在注册表中检索对应的时区
        /// </summary>
        /// <param name="id">标识符</param>
        /// <returns>时区</returns>
        TimeZoneInfo FindTimeZoneById(string id);

        /// <summary>
        /// 获取所有时区排序集合
        /// </summary>
        /// <returns>时区列表</returns>
        ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones();

        /// <summary>
        /// 将时间转换为当前用户所在时区的时间
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <returns>转换后的时间</returns>
        DateTime ConvertToUserTime(DateTime dt);

        /// <summary>
        /// 将时间转换为当前用户所在时区的时间
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <param name="sourceDateTimeKind">原本时间的类型</param>
        /// <returns>转换后的时间</returns>
        DateTime ConvertToUserTime(DateTime dt, DateTimeKind sourceDateTimeKind);

        /// <summary>
        /// 将时间转换为当前用户所在时区的时间
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <param name="sourceTimeZone">原本时间的时区</param>
        /// <returns>转换后的时间</returns>
        DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone);

        /// <summary>
        /// 将时间转换为当前用户所在时区的时间
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <param name="sourceTimeZone">原本时间的时区</param>
        /// <param name="destinationTimeZone">转换目标时间的时区</param>
        /// <returns>转换后的时间</returns>
        DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone);

        /// <summary>
        /// 将时间转换为UTC(世界协调时间)
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <returns>转换后的时间</returns>
        DateTime ConvertToUtcTime(DateTime dt);

        /// <summary>
        /// 将时间转换为UTC(世界协调时间)
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <param name="sourceDateTimeKind">原本时间的类型</param>
        /// <returns>转换后的时间</returns>
        DateTime ConvertToUtcTime(DateTime dt, DateTimeKind sourceDateTimeKind);

        /// <summary>
        /// 将时间转换为UTC(世界协调时间)
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <param name="sourceTimeZone">原本时间的时区</param>
        /// <returns>转换后的时间</returns>
        DateTime ConvertToUtcTime(DateTime dt, TimeZoneInfo sourceTimeZone);

        /// <summary>
        /// 获取指定用户所在位置的时区
        /// </summary>
        /// <param name="customer">用户</param>
        /// <returns>时区</returns>
        TimeZoneInfo GetCustomerTimeZone(Customer customer);

        /// <summary>
        /// 获取系统默认的时区
        /// </summary>
        TimeZoneInfo DefaultTimeZone { get; set; }

        /// <summary>
        /// 获取当前用户的时区
        /// </summary>
        TimeZoneInfo CurrentTimeZone { get; set; }

    }
}
