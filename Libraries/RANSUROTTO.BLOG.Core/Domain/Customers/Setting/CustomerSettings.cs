﻿using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;

namespace RANSUROTTO.BLOG.Core.Domain.Customers.Setting
{
    public class CustomerSettings : ISettings
    {

        public UserRegistrationType UserRegistrationType { get; set; }

        public bool NotifyNewCustomerRegistration { get; set; }

        public int PasswordMinLength { get; set; }

        public CustomerNameFormat CustomerNameFormat { get; set; }

        public bool AllowCustomersToUploadAvatars { get; set; }

        public bool DefaultAvatarEnabled { get; set; }

        public bool AllowViewingProfiles { get; set; }

        public bool ShowCustomersLocation { get; set; }

        public bool ShowCustomersJoinDate { get; set; }

        /// <summary>
        /// 获取或设置当前使用的登录类型
        /// </summary>
        public AuthenticationType CustomerAuthenticationType { get; set; }

        public bool AllowUsersToChangeUsernames { get; set; }

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
        /// 获取或设置标识修改密码时不应与指定限制数前的密码相同；如果允许客户可以同时使用相同的密码，则为0。
        /// </summary>
        public int UnduplicatedPasswordsNumber { get; set; }

        /// <summary>
        /// 获取或设置"最近活跃用户"的最后活动时间的限制分钟数
        /// </summary>
        public int OnlineCustomerMinutes { get; set; }

        /// <summary>
        /// 获取或设置是否开启记录用户最后访问页面的标识
        /// </summary>
        public bool StoreLastVisitedPage { get; set; }

        /// <summary>
        /// 获取或设置用户"生日"属性开启状态
        /// </summary>
        public bool DateOfBirthEnabled { get; set; }

        public bool DateOfBirthRequired { get; set; }

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

        public bool CityEnabled { get; set; }

        public bool CityRequired { get; set; }

        public bool CountryEnabled { get; set; }

        public bool CountryRequired { get; set; }

        public bool StateProvinceEnabled { get; set; }

        public bool StateProvinceRequired { get; set; }

        /// <summary>
        /// 获取或设置用户"性别"属性开启状态
        /// </summary>
        public bool GenderEnabled { get; set; }

        public bool AcceptPrivacyPolicyEnabled { get; set; }

    }
}
