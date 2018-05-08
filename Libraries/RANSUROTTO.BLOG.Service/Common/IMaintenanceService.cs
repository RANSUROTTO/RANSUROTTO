using System.Collections.Generic;
using System.IO;

namespace RANSUROTTO.BLOG.Services.Common
{
    /// <summary>
    /// 备份服务业务接口
    /// </summary>
    public interface IMaintenanceService
    {

        /// <summary>
        /// 获取所有备份文件
        /// </summary>
        /// <returns>备份文件列表</returns>
        IList<FileInfo> GetAllBackupFiles();

        /// <summary>
        /// 为当前数据库创建备份
        /// </summary>
        void BackupDatabase();

        /// <summary>
        /// 从备份恢复数据库
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        void RestoreDatabase(string backupFileName);

        /// <summary>
        /// 返回备份文件的路径
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <returns>备份文件的路径</returns>
        string GetBackupPath(string backupFileName);

    }
}
