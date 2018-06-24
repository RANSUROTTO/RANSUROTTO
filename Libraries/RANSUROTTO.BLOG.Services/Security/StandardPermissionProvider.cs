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
        /*Admin area 'Content Management' permission*/
        public static readonly PermissionRecord ManageCategories
            = new PermissionRecord { Name = "Admin area. Manage Categories", SystemName = "ManageCategories", Category = "Content Management" };
        public static readonly PermissionRecord ManageBlogposts
            = new PermissionRecord { Name = "Admin area. Manage Blog posts", SystemName = "ManageBlogposts", Category = "Content Management" };
        public static readonly PermissionRecord ManageIdea
            = new PermissionRecord { Name = "Admin area. Manage Idea", SystemName = "ManageIdea", Category = "Content Management" };
        public static readonly PermissionRecord ManageBlogTags
            = new PermissionRecord { Name = "Admin area. Manage Blog Tags", SystemName = "ManageBlogTags", Category = "Content Management" };
        /*Admin area 'Customers' permission*/
        public static readonly PermissionRecord ManageCustomers
            = new PermissionRecord { Name = "Admin area. Manage Customers", SystemName = "ManageCustomers", Category = "Customers" };
        /*Admin area 'Configuration' permission*/
        public static readonly PermissionRecord ManageLanguages
            = new PermissionRecord { Name = "Admin area. Manage Languages", SystemName = "ManageLanguages", Category = "Configuration" };
        public static readonly PermissionRecord ManageSettings
            = new PermissionRecord { Name = "Admin area. Manage Settings", SystemName = "ManageSettings", Category = "Configuration" };
        public static readonly PermissionRecord ManageActivityLog
            = new PermissionRecord { Name = "Admin area. Manage Activity Log", SystemName = "ManageActivityLog", Category = "Configuration" };
        public static readonly PermissionRecord ManageAcl
            = new PermissionRecord { Name = "Admin area. Manage ACL", SystemName = "ManageACL", Category = "Configuration" };
        public static readonly PermissionRecord ManageEmailAccounts
            = new PermissionRecord { Name = "Admin area. Manage Email Accounts", SystemName = "ManageEmailAccounts", Category = "Configuration" };
        public static readonly PermissionRecord ManageSystemLog
            = new PermissionRecord { Name = "Admin area. Manage System Log", SystemName = "ManageSystemLog", Category = "Configuration" };
        public static readonly PermissionRecord ManageMaintenance
            = new PermissionRecord { Name = "Admin area. Manage Maintenance", SystemName = "ManageMaintenance", Category = "Configuration" };
        public static readonly PermissionRecord HtmlEditorManagePictures
            = new PermissionRecord { Name = "Admin area. HTML Editor. Manage pictures", SystemName = "HtmlEditor.ManagePictures", Category = "Configuration" };
        public static readonly PermissionRecord ManageScheduleTasks
            = new PermissionRecord { Name = "Admin area. Manage Schedule Tasks", SystemName = "ManageScheduleTasks", Category = "Configuration" };

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
                AccessAdminPanel,
                ManageCategories,
                ManageBlogposts,
                ManageIdea,
                ManageBlogTags,
                ManageCustomers,
                ManageLanguages,
                ManageSettings,
                ManageActivityLog,
                ManageAcl,
                ManageEmailAccounts,
                ManageSystemLog,
                ManageMaintenance,
                HtmlEditorManagePictures,
                ManageScheduleTasks,
                PublicAllowNavigation
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
                        AccessAdminPanel,
                        ManageCategories,
                        ManageBlogposts,
                        ManageIdea,
                        ManageBlogTags,
                        ManageCustomers,
                        ManageLanguages,
                        ManageSettings,
                        ManageActivityLog,
                        ManageAcl,
                        ManageEmailAccounts,
                        ManageSystemLog,
                        ManageMaintenance,
                        HtmlEditorManagePictures,
                        ManageScheduleTasks,
                        PublicAllowNavigation
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
