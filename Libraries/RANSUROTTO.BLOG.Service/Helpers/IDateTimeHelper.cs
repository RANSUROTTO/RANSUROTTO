using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Service.Helpers
{
    /// <summary>
    /// 日期时间帮助类
    /// </summary>
    public interface IDateTimeHelper
    {

        TimeZoneInfo FindTimeZoneById(string id);

        ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones();

        DateTime ConvertToUserTime(DateTime dt);

        DateTime ConvertToUserTime(DateTime dt, DateTimeKind sourceDateTimeKind);

        DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone);

        DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone);

        DateTime ConvertToUtcTime(DateTime dt);

        DateTime ConvertToUtcTime(DateTime dt, DateTimeKind sourceDateTimeKind);

        DateTime ConvertToUtcTime(DateTime dt, TimeZoneInfo sourceTimeZone);

        TimeZoneInfo GetCustomerTimeZone(Customer customer);

        TimeZoneInfo DefaultStoreTimeZone { get; set; }

        TimeZoneInfo CurrentTimeZone { get; set; }

    }
}
