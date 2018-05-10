using System;
using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Messages
{
    /// <summary>
    /// 代表一个邮箱账号
    /// </summary>
    public class EmailAccount : BaseEntity
    {

        /// <summary>
        /// 获取或设置电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 获取或设置主机名
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 获取或设置端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 获取或设置用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置是否启用SSL
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// 获取或设置是否使用默认SSL凭证
        /// </summary>
        public bool UseDefaultCredentials { get; set; }

        /// <summary>
        /// 获取电子邮箱账户的友好名称
        /// </summary>
        public string FriendlyName
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(this.DisplayName))
                    return this.Email + " (" + this.DisplayName + ")";
                return this.Email;
            }
        }

    }
}
