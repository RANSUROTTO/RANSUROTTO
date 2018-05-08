using System;
using System.Collections.Generic;
using System.Linq;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Logging;
using RANSUROTTO.BLOG.Core.Domain.Logging.Setting;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Data.Context;

namespace RANSUROTTO.BLOG.Services.Logging
{
    public class CustomerActivityService : ICustomerActivityService
    {

        #region Constants

        /// <summary>
        /// 活动日志类型缓存键
        /// </summary>
        private const string ACTIVITYTYPE_ALL_KEY = "Ransurotto.activitytype.all";

        /// <summary>
        /// 缓存清空匹配模式
        /// </summary>
        private const string ACTIVITYTYPE_PATTERN_KEY = "Ransurotto.activitytype.";

        #endregion

        #region Fields

        private readonly IWebHelper _webHelper;
        private readonly LogSettings _logSettings;
        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IRepository<ActivityLog> _activityLogRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructor

        public CustomerActivityService(IWebHelper webHelper, LogSettings logSettings, IRepository<ActivityLogType> activityLogTypeRepository, IRepository<ActivityLog> activityLogRepository, ICacheManager cacheManager, IWorkContext workContext, IDbContext dbContext)
        {
            _webHelper = webHelper;
            _logSettings = logSettings;
            _activityLogTypeRepository = activityLogTypeRepository;
            _activityLogRepository = activityLogRepository;
            _cacheManager = cacheManager;
            _workContext = workContext;
            _dbContext = dbContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 插入活动日志类型
        /// </summary>
        /// <param name="activityLogType">活动日志类型</param>
        public virtual void InsertActivityType(ActivityLogType activityLogType)
        {
            if (activityLogType == null)
                throw new ArgumentNullException(nameof(activityLogType));

            _activityLogTypeRepository.Insert(activityLogType);

            //删除缓存
            _cacheManager.RemoveByPattern(ACTIVITYTYPE_PATTERN_KEY);
        }

        /// <summary>
        /// 更新活动日志类型
        /// </summary>
        /// <param name="activityLogType">活动日志类型</param>
        public virtual void UpdateActivityType(ActivityLogType activityLogType)
        {
            if (activityLogType == null)
                throw new ArgumentNullException(nameof(activityLogType));

            _activityLogTypeRepository.Update(activityLogType);
            _cacheManager.RemoveByPattern(ACTIVITYTYPE_PATTERN_KEY);
        }

        /// <summary>
        /// 删除活动日志类型
        /// </summary>
        /// <param name="activityLogType">活动日志类型</param>
        public virtual void DeleteActivityType(ActivityLogType activityLogType)
        {
            if (activityLogType == null)
                throw new ArgumentNullException(nameof(activityLogType));

            _activityLogTypeRepository.Delete(activityLogType);
            _cacheManager.RemoveByPattern(ACTIVITYTYPE_PATTERN_KEY);
        }

        /// <summary>
        /// 获取所有活动日志类型
        /// </summary>
        /// <returns>活动日志类型列表</returns>
        public virtual IList<ActivityLogType> GetAllActivityTypes()
        {
            var query = from alt in _activityLogTypeRepository.Table
                        orderby alt.Name
                        select alt;
            var activityLogTypes = query.ToList();
            return activityLogTypes;
        }

        /// <summary>
        /// 通过标识符获取活动日志类型
        /// </summary>
        /// <param name="activityLogTypeId">活动日志类型标识符</param>
        /// <returns>活动日志类型</returns>
        public virtual ActivityLogType GetActivityTypeById(long activityLogTypeId)
        {
            if (activityLogTypeId == 0)
                return null;

            return _activityLogTypeRepository.GetById(activityLogTypeId);
        }

        /// <summary>
        /// 添加活动日志
        /// </summary>
        /// <param name="systemKeyword">类型关键词</param>
        /// <param name="comment">活动注释</param>
        /// <param name="commentParams">活动注释格式化参数</param>
        /// <returns>活动日志</returns>
        public virtual ActivityLog InsertActivity(string systemKeyword, string comment, params object[] commentParams)
        {
            return InsertActivity(_workContext.CurrentCustomer, systemKeyword, comment, commentParams);
        }

        /// <summary>
        /// 添加活动日志
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="systemKeyword">类型关键词</param>
        /// <param name="comment">活动注释</param>
        /// <param name="commentParams">活动注释格式化参数</param>
        /// <returns>活动日志</returns>
        public virtual ActivityLog InsertActivity(Customer customer, string systemKeyword, string comment, params object[] commentParams)
        {
            if (customer == null)
                return null;

            var activityTypes = GetAllActivityTypesCached();
            var activityType = activityTypes.ToList().Find(at => at.SystemKeyword == systemKeyword);
            if (activityType == null || !activityType.Enabled)
                return null;

            comment = CommonHelper.EnsureNotNull(comment);
            comment = string.Format(comment, commentParams);
            comment = CommonHelper.EnsureMaximumLength(comment, 4000);

            var activity = new ActivityLog();
            activity.ActivityLogTypeId = activityType.Id;
            activity.Customer = customer;
            activity.Comment = comment;
            activity.CreatedOnUtc = DateTime.UtcNow;
            activity.IpAddress = _webHelper.GetCurrentIpAddress();

            _activityLogRepository.Insert(activity);

            return activity;
        }

        /// <summary>
        /// 删除活动日志
        /// </summary>
        /// <param name="activityLog">活动日志</param>
        public void DeleteActivity(ActivityLog activityLog)
        {
            if (activityLog == null)
                throw new ArgumentNullException(nameof(activityLog));

            if (_logSettings.IgnoreSoftDelete)
            {
                //此处关系到系统存储与性能,将直接进行硬删除
                //日志删除不为修改基类BaseEntity.IsDelete字段,直接从数据库中删除该数据
                _dbContext.Set<ActivityLog>().Remove(activityLog);
            }
            else
            {
                _activityLogRepository.Delete(activityLog);
            }
        }

        /// <summary>
        /// 通过条件获取活动日志列表
        /// </summary>
        /// <param name="createdOnFrom">该UTC时间后创建的日志; NULL为不限制</param>
        /// <param name="createdOnTo">该UTC时间前创建的日志; NULL为不限制</param>
        /// <param name="customerId">用户标识符; NULL为不限制</param>
        /// <param name="activityLogTypeId">活动日志类型标识符</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="ipAddress">IP地址; NULL为不限制</param>
        /// <returns>活动日志列表</returns>
        public virtual IPagedList<ActivityLog> GetAllActivities(DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, long? customerId = null, long activityLogTypeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, string ipAddress = null)
        {
            var query = _activityLogRepository.Table;
            if (!String.IsNullOrEmpty(ipAddress))
                query = query.Where(al => al.IpAddress.Contains(ipAddress));
            if (createdOnFrom.HasValue)
                query = query.Where(al => createdOnFrom.Value <= al.CreatedOnUtc);
            if (createdOnTo.HasValue)
                query = query.Where(al => createdOnTo.Value >= al.CreatedOnUtc);
            if (activityLogTypeId > 0)
                query = query.Where(al => activityLogTypeId == al.ActivityLogTypeId);
            if (customerId.HasValue)
                query = query.Where(al => customerId.Value == al.CustomerId);

            query = query.OrderByDescending(al => al.CreatedOnUtc);

            var activityLog = new PagedList<ActivityLog>(query, pageIndex, pageSize);
            return activityLog;
        }

        /// <summary>
        /// 通过标识符获取活动日志
        /// </summary>
        /// <param name="activityLogId">活动日志标识符</param>
        /// <returns>活动日志</returns>
        public virtual ActivityLog GetActivityById(long activityLogId)
        {
            if (activityLogId == 0)
                return null;

            return _activityLogRepository.GetById(activityLogId);
        }

        /// <summary>
        /// 清空活动日志
        /// </summary>
        public virtual void ClearAllActivities()
        {
            var activityLog = _activityLogRepository.Table.ToList();
            foreach (var activityLogItem in activityLog)
                DeleteActivity(activityLogItem);
        }

        #endregion

        #region Utilities

        protected virtual IList<ActivityLogTypeForCaching> GetAllActivityTypesCached()
        {
            string key = string.Format(ACTIVITYTYPE_ALL_KEY);
            return _cacheManager.Get(key, () =>
            {
                var result = new List<ActivityLogTypeForCaching>();
                var activityLogTypes = GetAllActivityTypes();
                foreach (var alt in activityLogTypes)
                {
                    var altForCaching = new ActivityLogTypeForCaching
                    {
                        Id = alt.Id,
                        SystemKeyword = alt.SystemKeyword,
                        Name = alt.Name,
                        Enabled = alt.Enabled
                    };
                    result.Add(altForCaching);
                }
                return result;
            });
        }

        #endregion

        #region Nested classes

        [Serializable]
        public class ActivityLogTypeForCaching
        {
            public long Id { get; set; }
            public string SystemKeyword { get; set; }
            public string Name { get; set; }
            public bool Enabled { get; set; }
        }

        #endregion

    }
}
