using System;
using System.Data.Common;
using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Data.Provider
{

    public class SqliteDataProvider : IDataProvider
    {
        public void InitConnectionFactory()
        {
            throw new NotImplementedException();
        }

        public void SetDatabaseInitializer()
        {
            throw new NotImplementedException();
        }

        public void InitDatabase()
        {
            throw new NotImplementedException();
        }

        public bool StoredProceduredSupported { get; }
        public bool BackupSupported { get; }
        public DbParameter GetParameter()
        {
            throw new NotImplementedException();
        }

        public int SupportedLengthOfBinaryHash()
        {
            throw new NotImplementedException();
        }
    }

}
