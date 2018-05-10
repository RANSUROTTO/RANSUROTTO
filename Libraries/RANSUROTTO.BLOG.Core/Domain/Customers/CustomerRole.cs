using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Security;

namespace RANSUROTTO.BLOG.Core.Domain.Customers
{
    /// <summary>
    /// 权限角色
    /// </summary>
    public class CustomerRole : BaseEntity
    {
        private ICollection<PermissionRecord> _permissionRecords;

        /// <summary>
        /// 获取或设置权限角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置权限角色的系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 获取或设置权限角色是否处于可用状态
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// 获取或设置权限角色是否属于系统角色
        /// </summary>
        public bool IsSystemRole { get; set; }

        #region Navigation Properties

        /// <summary>
        /// 获取或设置权限记录
        /// </summary>
        public virtual ICollection<PermissionRecord> PermissionRecords
        {
            get { return _permissionRecords ?? new List<PermissionRecord>(); }
            protected set { _permissionRecords = value; }
        }

        #endregion

    }
}
