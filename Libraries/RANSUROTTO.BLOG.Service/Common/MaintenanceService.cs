using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Data.Context;

namespace RANSUROTTO.BLOG.Service.Common
{
    public class MaintenanceService : IMaintenanceService
    {

        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructor

        public MaintenanceService(HttpContextBase httpContext, IDataProvider dataProvider, IDbContext dbContext)
        {
            _httpContext = httpContext;
            _dataProvider = dataProvider;
            _dbContext = dbContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取所有备份文件
        /// </summary>
        /// <returns>备份文件列表</returns>
        public virtual IList<FileInfo> GetAllBackupFiles()
        {
            var path = GetBackupDirectoryPath();

            if (!Directory.Exists(path))
            {
                throw new IOException("备份文件夹不存在");
            }

            return Directory.GetFiles(path, "*.sql").Select(fullPath => new FileInfo(fullPath))
                .OrderByDescending(p => p.CreationTime).ToList();
        }

        public virtual void BackupDatabase()
        {
            CheckBackupSupported();

            //TODO 这里应该考虑各种数据库拥有不同的备份形式。目前此处实现为MYSQL数据库备份功能
            var fileName = string.Format("{0}database_{1:yyyy-MM-dd-HH-mm-ss}_{2}.sql",
                GetBackupDirectoryPath(), DateTime.Now, CommonHelper.GenerateRandomDigitCode(10));

            var commandText = string.Format("");

            _dbContext.ExecuteSqlCommand(commandText, true);
        }

        public virtual void RestoreDatabase(string backupFileName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回备份文件的路径
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <returns>备份文件的路径</returns>
        public virtual string GetBackupPath(string backupFileName)
        {
            return Path.Combine(GetBackupDirectoryPath(), backupFileName);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 返回备份文件所在文件夹路径
        /// </summary>
        /// <returns>备份文件所在文件夹路径</returns>
        protected virtual string GetBackupDirectoryPath()
        {
            return string.Format("{0}Administration\\db_backups\\", _httpContext.Request.PhysicalApplicationPath);
        }

        /// <summary>
        /// 检查该数据库是否支持备份操作
        /// </summary>
        protected virtual void CheckBackupSupported()
        {
            if (_dataProvider.BackupSupported) return;

            throw new DataException("此数据库不支持备份.");
        }

        #endregion

    }
}
