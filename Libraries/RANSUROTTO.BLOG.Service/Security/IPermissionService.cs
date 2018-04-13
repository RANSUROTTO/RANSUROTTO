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

        /// <summary>
        /// 通过标识符获取权限项
        /// </summary>
        /// <param name="permissionId">权限项标识符</param>
        /// <returns>权限项</returns>
        PermissionRecord GetPermissionRecordById(long permissionId);

        /// <summary>
        /// 通过系统名称获取权限项
        /// </summary>
        /// <param name="systemName">权限项系统名称</param>
        /// <returns>权限项</returns>
        PermissionRecord GetPermissionRecordBySystemName(string systemName);

        /// <summary>
        /// 获取所有权限项
        /// </summary>
        /// <returns>权限项列表</returns>
        IList<PermissionRecord> GetAllPermissionRecords();

        /// <summary>
        /// 添加权限项
        /// </summary>
        /// <param name="permission">权限项</param>
        void InsertPermissionRecord(PermissionRecord permission);

        /// <summary>
        /// 更新权限项
        /// </summary>
        /// <param name="permission">权限项</param>
        void UpdatePermissionRecord(PermissionRecord permission);

        /// <summary>
        /// 删除权限项
        /// </summary>
        /// <param name="permission">权限项</param>
        void DeletePermissionRecord(PermissionRecord permission);

        /// <summary>
        /// 安装权限
        /// </summary>
        /// <param name="permissionProvider">权限提供商</param>
        void InstallPermissions(IPermissionProvider permissionProvider);

        /// <summary>
        /// 卸载权限
        /// </summary>
        /// <param name="permissionProvider">权限提供商</param>
        void UninstallPermissions(IPermissionProvider permissionProvider);

        /// <summary>
        /// 检查当前用户是否可通过权限项
        /// </summary>
        /// <param name="permission">权限项</param>
        /// <returns>结果</returns>
        bool Authorize(PermissionRecord permission);

        /// <summary>
        /// 检查指定用户是否可通过权限项
        /// </summary>
        /// <param name="permission">权限项</param>
        /// <param name="customer">用户</param>
        /// <returns>结果</returns>
        bool Authorize(PermissionRecord permission, Customer customer);

        /// <summary>
        /// 检查当前用户是否可通过指定系统名称的权限项
        /// </summary>
        /// <param name="permissionRecordSystemName">权限项的系统名称</param>
        /// <returns>结果</returns>
        bool Authorize(string permissionRecordSystemName);

        /// <summary>
        /// 检查指定用户是否可通过指定系统名称的权限项
        /// </summary>
        /// <param name="permissionRecordSystemName">权限项的系统名称</param>
        /// <param name="customer">用户</param>
        /// <returns>结果</returns>
        bool Authorize(string permissionRecordSystemName, Customer customer);

    }
}
