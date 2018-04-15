using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Web.Models.Customer
{
    public class LoginModel : BaseModel
    {

        /// <summary>
        /// 登录名 / 用户名 / 邮箱账号
        /// </summary>
        [ResourceDisplayName("Account.Login.Fields.LoginName")]
        [AllowHtml]
        public string LoginName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [DataType(DataType.Password)]
        [NoTrim]
        [ResourceDisplayName("Account.Login.Fields.Password")]
        [AllowHtml]
        public string Password { get; set; }

        /// <summary>
        /// 记住我选项
        /// </summary>
        [ResourceDisplayName("Account.Login.Fields.RememberMe")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// 标识是否需要显示验证码
        /// </summary>
        public bool DisplayCaptcha { get; set; }

    }
}