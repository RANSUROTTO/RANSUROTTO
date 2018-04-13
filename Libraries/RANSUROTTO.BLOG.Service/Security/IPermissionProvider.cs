using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Security;

namespace RANSUROTTO.BLOG.Service.Security
{
    /// <summary>
    /// 权限提供商
    /// </summary>
    public interface IPermissionProvider
    {

        IEnumerable<PermissionRecord> GetPermissions();



    }
}
