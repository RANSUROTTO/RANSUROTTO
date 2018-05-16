using System;
using System.Collections.Generic;
using System.Linq;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Logging;
using RANSUROTTO.BLOG.Core.Domain.Logging.Enum;
using RANSUROTTO.BLOG.Core.Domain.Logging.Setting;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Data.Context;

namespace RANSUROTTO.BLOG.Services.Logging
{
    public class Logger : ILogger
    {

        #region Fields

        private readonly IRepository<Log> _logRepository;
        private readonly IWebHelper _webHelper;
        private readonly IDbContext _dbContext;
        private readonly LogSettings _logSettings;

        #endregion

        #region Constructor

        public Logger(IRepository<Log> logRepository, IWebHelper webHelper, IDbContext dbContext, LogSettings logSettings)
        {
            _logRepository = logRepository;
            _webHelper = webHelper;
            _dbContext = dbContext;
            _logSettings = logSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="log">日志</param>
        public virtual void DeleteLog(Log log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));
            
                _logRepository.Delete(log);
        }

        /// <summary>
        /// 删除多个日志对象
        /// </summary>
        /// <param name="logs">日志集合</param>
        public virtual void DeleteLogs(IList<Log> logs)
        {
            if (logs == null)
                throw new ArgumentNullException(nameof(logs));

            foreach (var log in logs)
                DeleteLog(log);
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        public virtual void ClearLog()
        {
            var logs = _logRepository.Table.ToList();
            DeleteLogs(logs);
        }

        /// <summary>
        /// 通过指定条件获取日志列表
        /// </summary>
        /// <param name="fromUtc">该UTC时间后创建的日志; NULL为不限制</param>
        /// <param name="toUtc">该UTC时间前创建的日志; NULL为不限制</param>
        /// <param name="message">消息</param>
        /// <param name="logLevel">日志级别; NULL为不限制</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>日志列表</returns>
        public virtual IPagedList<Log> GetAllLogs(DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = "", LogLevel? logLevel = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _logRepository.Table;
            if (fromUtc.HasValue)
                query = query.Where(l => fromUtc.Value <= l.CreatedOnUtc);
            if (toUtc.HasValue)
                query = query.Where(l => toUtc.Value >= l.CreatedOnUtc);
            if (logLevel.HasValue)
            {
                var logLevelId = (int)logLevel.Value;
                query = query.Where(l => logLevelId == l.LogLevelId);
            }
            if (!string.IsNullOrEmpty(message))
                query = query.Where(l => l.ShortMessage.Contains(message) || l.FullMessage.Contains(message));

            query = query.OrderByDescending(l => l.CreatedOnUtc);

            var log = new PagedList<Log>(query, pageIndex, pageSize);
            return log;
        }

        /// <summary>
        /// 通过标识符获取日志
        /// </summary>
        /// <param name="logId">日志标识符</param>
        /// <returns>日志</returns>
        public virtual Log GetLogById(long logId)
        {
            if (logId == 0)
                return null;

            return _logRepository.GetById(logId);
        }

        /// <summary>
        /// 通过标识符列表获取日志列表
        /// </summary>
        /// <param name="logIds">日志标识符列表</param>
        /// <returns>日志列表</returns>
        public virtual IList<Log> GetLogByIds(long[] logIds)
        {
            if (logIds == null || logIds.Length == 0)
                return new List<Log>();

            var query = from l in _logRepository.Table
                        where logIds.Contains(l.Id)
                        select l;
            var logItems = query.ToList();

            //按查询标识符列表的顺序进行排序
            var sortedLogItems = new List<Log>();
            foreach (long id in logIds)
            {
                var log = logItems.Find(x => x.Id == id);
                if (log != null)
                    sortedLogItems.Add(log);
            }
            return sortedLogItems;
        }

        /// <summary>
        /// 插入一条日志
        /// </summary>
        /// <param name="logLevel">日志等级</param>
        /// <param name="shortMessage">简短消息</param>
        /// <param name="fullMessage">完整消息</param>
        /// <param name="customer">触发日志记录的用户</param>
        /// <returns>日志</returns>
        public virtual Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", Customer customer = null)
        {
            //检查该日志消息是否存在忽略列表中
            if (IgnoreLog(shortMessage) || IgnoreLog(fullMessage))
                return null;

            var log = new Log
            {
                LogLevel = logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                IpAddress = _webHelper.GetCurrentIpAddress(),
                Customer = customer,
                PageUrl = _webHelper.GetThisPageUrl(true),
                ReferrerUrl = _webHelper.GetUrlReferrer()
            };

            _logRepository.Insert(log);

            return log;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 获取一个值,该值指示是否不应记录此消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>结果</returns>
        protected virtual bool IgnoreLog(string message)
        {
            if (!_logSettings.IgnoreLogWordlist.Any())
                return false;

            if (String.IsNullOrWhiteSpace(message))
                return false;

            return _logSettings
                .IgnoreLogWordlist
                .Any(x => message.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        #endregion

    }
}
