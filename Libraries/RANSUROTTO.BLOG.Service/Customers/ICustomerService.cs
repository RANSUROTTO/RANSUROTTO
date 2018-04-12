using System;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;

namespace RANSUROTTO.BLOG.Service.Customers
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
        Customer GetCustomerById(long customerId);

        /// <summary>
        /// 通过标识符列表获取用户列表
        /// </summary>
        /// <param name="customerIds">用户标识符列表</param>
        /// <returns>用户列表</returns>
        IList<Customer> GetCustomersByIds(long[] customerIds);

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
        /// 更新用户
        /// </summary>
        /// <param name="customer">用户</param>
        void UpdateCustomer(Customer customer);

        #endregion

        #region Customer roles



        #endregion

        #region Customer passwords

        /// <summary>
        /// 获取用户的密码列表
        /// </summary>
        /// <param name="customerId">用户标识符; null为不限制</param>
        /// <param name="passwordFormat">密码格式化类型; null为不限制</param>
        /// <param name="passwordsToReturn">返回的记录数量; null为不限制</param>
        /// <returns>用户密码列表</returns>
        IList<CustomerPassword> GetCustomerPasswords(long? customerId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null);

        /// <summary>
        /// 通过用户标识符获取对应用户当前的密码
        /// </summary>
        /// <param name="customerId">用户标识符</param>
        /// <returns>用户密码</returns>
        CustomerPassword GetCurrentPassword(long customerId);

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
