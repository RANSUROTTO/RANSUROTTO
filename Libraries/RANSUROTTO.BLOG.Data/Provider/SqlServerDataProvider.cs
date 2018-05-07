using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Data.Context;
using RANSUROTTO.BLOG.Data.Initializers;

namespace RANSUROTTO.BLOG.Data.Provider
{

    /// <summary>
    /// SqlServer数据提供者
    /// </summary>
    public class SqlServerDataProvider : IDataProvider
    {

        #region Properties

        /// <summary>
        /// SqlServer数据库支持存储过程
        /// </summary>
        public virtual bool StoredProceduredSupported
        {
            get { return true; }
        }

        /// <summary>
        /// SqlServer数据库支持备份
        /// </summary>
        public virtual bool BackupSupported
        {
            get { return true; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 初始化SqlServer数据库连接创建工厂
        /// </summary>
        public virtual void InitConnectionFactory()
        {
            var connectionFactory = new SqlConnectionFactory();
#pragma warning disable 618
            Database.DefaultConnectionFactory = connectionFactory;
#pragma warning restore 618
        }

        /// <summary>
        /// 初始化SqlServer数据库设置
        /// </summary>
        public virtual void SetDatabaseInitializer()
        {
            var tablesToValidate = new string[] { };

            var customCommands = new List<string>();
            customCommands.AddRange(ParseCommands(CommonHelper.MapPath("~/App_Data/Install/SqlServer.Indexes.sql"), false));
            customCommands.AddRange(ParseCommands(CommonHelper.MapPath("~/App_Data/Install/SqlServer.StoredProcedures.sql"), false));

            var initializer = new CreateTablesIfNotExist<EntityContext>(tablesToValidate, customCommands.ToArray());
            Database.SetInitializer(initializer);
        }

        /// <summary>
        /// 初始化SqlServer数据库
        /// </summary>
        public virtual void InitDatabase()
        {
            InitConnectionFactory();
            SetDatabaseInitializer();
        }

        /// <summary>
        /// 获取支持数据库参数对象 (用于存储过程)
        /// </summary>
        /// <returns>参数对象</returns>
        public virtual DbParameter GetParameter()
        {
            return new SqlParameter();
        }

        public virtual int SupportedLengthOfBinaryHash()
        {
            return 800;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 将指定SQL脚本文件转换为SQL脚本字符串对象
        /// </summary>
        /// <param name="filePath">SQL脚本文件路径</param>
        /// <param name="throwExceptionIfNonExists">文件未找到时是否抛出异常</param>
        /// <returns></returns>
        protected virtual string[] ParseCommands(string filePath, bool throwExceptionIfNonExists)
        {
            if (!File.Exists(filePath))
            {
                if (throwExceptionIfNonExists)
                    throw new ArgumentException(string.Format("Specified file doesn't exist - {0}", filePath));

                return new string[0];
            }


            var statements = new List<string>();
            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            {
                string statement;
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                {
                    statements.Add(statement);
                }
            }

            return statements.ToArray();
        }

        protected virtual string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();

            while (true)
            {
                var lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                        return sb.ToString();

                    return null;
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion

    }

}
