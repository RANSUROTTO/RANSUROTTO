using System;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Framework.Localization;

namespace RANSUROTTO.BLOG.Admin.Models.Common
{
    public class SystemInfoModel : BaseModel
    {
        public SystemInfoModel()
        {
            this.ServerVariables = new List<ServerVariableModel>();
            this.LoadedAssemblies = new List<LoadedAssemblyModel>();
        }

        /// <summary>
        /// ASP.NET版本信息
        /// </summary>
        [ResourceDisplayName("Admin.System.SystemInfo.ASPNETInfo")]
        public string AspNetInfo { get; set; }

        /// <summary>
        /// 完全信任级别
        /// </summary>
        [ResourceDisplayName("Admin.System.SystemInfo.IsFullTrust")]
        public string IsFullTrust { get; set; }

        /// <summary>
        /// RANSUROTTO.BLOG版本
        /// </summary>
        [ResourceDisplayName("Admin.System.SystemInfo.RansurottoVersion")]
        public string RansurottoVersion { get; set; }

        /// <summary>
        /// 操作系统信息
        /// </summary>
        [ResourceDisplayName("Admin.System.SystemInfo.OperatingSystem")]
        public string OperatingSystem { get; set; }

        /// <summary>
        /// 服务器的本地时间
        /// </summary>
        [ResourceDisplayName("Admin.System.SystemInfo.ServerLocalTime")]
        public DateTime ServerLocalTime { get; set; }

        /// <summary>
        /// 服务器时区
        /// </summary>
        [ResourceDisplayName("Admin.System.SystemInfo.ServerTimeZone")]
        public string ServerTimeZone { get; set; }

        /// <summary>
        /// (UTC)世界协调时间
        /// </summary>
        [ResourceDisplayName("Admin.System.SystemInfo.UTCTime")]
        public DateTime UtcTime { get; set; }

        /// <summary>
        /// 用户区域时间
        /// </summary>
        [ResourceDisplayName("Admin.System.SystemInfo.CurrentUserTime")]
        public DateTime CurrentUserTime { get; set; }

        /// <summary>
        /// HTTP_HOST
        /// </summary>
        [ResourceDisplayName("Admin.System.SystemInfo.HTTPHOST")]
        public string HttpHost { get; set; }

        [ResourceDisplayName("Admin.System.SystemInfo.ServerVariables")]
        public IList<ServerVariableModel> ServerVariables { get; set; }

        [ResourceDisplayName("Admin.System.SystemInfo.LoadedAssemblies")]
        public IList<LoadedAssemblyModel> LoadedAssemblies { get; set; }

        /// <summary>
        /// 服务器变量
        /// </summary>
        public partial class ServerVariableModel : BaseModel
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        /// <summary>
        /// 已加载的程序集
        /// </summary>
        public partial class LoadedAssemblyModel : BaseModel
        {
            public string FullName { get; set; }
            public string Location { get; set; }
            public bool IsDebug { get; set; }
            public DateTime? BuildDate { get; set; }
        }
    }
}