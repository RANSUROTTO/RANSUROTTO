using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Data.Context;

namespace RANSUROTTO.BLOG.Services.Common
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

            return Directory.GetFiles(path, "*.bak").Select(fullPath => new FileInfo(fullPath))
                .OrderByDescending(p => p.CreationTime).ToList();
        }

        /// <summary>
        /// 为当前数据库创建备份
        /// </summary>
        public virtual void BackupDatabase()
        {
            CheckBackupSupported();

            //这里应该考虑各种数据库拥有不同的备份形式。目前此处实现为 SQL Server 数据库备份功能
            var fileName = string.Format(
                "{0}database_{1:yyyy-MM-dd-HH-mm-ss}_{2}.bak",
                GetBackupDirectoryPath(), DateTime.Now, CommonHelper.GenerateRandomDigitCode(10));

            var commandText = string.Format(
                "BACKUP DATABASE [{0}] TO DISK = '{1}' WITH FORMAT",
                _dbContext.DbName(), fileName);

            _dbContext.ExecuteSqlCommand(commandText, true);
        }

        /// <summary>
        /// 从备份恢复数据库
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        public virtual void RestoreDatabase(string backupFileName)
        {
            CheckBackupSupported();
            var settings = new DataSettingsManager();
            var conn = new SqlConnectionStringBuilder(settings.LoadSettings().DataConnectionString)
            {
                InitialCatalog = "master"
            };

            //这里应该考虑各种数据库拥有不同的恢复备份形式。目前此处实现为 SQL Server 数据库恢复备份功能
            using (var sqlConnectiononn = new SqlConnection(conn.ToString()))
            {
                var commandText = string.Format(
                    "DECLARE @ErrorMessage NVARCHAR(4000)\n" +
                    "ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE\n" +
                    "BEGIN TRY\n" +
                    "RESTORE DATABASE [{0}] FROM DISK = '{1}' WITH REPLACE\n" +
                    "END TRY\n" +
                    "BEGIN CATCH\n" +
                    "SET @ErrorMessage = ERROR_MESSAGE()\n" +
                    "END CATCH\n" +
                    "ALTER DATABASE [{0}] SET MULTI_USER WITH ROLLBACK IMMEDIATE\n" +
                    "IF (@ErrorMessage is not NULL)\n" +
                    "BEGIN\n" +
                    "RAISERROR (@ErrorMessage, 16, 1)\n" +
                    "END",
                    _dbContext.DbName(),
                    backupFileName);

                DbCommand dbCommand = new SqlCommand(commandText, sqlConnectiononn);
                if (sqlConnectiononn.State != ConnectionState.Open)
                    sqlConnectiononn.Open();
                dbCommand.ExecuteNonQuery();
            }
            //清空连接池
            SqlConnection.ClearAllPools();
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
