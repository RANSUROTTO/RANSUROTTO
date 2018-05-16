using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Security;

namespace RANSUROTTO.BLOG.Services.Security
{
    /// <summary>
    /// 默认的系统权限角色
    /// </summary>
    public class DefaultPermissionRecord
    {

        public DefaultPermissionRecord()
        {
            PermissionRecords = new List<PermissionRecord>();
        }

        /// <summary>
        /// 权限角色系统名称
        /// </summary>
        public string CustomerRoleSystemName { get; set; }

        /// <summary>
        /// 权限项列表
        /// </summary>
        public IEnumerable<PermissionRecord> PermissionRecords { get; set; }

    }
}
