using System;
using System.IO;
using System.Collections.Generic;

namespace RANSUROTTO.BLOG.Service.Common
{
    public class MaintenanceService : IMaintenanceService
    {

        public IList<FileInfo> GetAllBackupFiles()
        {
            throw new NotImplementedException();
        }

        public void BackupDatabase()
        {
            throw new NotImplementedException();
        }

        public void RestoreDatabase(string backupFileName)
        {
            throw new NotImplementedException();
        }

        public string GetBackupPath(string backupFileName)
        {
            throw new NotImplementedException();
        }

    }
}
