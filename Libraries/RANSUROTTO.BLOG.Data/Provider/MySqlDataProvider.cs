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
            var tablesToValidate = new string[] { };

            var customCommands = new List<string>();
            customCommands.Add(ParseCommands(CommonHelper.MapPath("~/App_Data/Install/SqlServer.Indexes.sql"), false));
            customCommands.Add(ParseCommands(CommonHelper.MapPath("~/App_Data/Install/SqlServer.StoredProcedures.sql"), false));

            var initializer = new MySQL_CreateTablesIfNotExist<EntityContext>(tablesToValidate, customCommands.ToArray());
            Database.SetInitializer(initializer);
        }

        /// <summary>
        /// 初始化MySql数据库
        /// </summary>
        public virtual void InitDatabase()
        {
            InitConnectionFactory();
            SetDatabaseInitializer();
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

        #region Utilities

        /// <summary>
        /// 将指定SQL脚本文件转换为SQL脚本字符串对象
        /// </summary>
        /// <param name="filePath">SQL脚本文件路径</param>
        /// <param name="throwExceptionIfNonExists">文件未找到时是否抛出异常</param>
        /// <returns></returns>
        protected virtual string ParseCommands(string filePath, bool throwExceptionIfNonExists)
        {
            if (!File.Exists(filePath))
            {
                if (throwExceptionIfNonExists)
                    throw new ArgumentException($"指定的文件不存在 - {filePath}");

                return string.Empty;
            }

            string command;

            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            {
                command = reader.ReadToEnd();
            }
            return command;
        }

        #endregion

    }

}
