using System;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Service.Customers
{

    /// <summary>
    /// 用户业务层接口
    /// </summary>
    public interface ICustomerService
    {

        #region Customers

        /// <summary>
        /// 通过标识符列表获取用户列表
        /// </summary>
        /// <param name="customerIds">用户标识符列表</param>
        /// <returns>用户列表</returns>
        IList<Customer> GetCustomersByIds(int[] customerIds);

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
        /// 通过标识符获取用户
        /// </summary>
        /// <param name="customerId">用户标识符</param>
        /// <returns>用户</returns>
        Customer GetCustomerById(long customerId);

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



        #endregion

    }

}
