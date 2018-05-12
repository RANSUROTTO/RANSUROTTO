using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;

namespace RANSUROTTO.BLOG.Core.Domain.Customers.Setting
{
    public class CustomerSettings : ISettings
    {

        /// <summary>
        /// 获取或设置当前使用的登录类型
        /// </summary>
        public AuthenticationType CurrentAuthenticationType { get; set; }

        /// <summary>
        /// 获取或设置默认密码处理格式
        /// </summary>
        public PasswordFormat DefaultPasswordFormat { get; set; }

        /// <summary>
        /// 获取或设置用户密码使用哈希加密时使用的哈希算法名
        /// </summary>
        public string HashedPasswordFormat { get; set; }

        /// <summary>
        /// 获取或设置用户登录时允许密码输入错误的次数,超过后将锁定用户,0为不限制
        /// </summary>
        public int FailedPasswordAllowedAttempts { get; set; }

        /// <summary>
        /// 获取或设置用户单次被锁定的时间(单位:分/min)
        /// </summary>
        public int FailedPasswordLockoutMinutes { get; set; }

        /// <summary>
        /// 获取或设置标识修改密码时不应与前一个密码相同；如果允许客户可以同时使用相同的密码，则为0。
        /// </summary>
        public int UnduplicatedPasswordsNumber { get; set; }

        /// <summary>
        /// 获取或设置用户"生日"属性开启状态
        /// </summary>
        public bool DateOfBirthEnabled { get; set; }

        /// <summary>
        /// 获取或设置用户"公司"属性开启状态
        /// </summary>
        public bool CompanyEnabled { get; set; }
        /// <summary>
        /// 获取或设置用户"公司"属性是否为必填项
        /// </summary>
        public bool CompanyRequired { get; set; }

        /// <summary>
        /// 获取或设置用户"手机号"属性开启状态
        /// </summary>
        public bool PhoneEnabled { get; set; }
        /// <summary>
        /// 获取或设置用户"手机号"属性是否为必填项
        /// </summary>
        public bool PhoneRequired { get; set; }

        /// <summary>
        /// 获取或设置用户"性别"属性开启状态
        /// </summary>
        public bool GenderEnabled { get; set; }

    }
}
