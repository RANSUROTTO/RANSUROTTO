using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;

namespace RANSUROTTO.BLOG.Core.Domain.Customers
{

    /// <summary>
    /// 用户密码
    /// </summary>
    public class CustomerPassword : BaseEntity
    {

        /// <summary>
        /// 获取或设置用户ID
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 获取或设置密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置密码格式化类型ID
        /// </summary>
        public int PasswordFormatId { get; set; }

        /// <summary>
        /// 获取或设置密码盐
        /// </summary>
        public string PasswordSalt { get; set; }

        #region Navigation Properties

        /// <summary>
        /// 获取或设置密码格式化类型
        /// </summary>
        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)this.PasswordFormatId; }
            set { PasswordFormatId = (int)value; }
        }

        /// <summary>
        /// 获取或设置用户
        /// </summary>
        public virtual Customer Customer { get; set; }

        #endregion

    }

}
