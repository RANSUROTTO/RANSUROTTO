using System;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;

namespace RANSUROTTO.BLOG.Services.Customers
{

    /// <summary>
    /// 用户业务层接口
    /// </summary>
    public interface ICustomerService
    {

        #region Customers

        /// <summary>
        /// 通过标识符获取用户
        /// </summary>
        /// <param name="customerId">用户标识符</param>
        /// <returns>用户</returns>
        Customer GetCustomerById(int customerId);

        /// <summary>
        /// 通过标识符列表获取用户列表
        /// </summary>
        /// <param name="customerIds">用户标识符列表</param>
        /// <returns>用户列表</returns>
        IList<Customer> GetCustomersByIds(int[] customerIds);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="createdFromUtc">该UTC时间后创建的用户;Null为不限制</param>
        /// <param name="createdToUtc">该UTC时间前创建的用户;Null为不限制</param>
        /// <param name="customerRoleIds">匹配客户角色标识符;Null为不限制</param>
        /// <param name="email">电子邮箱</param>
        /// <param name="username">用户名</param>
        /// <param name="name">姓名</param>
        /// <param name="dayOfBirth">出生日;0为不限制</param>
        /// <param name="monthOfBirth">出生月;0为不限制</param>
        /// <param name="company">公司</param>
        /// <param name="phone">手机号</param>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>用户列表</returns>
        IPagedList<Customer> GetAllCustomers(DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null, int[] customerRoleIds = null,
            string email = null, string username = null, string name = null,
            int dayOfBirth = 0, int monthOfBirth = 0, string company = null,
            string phone = null, string ipAddress = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 获取最后天数前活跃用户列表
        /// </summary>
        /// <param name="lastActivityFromUtc">用户最后活动日期（从）</param>
        /// <param name="customerRoleIds">通过用户角色标识符列表来匹配过滤;传递Null为不限制</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>用户列表</returns>
        IPagedList<Customer> GetOnlineCustomers(DateTime lastActivityFromUtc,
            int[] customerRoleIds, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 通过系统名称获取用户
        /// </summary>
        /// <param name="systemName">用户系统名称</param>
        /// <returns>用户</returns>
        Customer GetCustomerBySystemName(string systemName);

        /// <summary>
        /// 通过GUID获取用户
        /// </summary>
        /// <param name="customerGuid">用户GUID</param>
        /// <returns>用户</returns>
        Customer GetCustomerByGuid(Guid customerGuid);

        /// <summary>
        /// 通过用户名/Email获取用户
        /// </summary>
        /// <param name="usernameOrEmail">用户名/Email</param>
        /// <returns>用户</returns>
        Customer GetCustomerByUsernameOrEmail(string usernameOrEmail);

        /// <summary>
        /// 通过Email获取用户
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>用户</returns>
        Customer GetCustomerByEmail(string email);

        /// <summary>
        /// 通过用户名获取用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户</returns>
        Customer GetCustomerByUsername(string username);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="customer">用户</param>
        void DeleteCustomer(Customer customer);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="customer">用户</param>
        void InsertCustomer(Customer customer);

        /// <summary>
        /// 添加游客用户
        /// </summary>
        /// <returns>用户</returns>
        Customer InsertGuestCustomer();

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="customer">用户</param>
        void UpdateCustomer(Customer customer);

        #endregion

        #region Customer roles

        /// <summary>
        /// 通过标识符获取权限角色
        /// </summary>
        /// <param name="customerRoleId">权限角色标识符</param>
        /// <returns>权限角色</returns>
        CustomerRole GetCustomerRoleById(int customerRoleId);

        /// <summary>
        /// 通过系统名称获取用户权限角色
        /// </summary>
        /// <param name="systemName">角色系统名称</param>
        /// <returns>用户权限角色</returns>
        CustomerRole GetCustomerRoleBySystemName(string systemName);

        /// <summary>
        /// 获取所有权限角色
        /// </summary>
        /// <param name="showHidden">是否显示已隐藏的权限角色</param>
        /// <returns>权限角色</returns>
        IList<CustomerRole> GetAllCustomerRoles(bool showHidden = false);

        /// <summary>
        /// 添加权限角色
        /// </summary>
        /// <param name="customerRole">权限角色</param>
        void InsertCustomerRole(CustomerRole customerRole);

        /// <summary>
        /// 更新权限角色
        /// </summary>
        /// <param name="customerRole">权限角色</param>
        void UpdateCustomerRole(CustomerRole customerRole);

        /// <summary>
        /// 删除权限角色
        /// </summary>
        /// <param name="customerRole">权限角色</param>
        void DeleteCustomerRole(CustomerRole customerRole);

        #endregion

        #region Customer passwords

        /// <summary>
        /// 获取用户的密码列表
        /// </summary>
        /// <param name="customerId">用户标识符; null为不限制</param>
        /// <param name="passwordFormat">密码格式化类型; null为不限制</param>
        /// <param name="passwordsToReturn">返回的记录数量; null为不限制</param>
        /// <returns>用户密码列表</returns>
        IList<CustomerPassword> GetCustomerPasswords(int? customerId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null);

        /// <summary>
        /// 通过用户标识符获取对应用户当前的密码
        /// </summary>
        /// <param name="customerId">用户标识符</param>
        /// <returns>用户密码</returns>
        CustomerPassword GetCurrentPassword(int customerId);

        /// <summary>
        /// 添加用户密码
        /// </summary>
        /// <param name="customerPassword">用户密码</param>
        void InsertCustomerPassword(CustomerPassword customerPassword);

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="customerPassword">用户密码</param>
        void UpdateCustomerPassword(CustomerPassword customerPassword);

        #endregion

    }

}
