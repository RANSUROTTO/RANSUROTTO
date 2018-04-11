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

    }

}
