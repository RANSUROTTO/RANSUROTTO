using System;
using System.Web.Mvc;
using System.Threading;
using System.Data.SqlClient;
using System.Security.Principal;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Data.Provider;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Framework.Security;
using RANSUROTTO.BLOG.Web.Models.Install;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Services.Installation;
using RANSUROTTO.BLOG.Web.Infrastructure.Installation;

namespace RANSUROTTO.BLOG.Web.Controllers
{
    public class InstallController : BasePublicController
    {

        #region Fields

        private readonly WebConfig _config;
        private readonly IInstallationLocalizationService _locService;

        #endregion

        #region Properties

        /// <summary>
        /// 指示是否开启MARS(多个活动结果集)
        /// </summary>
        protected virtual bool UseMars
        {
            get { return false; }
        }

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
                InstallSampleData = false,
                DatabaseConnectionString = "",
                DataProvider = "sqlserver",
                //fast installation service does not support SQL compact
                DisableSampleDataOption = _config.DisableSampleDataDuringInstallation,
                SqlAuthenticationType = "sqlauthentication",
                SqlConnectionInfo = "sqlconnectioninfo_values",
                SqlServerCreateDatabase = false,
                UseCustomCollation = false,
                Collation = "SQL_Latin1_General_CP1_CI_AS"
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

            //SQL Server
            if (model.DataProvider.Equals("sqlserver", StringComparison.InvariantCultureIgnoreCase))
            {
                if (model.SqlConnectionInfo.Equals("sqlconnectioninfo_raw", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.IsNullOrEmpty(model.DatabaseConnectionString))
                        ModelState.AddModelError("", _locService.GetResource("ConnectionStringRequired"));

                    try
                    {
                        new SqlConnectionStringBuilder(model.DatabaseConnectionString);
                    }
                    catch
                    {
                        ModelState.AddModelError("", _locService.GetResource("ConnectionStringWrongFormat"));
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(model.SqlServerName))
                        ModelState.AddModelError("", _locService.GetResource("SqlServerNameRequired"));
                    if (string.IsNullOrEmpty(model.SqlDatabaseName))
                        ModelState.AddModelError("", _locService.GetResource("DatabaseNameRequired"));

                    if (model.SqlAuthenticationType.Equals("sqlauthentication", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(model.SqlServerUsername))
                            ModelState.AddModelError("", _locService.GetResource("SqlServerUsernameRequired"));
                        if (string.IsNullOrEmpty(model.SqlServerPassword))
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
                    if (model.DataProvider.Equals("sqlserver", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //SQL Server
                        if (model.SqlConnectionInfo.Equals("sqlconnectioninfo_raw", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var sqlCsb = new SqlConnectionStringBuilder(model.DatabaseConnectionString);
                            if (this.UseMars)
                            {
                                sqlCsb.MultipleActiveResultSets = true;
                            }
                            connectionString = sqlCsb.ToString();
                        }
                        else
                        {
                            connectionString = CreateConnectionString(model.SqlAuthenticationType == "windowsauthentication",
                                model.SqlServerName, model.SqlDatabaseName,
                                model.SqlServerUsername, model.SqlServerPassword);
                        }

                        if (model.SqlServerCreateDatabase)
                        {
                            if (!SqlServerDatabaseExists(connectionString))
                            {
                                //create database
                                var collation = model.UseCustomCollation ? model.Collation : "";
                                var errorCreatingDatabase = CreateDatabase(connectionString, collation);
                                if (!String.IsNullOrEmpty(errorCreatingDatabase))
                                    throw new Exception(errorCreatingDatabase);
                            }
                        }
                        else
                        {
                            if (!SqlServerDatabaseExists(connectionString))
                                throw new Exception(_locService.GetResource("DatabaseNotExists"));
                        }
                    }
                    else
                    {
                        //SQL CE
                        string databaseFileName = "Ransurotto.Db.sdf";
                        string databasePath = @"|DataDirectory|\" + databaseFileName;
                        connectionString = "Data Source=" + databasePath + ";Persist Security Info=False";

                        string databaseFullPath = CommonHelper.MapPath("~/App_Data/") + databaseFileName;
                        if (System.IO.File.Exists(databaseFullPath))
                        {
                            System.IO.File.Delete(databaseFullPath);
                        }
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

                    //添加数据
                    var installationService = EngineContext.Current.Resolve<IInstallationService>();
                    installationService.InstallData(model.AdminEmail, model.AdminPassword, model.InstallSampleData);

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

                    var cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("ransurotto_cache_static");
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

            //更新当前语言
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
        /// 创建SQL Server数据库连接字符串
        /// </summary>
        /// <param name="trustedConnection">使用可信连接</param>
        /// <param name="serverName">数据库服务实例名</param>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="userName">连接用户名</param>
        /// <param name="password">连接密码</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>结果</returns>
        [NonAction]
        protected virtual string CreateConnectionString(bool trustedConnection,
            string serverName, string databaseName,
            string userName, string password, int timeout = 0)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.IntegratedSecurity = trustedConnection;
            builder.DataSource = serverName;
            builder.InitialCatalog = databaseName;
            if (!trustedConnection)
            {
                builder.UserID = userName;
                builder.Password = password;
            }
            builder.PersistSecurityInfo = false;
            if (this.UseMars)
            {
                builder.MultipleActiveResultSets = true;
            }
            if (timeout > 0)
            {
                builder.ConnectTimeout = timeout;
            }
            return builder.ConnectionString;
        }

        /// <summary>
        /// 检查SQL Server数据库是否存在
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>结果</returns>
        [NonAction]
        public virtual bool SqlServerDatabaseExists(string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
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
        /// 创建SQL Server数据库
        /// </summary>
        /// <param name="connectionString">SQL Server连接字符串</param>
        /// <param name="collation">SQL Server排序规则</param>
        /// <param name="triesToConnect">
        /// 尝试连接至数据库的次数
        /// 如果超过次数无法正常连接,则返回错误
        /// 输入0跳过验证
        /// </param>
        /// <returns></returns>
        [NonAction]
        public virtual string CreateDatabase(string connectionString, string collation, int triesToConnect = 10)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                var databaseName = builder.InitialCatalog;
                //对SQL Server数据库系统表'master'进行操作
                builder.InitialCatalog = "master";
                var masterCatalogConnectionString = builder.ToString();
                string query = string.Format("CREATE DATABASE [{0}]", databaseName);
                if (!String.IsNullOrWhiteSpace(collation))
                    query = string.Format("{0} COLLATE {1}", query, collation);
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

                        if (!this.SqlServerDatabaseExists(connectionString))
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
        }

        #endregion

    }
}