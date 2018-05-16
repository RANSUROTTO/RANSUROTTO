using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Security;

namespace RANSUROTTO.BLOG.Services.Security
{
    /// <summary>
    /// 权限提供商
    /// </summary>
    public interface IPermissionProvider
    {

        /// <summary>
        /// 获取权限项列表
        /// </summary>
        /// <returns>权限项列表</returns>
        IEnumerable<PermissionRecord> GetPermissions();

        /// <summary>
        /// 获取默认权限角色列表
        /// </summary>
        /// <returns>权限角色列表</returns>
        IEnumerable<DefaultPermissionRecord> GetDefaultPermissions();

    }
}
