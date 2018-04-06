using System;
using System.Collections.ObjectModel;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Service.Configuration;

namespace RANSUROTTO.BLOG.Service.Helpers
{
    public class DateTimeHelper : IDateTimeHelper
    {


        public TimeZoneInfo FindTimeZoneById(string id)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones()
        {
            throw new NotImplementedException();
        }

        public DateTime ConvertToUserTime(DateTime dt)
        {
            throw new NotImplementedException();
        }

        public DateTime ConvertToUserTime(DateTime dt, DateTimeKind sourceDateTimeKind)
        {
            throw new NotImplementedException();
        }

        public DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone)
        {
            throw new NotImplementedException();
        }

        public DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
        {
            throw new NotImplementedException();
        }

        public DateTime ConvertToUtcTime(DateTime dt)
        {
            throw new NotImplementedException();
        }

        public DateTime ConvertToUtcTime(DateTime dt, DateTimeKind sourceDateTimeKind)
        {
            throw new NotImplementedException();
        }

        public DateTime ConvertToUtcTime(DateTime dt, TimeZoneInfo sourceTimeZone)
        {
            throw new NotImplementedException();
        }

        public TimeZoneInfo GetCustomerTimeZone(Customer customer)
        {
            throw new NotImplementedException();
        }

        public TimeZoneInfo DefaultTimeZone { get; set; }
        public TimeZoneInfo CurrentTimeZone { get; set; }
    }
}
