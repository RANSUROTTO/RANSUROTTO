using System;
using System.Linq;
using System.Data.Entity;
using System.Transactions;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace RANSUROTTO.BLOG.Data.Initializers
{
    public class MySQL_CreateTablesIfNotExist<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
    {

        private readonly string[] _tablesToValidate;
        private readonly string[] _customCommands;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tablesToValidate">标识需要验证现有表表名列表</param>
        /// <param name="customCommands">自定义需要执行SQL脚本文件路径列表</param>
        public MySQL_CreateTablesIfNotExist(string[] tablesToValidate, string[] customCommands)
        {
            this._tablesToValidate = tablesToValidate;
            this._customCommands = customCommands;
        }

        public void InitializeDatabase(TContext context)
        {
            bool dbExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }
            if (dbExists)
            {
                bool createTables;
                var dataBaseName = new MySqlConnectionStringBuilder(context.Database.Connection.ConnectionString).Database;

                if (_tablesToValidate != null && _tablesToValidate.Length > 0)
                {
                    var existingTableNames = new List<string>(context.Database.SqlQuery<string>($"SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_SCHEMA=\"{dataBaseName}\""));
                    createTables = !existingTableNames.Intersect(_tablesToValidate, StringComparer.InvariantCultureIgnoreCase).Any();
                }
                else
                {
                    int numberOfTables = 0;
                    foreach (var t1 in context.Database.SqlQuery<int>($"SELECT COUNT(1) FROM information_schema.TABLES WHERE TABLE_SCHEMA=\"{dataBaseName}\""))
                        numberOfTables = t1;

                    createTables = numberOfTables == 0;
                }

                if (createTables)
                {
                    var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                    context.Database.ExecuteSqlCommand(dbCreationScript);

                    context.SaveChanges();

                    if (_customCommands != null && _customCommands.Length > 0)
                    {
                        foreach (var command in _customCommands)
                            if (!string.IsNullOrEmpty(command))
                                context.Database.ExecuteSqlCommand(command);
                    }
                }
            }
            else
            {
                throw new ApplicationException("不存在数据库实例");
            }
        }

    }
}
