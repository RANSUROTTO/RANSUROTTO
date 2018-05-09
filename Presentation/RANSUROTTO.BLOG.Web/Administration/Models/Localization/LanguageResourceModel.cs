using System.Web.Mvc;
using FluentValidation.Attributes;
using RANSUROTTO.BLOG.Admin.Validators.Localization;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Localization
{
    [Validator(typeof(LanguageResourceValidator))]
    public class LanguageResourceModel : BaseEntityModel
    {

        [ResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [ResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Value")]
        [AllowHtml]
        public string Value { get; set; }

        public long LanguageId { get; set; }
    }
}