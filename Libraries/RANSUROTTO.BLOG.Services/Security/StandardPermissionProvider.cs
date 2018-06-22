using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;
using RANSUROTTO.BLOG.Core.Domain.Security;

namespace RANSUROTTO.BLOG.Services.Security
{
    public class StandardPermissionProvider : IPermissionProvider
    {

        #region Properties

        /*Admin area permission*/
        public static readonly PermissionRecord AccessAdminPanel
            = new PermissionRecord { Name = "Access admin area", SystemName = "AccessAdminPanel", Category = "Standard" };
        public static readonly PermissionRecord HtmlEditorManagePictures
            = new PermissionRecord { Name = "Admin area. HTML Editor. Manage pictures", SystemName = "HtmlEditor.ManagePictures", Category = "Configuration" };

        /*Public area permission*/
        public static readonly PermissionRecord PublicAllowNavigation
            = new PermissionRecord { Name = "Public area,Allow navigation", SystemName = "PublicAllowNavigation", Category = "Public" };

        #endregion

        #region Methods

        /// <summary>
        /// 获取权限项列表
        /// </summary>
        /// <returns>权限项列表</returns>
        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                AccessAdminPanel
            };
        }

        /// <summary>
        /// 获取默认权限角色列表
        /// </summary>
        /// <returns>权限角色列表</returns>
        public virtual IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            return new[]
            {
                new DefaultPermissionRecord
                {
                    CustomerRoleSystemName = SystemCustomerRoleNames.Administrators,
                    PermissionRecords = new[]
                    {
                        AccessAdminPanel
                    }
                },
                new DefaultPermissionRecord
                {
                    CustomerRoleSystemName = SystemCustomerRoleNames.Registered,
                    PermissionRecords = new[]
                    {
                        PublicAllowNavigation
                    }
                },
                new DefaultPermissionRecord
                {
                    CustomerRoleSystemName = SystemCustomerRoleNames.Guests,
                    PermissionRecords = new[]
                    {
                        PublicAllowNavigation
                    }
                }
            };
        }

        #endregion

    }
}
