using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Web.Validators.Install;

namespace RANSUROTTO.BLOG.Web.Models.Install
{
    [Validator(typeof(InstallValidator))]
    public class InstallModel : BaseModel
    {

        public InstallModel()
        {
            AvailableLanguages = new List<SelectListItem>();
        }

        /// <summary>
        /// 管理员电子邮箱
        /// </summary>
        [AllowHtml]
        public string AdminEmail { get; set; }

        /// <summary>
        /// 管理员密码
        /// </summary>
        [AllowHtml]
        [NoTrim]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }

        /// <summary>
        /// 确认管理员密码
        /// </summary>
        [AllowHtml]
        [NoTrim]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 数据库原始连接字符串
        /// </summary>
        [AllowHtml]
        public string DatabaseConnectionString { get; set; }
        /// <summary>
        /// 数据库提供商[可选值:MySQL]
        /// </summary>
        public string DataProvider { get; set; }
        /// <summary>
        /// 连接字符串配置模式[可选值:sqlconnectioninfo_values配置连接字符串|sqlconnectioninfo_raw输入原始连接字符串]
        /// </summary>
        public string MySqlConnectionInfo { get; set; }
        /// <summary>
        /// mysql实例名
        /// </summary>
        [AllowHtml]
        public string MySqlServerName { get; set; }
        /// <summary>
        /// mysql数据库名称
        /// </summary>
        [AllowHtml]
        public string MySqlDatabaseName { get; set; }
        /// <summary>
        /// 数据库验证类型[可选值:sqlauthentication使用mysql账户验证]
        /// </summary>
        public string MySqlAuthenticationType { get; set; }
        /// <summary>
        /// 数据库连接用户名
        /// </summary>
        [AllowHtml]
        public string MySqlServerUsername { get; set; }
        /// <summary>
        /// 数据库连接密码
        /// </summary>
        [AllowHtml]
        public string MySqlServerPassword { get; set; }
        /// <summary>
        /// 如果数据库不存在则创建
        /// </summary>
        public bool MySqlServerCreateDatabase { get; set; }

        public bool UseCustomCollation { get; set; }
        [AllowHtml]
        public string Collation { get; set; }

        /// <summary>
        /// 指示是否禁用"导入测试数据"选项
        /// </summary>
        public bool DisableSampleDataOption { get; set; }
        /// <summary>
        /// 标识是否需要"导入测试数据"的值
        /// </summary>
        public bool InstallSampleData { get; set; }

        /// <summary>
        /// 可用的安装页面语言列表
        /// </summary>
        public List<SelectListItem> AvailableLanguages { get; set; }

    }
}