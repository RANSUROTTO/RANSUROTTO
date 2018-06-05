using System.Web.Mvc;
using FluentValidation.Attributes;
using RANSUROTTO.BLOG.Admin.Validators.Settings;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Settings
{
    [Validator(typeof(SettingValidator))]
    public class SettingModel : BaseEntityModel
    {

        [ResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Value")]
        [AllowHtml]
        public string Value { get; set; }

    }
}