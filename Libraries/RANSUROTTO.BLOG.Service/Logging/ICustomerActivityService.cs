using System;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Logging;

namespace RANSUROTTO.BLOG.Service.Logging
{

    /// <summary>
    /// 用户活动业务层接口
    /// </summary>
    public interface ICustomerActivityService
    {

        /// <summary>
        /// 插入活动日志类型
        /// </summary>
        /// <param name="activityLogType">活动日志类型</param>
        void InsertActivityType(ActivityLogType activityLogType);

        /// <summary>
        /// 更新活动日志类型
        /// </summary>
        /// <param name="activityLogType">活动日志类型</param>
        void UpdateActivityType(ActivityLogType activityLogType);

        /// <summary>
        /// 删除活动日志类型
        /// </summary>
        /// <param name="activityLogType">活动日志类型</param>
        void DeleteActivityType(ActivityLogType activityLogType);

        /// <summary>
        /// 获取所有活动日志类型
        /// </summary>
        /// <returns>活动日志类型列表</returns>
        IList<ActivityLogType> GetAllActivityTypes();

        /// <summary>
        /// 通过标识符获取活动日志类型
        /// </summary>
        /// <param name="activityLogTypeId">活动日志类型标识符</param>
        /// <returns>活动日志类型</returns>
        ActivityLogType GetActivityTypeById(long activityLogTypeId);

        /// <summary>
        /// 添加活动日志
        /// </summary>
        /// <param name="systemKeyword">类型关键词</param>
        /// <param name="comment">活动注释</param>
        /// <param name="commentParams">活动注释格式化参数</param>
        /// <returns>活动日志</returns>
        ActivityLog InsertActivity(string systemKeyword, string comment, params object[] commentParams);

        /// <summary>
        /// 添加活动日志
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="systemKeyword">类型关键词</param>
        /// <param name="comment">活动注释</param>
        /// <param name="commentParams">活动注释格式化参数</param>
        /// <returns>活动日志</returns>
        ActivityLog InsertActivity(Customer customer, string systemKeyword, string comment, params object[] commentParams);

        /// <summary>
        /// 删除活动日志
        /// </summary>
        /// <param name="activityLog">活动日志</param>
        void DeleteActivity(ActivityLog activityLog);

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
        IPagedList<ActivityLog> GetAllActivities(DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, long? customerId = null, long activityLogTypeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, string ipAddress = null);

        /// <summary>
        /// 通过标识符获取活动日志
        /// </summary>
        /// <param name="activityLogId">活动日志标识符</param>
        /// <returns>活动日志</returns>
        ActivityLog GetActivityById(long activityLogId);

        /// <summary>
        /// 清空活动日志
        /// </summary>
        void ClearAllActivities();

    }
}
