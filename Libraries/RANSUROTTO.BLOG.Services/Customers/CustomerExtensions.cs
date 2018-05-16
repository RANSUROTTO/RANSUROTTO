using System;
using System.Linq;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;

namespace RANSUROTTO.BLOG.Services.Customers
{
    public static class CustomerExtensions
    {

        /// <summary>
        /// 检查某个用户是否属于指定的权限角色
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="customerRoleSystemName">权限角色系统名称</param>
        /// <param name="onlyActiveCustomerRoles">仅查询可用的权限角色</param>
        /// <returns>结果</returns>
        public static bool IsInCustomerRole(this Customer customer,
            string customerRoleSystemName, bool onlyActiveCustomerRoles = true)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (string.IsNullOrEmpty(customerRoleSystemName))
                throw new ArgumentNullException(nameof(customerRoleSystemName));

            var result = customer.CustomerRoles
                             .FirstOrDefault(cr => (!onlyActiveCustomerRoles || cr.Active) && (cr.SystemName == customerRoleSystemName)) != null;
            return result;
        }

        /// <summary>
        /// 检查某个用户是否为搜索引擎账户用户
        /// </summary>
        /// <param name="customer">用户</param>
        /// <returns>结果</returns>
        public static bool IsSearchEngineAccount(this Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (!customer.IsSystemAccount || string.IsNullOrEmpty(customer.SystemName))
                return false;

            var result = customer.SystemName.Equals(SystemCustomerNames.SearchEngine, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// 检查某个用户是否为后台任务账户用户
        /// </summary>
        /// <param name="customer">用户</param>
        /// <returns>结果</returns>
        public static bool IsBackgroundTaskAccount(this Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (!customer.IsSystemAccount || string.IsNullOrEmpty(customer.SystemName))
                return false;

            var result = customer.SystemName.Equals(SystemCustomerNames.BackgroundTask, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// 检查某个用户是否为系统管理员角色
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="onlyActiveCustomerRoles">仅查询可用的权限角色</param>
        /// <returns>结果</returns>
        public static bool IsAdmin(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemCustomerRoleNames.Administrators, onlyActiveCustomerRoles);
        }

        /// <summary>
        /// 检查某个用户是否为注册用户角色
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="onlyActiveCustomerRoles">仅查询可用的权限角色</param>
        /// <returns>结果</returns>
        public static bool IsRegistered(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemCustomerRoleNames.Registered, onlyActiveCustomerRoles);
        }

        /// <summary>
        /// 检查某个用户是否为游客角色
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="onlyActiveCustomerRoles">仅查询可用的权限角色</param>
        /// <returns>结果</returns>
        public static bool IsGuest(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemCustomerRoleNames.Guests, onlyActiveCustomerRoles);
        }

    }
}
