using System.Web.Mvc;
using System.Collections.Generic;
using FluentValidation.Attributes;
using RANSUROTTO.BLOG.Framework.Mvc;
using System.ComponentModel.DataAnnotations;
using RANSUROTTO.BLOG.Web.Validators.Install;

namespace RANSUROTTO.BLOG.Web.Models.Install
{
    [Validator(typeof(InstallValidator))]
    public class InstallModel : BaseModel
    {
        public InstallModel()
        {
            this.AvailableLanguages = new List<SelectListItem>();
        }

        [AllowHtml]
        public string AdminEmail { get; set; }

        [AllowHtml]
        [NoTrim]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }

        [AllowHtml]
        [NoTrim]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        [AllowHtml]
        public string DatabaseConnectionString { get; set; }
        public string DataProvider { get; set; }
        public bool DisableSqlCompact { get; set; }
        //SQL Server properties
        public string SqlConnectionInfo { get; set; }
        [AllowHtml]
        public string SqlServerName { get; set; }
        [AllowHtml]
        public string SqlDatabaseName { get; set; }
        [AllowHtml]
        public string SqlServerUsername { get; set; }
        [AllowHtml]
        public string SqlServerPassword { get; set; }
        public string SqlAuthenticationType { get; set; }
        public bool SqlServerCreateDatabase { get; set; }

        public bool UseCustomCollation { get; set; }
        [AllowHtml]
        public string Collation { get; set; }

        public bool DisableSampleDataOption { get; set; }
        public bool InstallSampleData { get; set; }

        public List<SelectListItem> AvailableLanguages { get; set; }
    }
}