using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Security;

namespace RANSUROTTO.BLOG.Service.Security
{
    /// <summary>
    /// 权限业务层接口
    /// </summary>
    public interface IPermissionService
    {

        PermissionRecord GetPermissionRecordById(int permissionId);

        PermissionRecord GetPermissionRecordBySystemName(string systemName);

        IList<PermissionRecord> GetAllPermissionRecords();

        void InsertPermissionRecord(PermissionRecord permission);

        void UpdatePermissionRecord(PermissionRecord permission);

        void InstallPermissions(IPermissionProvider permissionProvider);

        void UninstallPermissions(IPermissionProvider permissionProvider);

        bool Authorize(PermissionRecord permission);

        bool Authorize(PermissionRecord permission, Customer customer);

        bool Authorize(string permissionRecordSystemName);

        bool Authorize(string permissionRecordSystemName, Customer customer);

    }
}
