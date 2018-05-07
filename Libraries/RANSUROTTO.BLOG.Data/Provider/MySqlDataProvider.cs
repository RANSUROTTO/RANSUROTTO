using System;
using System.IO;
using MySql.Data.Entity;
using System.Data.Entity;
using System.Data.Common;
using MySql.Data.MySqlClient;
using RANSUROTTO.BLOG.Core.Data;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Data.Context;
using RANSUROTTO.BLOG.Data.Initializers;

namespace RANSUROTTO.BLOG.Data.Provider
{

    public class MySqlDataProvider : IDataProvider
    {

        #region Properties

        /// <summary>
        /// MySql数据库支持存储过程
        /// </summary>
        public virtual bool StoredProceduredSupported
        {
            get { return true; }
        }

        /// <summary>
        /// MySql数据库支持备份
        /// </summary>
        public virtual bool BackupSupported
        {
            get { return true; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 初始化MySql数据库连接工厂
        /// </summary>
        public virtual void InitConnectionFactory()
        {
            var connectionFactory = new MySqlConnectionFactory();
#pragma warning disable 618
            Database.DefaultConnectionFactory = connectionFactory;
#pragma warning restore 618
        }

        /// <summary>
        /// 初始化MySql数据库设置
        /// </summary>
        public virtual void SetDatabaseInitializer()
        {

        }

        /// <summary>
        /// 初始化MySql数据库
        /// </summary>
        public virtual void InitDatabase()
        {

        }

        public virtual DbParameter GetParameter()
        {
            return new MySqlParameter();
        }

        public virtual int SupportedLengthOfBinaryHash()
        {
            return 0;
        }

        #endregion

    }

}
