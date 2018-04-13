using System.Linq;
using RANSUROTTO.BLOG.Core.Data;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Core.Domain.Security
{
    /// <summary>
    /// 权限项
    /// </summary>
    public class PermissionRecord : BaseEntity
    {
        private ICollection<CustomerRole> _customerRoles;

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 获取或设置类目分组
        /// </summary>
        public string Category { get; set; }

        #region Navigation Properties

        /// <summary>
        /// 获取或设置使用角色
        /// </summary>
        public virtual ICollection<CustomerRole> CustomerRoles
        {
            get
            {
                return _customerRoles?.Where(p => !p.IsDeleted).ToList() ?? (_customerRoles = new List<CustomerRole>());
            }
            protected set { _customerRoles = value; }
        }

        #endregion

    }
}
