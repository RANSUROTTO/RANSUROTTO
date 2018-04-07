using System.Data.Common;
using System.Data.Entity;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using RANSUROTTO.BLOG.Core.Data;

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
            //TODO 创建数据库表及其相关配置
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

    }

}
