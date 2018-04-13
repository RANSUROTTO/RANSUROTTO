using System;
using System.Linq;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Customers
{

    /// <summary>
    /// 用户
    /// </summary>
    public class Customer : BaseEntity
    {

        private ICollection<CustomerRole> _customerRoles;

        /// <summary>
        /// 获取或设置用户名/登录名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置电子邮箱是否需要再次验证
        /// </summary>
        public bool EmailToRevalidate { get; set; }

        /// <summary>
        /// 获取或设置系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 获取或设置标识该用户是否为系统账户
        /// </summary>
        public bool IsSystemAccount { get; set; }

        /// <summary>
        /// 获取或设置该用户已连续尝试登陆失败次数 (密码错误)
        /// </summary>
        public int FailedLoginAttempts { get; set; }

        /// <summary>
        /// 获取或设置该用户在指定UTC时间之前拒绝登陆
        /// </summary>
        public DateTime? CannotLoginUntilDateUtc { get; set; }

        /// <summary>
        /// 获取或设置该用户是否为可用状态
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// 获取或设置最后访问/操作时的IP地址
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// 获取或设置最后访问/操作时的UTC时间
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        /// <summary>
        /// 获取或设置最后登陆时的UTC时间
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// 获取或设置管理员对该用户的注释信息
        /// </summary>
        public string AdminComment { get; set; }

        #region Navigation Properties

        /// <summary>
        /// 获取或设置用户权限角色
        /// </summary>
        public virtual ICollection<CustomerRole> CustomerRoles
        {
            get { return _customerRoles?.Where(p => !p.IsDeleted).ToList() ?? (_customerRoles = new List<CustomerRole>()); }
            protected set { _customerRoles = value; }
        }

        #endregion

    }

}
