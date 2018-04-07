using System;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Data.Provider;
using RANSUROTTO.BLOG.Framework.Security;
using RANSUROTTO.BLOG.Web.Infrastructure.Installation;
using RANSUROTTO.BLOG.Web.Models.Install;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    public class InstallController : BasePublicController
    {

        #region Fields

        private readonly WebConfig _config;
        private readonly IInstallationLocalizationService _locService;

        #endregion

        #region Constructor

        public InstallController(WebConfig config, IInstallationLocalizationService locService)
        {
            _config = config;
            _locService = locService;
        }

        #endregion

        #region Methods

        public virtual ActionResult Index()
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            //设置页面超时时间为5分钟
            this.Server.ScriptTimeout = 300;

            var model = new InstallModel
            {
                AdminEmail = "admin@ransurotto.com",
                DataProvider = "mysql",
                MySqlAuthenticationType = "sqlauthentication",
                MySqlConnectionInfo = "sqlconnectioninfo_values",
                DisableSampleDataOption = _config.DisableSampleDataDuringInstallation,
            };

            foreach (var lang in _locService.GetAvailableLanguages())
            {
                model.AvailableLanguages.Add(new SelectListItem
                {
                    Value = Url.Action("ChangeLanguage", new { language = lang.Code }),
                    Text = lang.Name,
                    Selected = _locService.GetCurrentLanguage().Code == lang.Code,
                });
            }

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Index(InstallModel model)
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            //设置页面超时时间为5分钟
            this.Server.ScriptTimeout = 300;

            if (model.DatabaseConnectionString != null)
                model.DatabaseConnectionString = model.DatabaseConnectionString.Trim();

            foreach (var lang in _locService.GetAvailableLanguages())
            {
                model.AvailableLanguages.Add(new SelectListItem
                {
                    Value = Url.Action("ChangeLanguage", "Install", new { language = lang.Code }),
                    Text = lang.Name,
                    Selected = _locService.GetCurrentLanguage().Code == lang.Code,
                });
            }

            model.DisableSampleDataOption = _config.DisableSampleDataDuringInstallation;

            //MySQL
            if (model.DataProvider.Equals("mysql", StringComparison.InvariantCultureIgnoreCase))
            {
                if (model.MySqlConnectionInfo.Equals("sqlconnectioninfo_raw",
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.IsNullOrEmpty(model.DatabaseConnectionString))
                        ModelState.AddModelError("", _locService.GetResource("ConnectionStringRequired"));

                    try
                    {
                        new MySqlConnectionStringBuilder(model.DatabaseConnectionString);
                    }
                    catch
                    {
                        ModelState.AddModelError("", _locService.GetResource("ConnectionStringWrongFormat"));
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(model.MySqlServerName))
                        ModelState.AddModelError("", _locService.GetResource("SqlServerNameRequired"));
                    if (string.IsNullOrEmpty(model.MySqlDatabaseName))
                        ModelState.AddModelError("", _locService.GetResource("DatabaseNameRequired"));

                    if (model.MySqlAuthenticationType.Equals("sqlauthentication",
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(model.MySqlServerUsername))
                            ModelState.AddModelError("", _locService.GetResource("SqlServerUsernameRequired"));
                        if (string.IsNullOrEmpty(model.MySqlServerPassword))
                            ModelState.AddModelError("", _locService.GetResource("SqlServerPasswordRequired"));
                    }
                }
            }

            //验证是否有特定文件夹与文件操作权限
            var dirsToCheck = FilePermissionHelper.GetDirectoriesWrite();
            foreach (string dir in dirsToCheck)
                if (!FilePermissionHelper.CheckPermissions(dir, false, true, true, false))
                    ModelState.AddModelError("", string.Format(_locService.GetResource("ConfigureDirectoryPermissions"), WindowsIdentity.GetCurrent().Name, dir));

            var filesToCheck = FilePermissionHelper.GetFilesWrite();
            foreach (string file in filesToCheck)
                if (!FilePermissionHelper.CheckPermissions(file, false, true, true, true))
                    ModelState.AddModelError("", string.Format(_locService.GetResource("ConfigureFilePermissions"), WindowsIdentity.GetCurrent().Name, file));

            if (ModelState.IsValid)
            {
                var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                var settingsManager = new DataSettingsManager();
                try
                {
                    string connectionString;
                    //MySQL
                    if (model.MySqlConnectionInfo.Equals("sqlconnectioninfo_raw",
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        var sqlCsb = new MySqlConnectionStringBuilder(model.DatabaseConnectionString);
                        connectionString = sqlCsb.ToString();
                    }
                    else
                    {
                        connectionString = CreateMySqlConnectionString(model.MySqlServerName,
                            model.MySqlDatabaseName, model.MySqlServerUsername, model.MySqlServerPassword);
                    }

                    if (model.MySqlServerCreateDatabase)
                    {
                        if (!MySqlServerDatabaseExists(connectionString))
                        {
                            //创建数据库
                            var errorCreatingDatabase = CreateMySqlDatabase(connectionString);
                            if (!string.IsNullOrEmpty(errorCreatingDatabase))
                                throw new Exception(errorCreatingDatabase);
                        }
                    }
                    else
                    {
                        //检查到数据库不存在(未勾选不存在则创建选项)
                        if (!MySqlServerDatabaseExists(connectionString))
                            throw new Exception(_locService.GetResource("DatabaseNotExists"));
                    }

                    //保存数据库连接设定
                    var dataProvider = model.DataProvider;
                    var settings = new DataSettings
                    {
                        DataProvider = dataProvider,
                        DataConnectionString = connectionString
                    };
                    settingsManager.SaveSettings(settings);

                    //初始化数据提供商
                    var dataProviderInstance = EngineContext.Current.Resolve<BaseDataProviderManager>().LoadDataProvider();
                    dataProviderInstance.InitDatabase();

                    //TODO 添加测试数据

                    //TODO 安装插件

                    //TODO 添加权限

                    //清空缓存
                    DataSettingsHelper.ResetCache();

                    //重启应用程序
                    webHelper.RestartAppDomain();

                    //跳转至主页
                    return RedirectToRoute("HomePage");
                }
                catch (Exception ex)
                {
                    DataSettingsHelper.ResetCache();

                    var cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("Ransurotto_cache_static");
                    cacheManager.Clear();

                    settingsManager.SaveSettings(new DataSettings
                    {
                        DataProvider = null,
                        DataConnectionString = null
                    });

                    ModelState.AddModelError("", string.Format(_locService.GetResource("SetupFailed"), ex.Message));
                }
            }
            return View(model);
        }

        public virtual ActionResult ChangeLanguage(string language)
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            //Update current language
            _locService.SaveCurrentLanguage(language);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public virtual ActionResult RestartInstall()
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
                return RedirectToRoute("HomePage");

            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            webHelper.RestartAppDomain();

            return RedirectToRoute("HomePage");
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 创建数据库连接字符串
        /// </summary>
        /// <param name="serverName">数据库服务实例名</param>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="userName">连接用户名</param>
        /// <param name="password">连接密码</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>结果</returns>
        [NonAction]
        protected virtual string CreateMySqlConnectionString(
            string serverName, string databaseName,
            string userName, string password, uint timeout = 0)
        {
            var builder = new MySqlConnectionStringBuilder();

            builder.IntegratedSecurity = false;
            //builder.Port = 3306;
            builder.Server = serverName;
            builder.Database = databaseName;
            builder.UserID = userName;
            builder.Password = password;

            builder.PersistSecurityInfo = false;

            if (timeout > 0)
            {
                builder.ConnectionTimeout = timeout;
            }

            return builder.ConnectionString;
        }

        /// <summary>
        /// 检查MySQL数据库是否存在
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>结果</returns>
        [NonAction]
        public virtual bool MySqlServerDatabaseExists(string connectionString)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建MySQL数据库
        /// </summary>
        /// <param name="connectionString">MySQL连接字符串</param>
        /// <param name="triesToConnect"></param>
        /// <returns></returns>
        [NonAction]
        public virtual string CreateMySqlDatabase(string connectionString, int triesToConnect = 10)
        {
            try
            {
                //解析数据库名称
                var builder = new MySqlConnectionStringBuilder(connectionString);
                var databaseName = builder.Database;
                //对MySQL数据库系统表'mysql'进行操作
                builder.Database = "mysql";
                var masterCatalogConnectionString = builder.ToString();

                string query = $"CREATE DATABASE `{databaseName}`";

                using (var conn = new SqlConnection(masterCatalogConnectionString))
                {
                    conn.Open();
                    using (var command = new SqlCommand(query, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                //尝试连接
                if (triesToConnect > 0)
                {
                    for (var i = 0; i <= triesToConnect; i++)
                    {
                        if (i == triesToConnect)
                            throw new Exception("无法连接到新数据库,请重试或手动检查。");

                        if (!this.MySqlServerDatabaseExists(connectionString))
                            Thread.Sleep(1000);
                        else
                            break;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format(_locService.GetResource("DatabaseCreationError"), ex.Message);
            }
            return null;
        }

        #endregion

    }
}