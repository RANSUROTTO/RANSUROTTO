using System;
using System.Linq;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;
using RANSUROTTO.BLOG.Core.Domain.Customers.Setting;
using RANSUROTTO.BLOG.Service.Events;

namespace RANSUROTTO.BLOG.Service.Customers
{
    public class CustomerService : ICustomerService
    {

        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly CustomerSettings _customerSettings;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerPassword> _customerPasswordRepository;

        #endregion

        #region Constructor

        public CustomerService(ICacheManager cacheManager, CustomerSettings customerSettings, IEventPublisher eventPublisher, IRepository<Customer> customerRepository, IRepository<CustomerPassword> customerPasswordRepository)
        {
            _cacheManager = cacheManager;
            _customerSettings = customerSettings;
            _eventPublisher = eventPublisher;
            _customerRepository = customerRepository;
            _customerPasswordRepository = customerPasswordRepository;
        }

        #endregion

        #region Customer

        /// <summary>
        /// 通过标识符获取用户
        /// </summary>
        /// <param name="customerId">用户标识符</param>
        /// <returns>用户</returns>
        public virtual Customer GetCustomerById(long customerId)
        {
            if (customerId == 0)
                return null;

            return _customerRepository.GetById(customerId);
        }

        /// <summary>
        /// 通过标识符列表获取用户列表
        /// </summary>
        /// <param name="customerIds">用户标识符列表</param>
        /// <returns>用户列表</returns>
        public virtual IList<Customer> GetCustomersByIds(long[] customerIds)
        {
            if (customerIds == null || customerIds.Length == 0)
                return new List<Customer>();

            var query = from c in _customerRepository.Table
                        where customerIds.Contains(c.Id)
                        select c;
            var customers = query.ToList();

            //按查询ID列表进行排序
            var sortedCustomers = new List<Customer>();
            foreach (long id in customerIds)
            {
                var customer = customers.Find(x => x.Id == id);
                if (customer != null)
                    sortedCustomers.Add(customer);
            }
            return sortedCustomers;
        }

        /// <summary>
        /// 通过系统名称获取用户
        /// </summary>
        /// <param name="systemName">用户系统名称</param>
        /// <returns>用户</returns>
        public virtual Customer GetCustomerBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.SystemName == systemName
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        /// <summary>
        /// 通过GUID获取用户
        /// </summary>
        /// <param name="customerGuid">用户GUID</param>
        /// <returns>用户</returns>
        public virtual Customer GetCustomerByGuid(Guid customerGuid)
        {
            if (customerGuid == Guid.Empty)
                return null;

            var query = from c in _customerRepository.Table
                        where c.Guid == customerGuid
                        orderby c.Id
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        /// <summary>
        /// 通过用户名/Email获取用户
        /// </summary>
        /// <param name="usernameOrEmail">用户名/Email</param>
        /// <returns>用户</returns>
        public Customer GetCustomerByUsernameOrEmail(string usernameOrEmail)
        {
            if (string.IsNullOrEmpty(usernameOrEmail))
                return null;

            return usernameOrEmail.IndexOf('@') == -1
                ? GetCustomerByUsername(usernameOrEmail)
                : GetCustomerByEmail(usernameOrEmail);
        }

        /// <summary>
        /// 通过Email获取用户
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>用户</returns>
        public virtual Customer GetCustomerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.Email == email
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        /// <summary>
        /// 通过用户名获取用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户</returns>
        public virtual Customer GetCustomerByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.Username == username
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="customer">用户</param>
        public virtual void InsertCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            _customerRepository.Insert(customer);

            //发布插入通知
            _eventPublisher.EntityInserted(customer);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="customer">用户</param>
        public virtual void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            _customerRepository.Update(customer);

            //发布更新通知
            _eventPublisher.EntityUpdated(customer);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="customer">用户</param>
        public virtual void DeleteCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (!String.IsNullOrEmpty(customer.Email))
                customer.Email += "-DELETED";
            if (!String.IsNullOrEmpty(customer.Username))
                customer.Username += "-DELETED";

            _customerRepository.Delete(customer);

            //发布删除通知
            _eventPublisher.EntityDeleted(customer);
        }

        #endregion

        #region Customer Password

        /// <summary>
        /// 获取用户的密码列表
        /// </summary>
        /// <param name="customerId">用户标识符; null为不限制</param>
        /// <param name="passwordFormat">密码格式化类型; null为不限制</param>
        /// <param name="passwordsToReturn">返回的记录数量; null为不限制</param>
        /// <returns>用户密码列表</returns>
        public virtual IList<CustomerPassword> GetCustomerPasswords(long? customerId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null)
        {
            var query = _customerPasswordRepository.Table;

            if (customerId.HasValue)
                query = query.Where(password => password.CustomerId == customerId.Value);
            if (passwordFormat.HasValue)
                query = query.Where(password => password.PasswordFormatId == (int)passwordFormat.Value);
            if (passwordsToReturn.HasValue)
                query = query.OrderByDescending(password => password.CreateDateUtc).Take(passwordsToReturn.Value);

            return query.ToList();
        }

        /// <summary>
        /// 通过用户标识符获取对应用户当前的密码
        /// </summary>
        /// <param name="customerId">用户标识符</param>
        /// <returns>用户密码</returns>
        public virtual CustomerPassword GetCurrentPassword(long customerId)
        {
            if (customerId == 0)
                return null;

            return GetCustomerPasswords(customerId, passwordsToReturn: 1).FirstOrDefault();
        }

        /// <summary>
        /// 添加用户密码
        /// </summary>
        /// <param name="customerPassword">用户密码</param>
        public virtual void InsertCustomerPassword(CustomerPassword customerPassword)
        {
            if (customerPassword == null)
                throw new ArgumentNullException(nameof(customerPassword));

            _customerPasswordRepository.Insert(customerPassword);

            //发布添加通知
            _eventPublisher.EntityInserted(customerPassword);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="customerPassword">用户密码</param>
        public virtual void UpdateCustomerPassword(CustomerPassword customerPassword)
        {
            if (customerPassword == null)
                throw new ArgumentNullException(nameof(customerPassword));

            _customerPasswordRepository.Update(customerPassword);

            //发布更新通知
            _eventPublisher.EntityUpdated(customerPassword);
        }

        #endregion

    }
}
