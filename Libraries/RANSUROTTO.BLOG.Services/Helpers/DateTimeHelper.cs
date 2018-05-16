using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.AttributeName;
using RANSUROTTO.BLOG.Services.Common;
using RANSUROTTO.BLOG.Services.Configuration;
using RANSUROTTO.BLOG.Services.Helpers;
using RANSUROTTO.BLOG.Services.Helpers.Setting;

namespace RANSUROTTO.BLOG.Service.Helpers
{
    public class DateTimeHelper : IDateTimeHelper
    {

        #region Fields

        private readonly IWorkContext _workContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ISettingService _settingService;
        private readonly DateTimeSettings _dateTimeSettings;

        #endregion

        #region Constructor

        public DateTimeHelper(IWorkContext workContext, IGenericAttributeService genericAttributeService, ISettingService settingService, DateTimeSettings dateTimeSettings)
        {
            _workContext = workContext;
            _genericAttributeService = genericAttributeService;
            _settingService = settingService;
            _dateTimeSettings = dateTimeSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 通过标识符在注册表中检索对应的时区
        /// </summary>
        /// <param name="id">标识符</param>
        /// <returns>时区</returns>
        public TimeZoneInfo FindTimeZoneById(string id)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(id);
        }

        /// <summary>
        /// 获取所有时区排序集合
        /// </summary>
        /// <returns>时区列表</returns>
        public ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }

        /// <summary>
        /// 将时间转换为当前用户所在时区的时间
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <returns>转换后的时间</returns>
        public DateTime ConvertToUserTime(DateTime dt)
        {
            return ConvertToUserTime(dt, dt.Kind);
        }

        /// <summary>
        /// 将时间转换为当前用户所在时区的时间
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <param name="sourceDateTimeKind">原本时间的类型</param>
        /// <returns>转换后的时间</returns>
        public DateTime ConvertToUserTime(DateTime dt, DateTimeKind sourceDateTimeKind)
        {
            dt = DateTime.SpecifyKind(dt, sourceDateTimeKind);
            var currentUserTimeZoneInfo = this.CurrentTimeZone;
            return TimeZoneInfo.ConvertTime(dt, currentUserTimeZoneInfo);
        }

        /// <summary>
        /// 将时间转换为当前用户所在时区的时间
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <param name="sourceTimeZone">原本时间的时区</param>
        /// <returns>转换后的时间</returns>
        public DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone)
        {
            var currentUserTimeZoneInfo = this.CurrentTimeZone;
            return ConvertToUserTime(dt, sourceTimeZone, currentUserTimeZoneInfo);
        }

        /// <summary>
        /// 将时间转换为当前用户所在时区的时间
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <param name="sourceTimeZone">原本时间的时区</param>
        /// <param name="destinationTimeZone">转换目标时间的时区</param>
        /// <returns>转换后的时间</returns>
        public DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
        {
            return TimeZoneInfo.ConvertTime(dt, sourceTimeZone, destinationTimeZone);
        }

        /// <summary>
        /// 将时间转换为UTC(世界协调时间)
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <returns>转换后的时间</returns>
        public DateTime ConvertToUtcTime(DateTime dt)
        {
            return ConvertToUtcTime(dt, dt.Kind);
        }

        /// <summary>
        /// 将时间转换为UTC(世界协调时间)
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <param name="sourceDateTimeKind">原本时间的类型</param>
        /// <returns>转换后的时间</returns>
        public DateTime ConvertToUtcTime(DateTime dt, DateTimeKind sourceDateTimeKind)
        {
            dt = DateTime.SpecifyKind(dt, sourceDateTimeKind);
            return TimeZoneInfo.ConvertTimeToUtc(dt);
        }

        /// <summary>
        /// 将时间转换为UTC(世界协调时间)
        /// </summary>
        /// <param name="dt">原本时间</param>
        /// <param name="sourceTimeZone">原本时间的时区</param>
        /// <returns>转换后的时间</returns>
        public DateTime ConvertToUtcTime(DateTime dt, TimeZoneInfo sourceTimeZone)
        {
            if (sourceTimeZone.IsInvalidTime(dt))
            {
                //无法转换
                return dt;
            }

            return TimeZoneInfo.ConvertTimeToUtc(dt, sourceTimeZone);
        }

        /// <summary>
        /// 获取指定用户所在位置的时区
        /// </summary>
        /// <param name="customer">用户</param>
        /// <returns>时区</returns>
        public TimeZoneInfo GetCustomerTimeZone(Customer customer)
        {
            TimeZoneInfo timeZoneInfo = null;
            if (_dateTimeSettings.AllowCustomersToSetTimeZone)
            {
                string timeZoneId = string.Empty;
                if (customer != null)
                    timeZoneId = customer.GetAttribute<string>(SystemCustomerAttributeNames.TimeZoneId, _genericAttributeService);

                try
                {
                    if (!String.IsNullOrEmpty(timeZoneId))
                        timeZoneInfo = FindTimeZoneById(timeZoneId);
                }
                catch (Exception exc)
                {
                    Debug.Write(exc.ToString());
                }
            }

            if (timeZoneInfo == null)
                timeZoneInfo = this.DefaultTimeZone;

            return timeZoneInfo;
        }

        /// <summary>
        /// 获取或设置系统默认的时区
        /// </summary>
        public TimeZoneInfo DefaultTimeZone
        {
            get
            {
                TimeZoneInfo timeZoneInfo = null;
                try
                {
                    if (!String.IsNullOrEmpty(_dateTimeSettings.DefaultStoreTimeZoneId))
                        timeZoneInfo = FindTimeZoneById(_dateTimeSettings.DefaultStoreTimeZoneId);
                }
                catch (Exception exc)
                {
                    Debug.Write(exc.ToString());
                }

                if (timeZoneInfo == null)
                    timeZoneInfo = TimeZoneInfo.Local;

                return timeZoneInfo;
            }
            set
            {
                string defaultTimeZoneId = string.Empty;
                if (value != null)
                {
                    defaultTimeZoneId = value.Id;
                }

                _dateTimeSettings.DefaultStoreTimeZoneId = defaultTimeZoneId;
                _settingService.SaveSetting(_dateTimeSettings);
            }
        }

        /// <summary>
        /// 获取或设置当前用户的时区
        /// </summary>
        public TimeZoneInfo CurrentTimeZone
        {
            get
            {
                return GetCustomerTimeZone(_workContext.CurrentCustomer);
            }
            set
            {
                if (!_dateTimeSettings.AllowCustomersToSetTimeZone)
                    return;

                string timeZoneId = string.Empty;
                if (value != null)
                {
                    timeZoneId = value.Id;
                }

                _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer,
                    SystemCustomerAttributeNames.TimeZoneId, timeZoneId);
            }
        }

        #endregion

    }
}
