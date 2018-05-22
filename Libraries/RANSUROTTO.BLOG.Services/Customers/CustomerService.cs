using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Common;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.AttributeName;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;
using RANSUROTTO.BLOG.Core.Domain.Customers.Setting;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Services.Events;

namespace RANSUROTTO.BLOG.Services.Customers
{
    public class CustomerService : ICustomerService
    {

        #region Constants

        /// <summary>
        /// 所有权限角色缓存键
        /// </summary>
        /// <remarks>
        /// {0} : 显示已隐藏的权限角色
        /// </remarks>
        private const string CUSTOMERROLES_ALL_KEY = "Ransurotto.customerrole.all-{0}";

        /// <summary>
        /// 权限角色系统名称缓存键
        /// </summary>
        /// <remarks>
        /// {0} : 系统名称
        /// </remarks>
        private const string CUSTOMERROLES_BY_SYSTEMNAME_KEY = "Ransurotto.customerrole.systemname-{0}";

        /// <summary>
        /// 清空缓存匹配键
        /// </summary>
        private const string CUSTOMERROLES_PATTERN_KEY = "Ransurotto.customerrole.";

        #endregion

        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly CustomerSettings _customerSettings;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<CustomerRole> _customerRoleRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerPassword> _customerPasswordRepository;
        private readonly IRepository<GenericAttribute> _gaRepository;

        #endregion

        #region Constructor

        public CustomerService(ICacheManager cacheManager, CustomerSettings customerSettings, IEventPublisher eventPublisher, IRepository<CustomerRole> customerRoleRepository, IRepository<Customer> customerRepository, IRepository<CustomerPassword> customerPasswordRepository, IRepository<GenericAttribute> gaRepository)
        {
            _cacheManager = cacheManager;
            _customerSettings = customerSettings;
            _eventPublisher = eventPublisher;
            _customerRoleRepository = customerRoleRepository;
            _customerRepository = customerRepository;
            _customerPasswordRepository = customerPasswordRepository;
            _gaRepository = gaRepository;
        }

        #endregion

        #region Methods

        #region Customer

        /// <summary>
        /// 通过标识符获取用户
        /// </summary>
        /// <param name="customerId">用户标识符</param>
        /// <returns>用户</returns>
        public virtual Customer GetCustomerById(int customerId)
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
        public virtual IList<Customer> GetCustomersByIds(int[] customerIds)
        {
            if (customerIds == null || customerIds.Length == 0)
                return new List<Customer>();

            var query = from c in _customerRepository.Table
                        where customerIds.Contains(c.Id)
                        select c;
            var customers = query.ToList();

            //按查询ID列表进行排序
            var sortedCustomers = new List<Customer>();
            foreach (int id in customerIds)
            {
                var customer = customers.Find(x => x.Id == id);
                if (customer != null)
                    sortedCustomers.Add(customer);
            }
            return sortedCustomers;
        }

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
        public virtual IPagedList<Customer> GetAllCustomers(DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
            int[] customerRoleIds = null, string email = null, string username = null, string name = null,
            int dayOfBirth = 0, int monthOfBirth = 0, string company = null, string phone = null,
             string ipAddress = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _customerRepository.Table;
            query = query.Where(c => !c.Deleted);
            if (createdFromUtc.HasValue)
                query = query.Where(c => createdFromUtc.Value <= c.CreatedOnUtc);
            if (createdToUtc.HasValue)
                query = query.Where(c => createdToUtc.Value >= c.CreatedOnUtc);
            if (customerRoleIds != null && customerRoleIds.Length > 0)
                query = query.Where(c => c.CustomerRoles.Select(cr => cr.Id).Intersect(customerRoleIds).Any());
            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(c => c.Email.Contains(email));
            if (!string.IsNullOrWhiteSpace(username))
                query = query.Where(c => c.Username.Contains(username));
            /*search by DateOfBirth*/
            if (dayOfBirth > 0 && monthOfBirth > 0)
            {
                string dateOfBirthStr = monthOfBirth.ToString("00", CultureInfo.InvariantCulture) + "-" + dayOfBirth.ToString("00", CultureInfo.InvariantCulture);
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { Customer = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "Customer" &&
                                 z.Attribute.Key == SystemCustomerAttributeNames.DateOfBirth &&
                                 z.Attribute.Value.Substring(5, 5) == dateOfBirthStr))
                    .Select(z => z.Customer);
            }
            else if (dayOfBirth > 0)
            {
                string dateOfBirthStr = dayOfBirth.ToString("00", CultureInfo.InvariantCulture);
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { Customer = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "Customer" &&
                                 z.Attribute.Key == SystemCustomerAttributeNames.DateOfBirth &&
                                 z.Attribute.Value.Substring(8, 2) == dateOfBirthStr))
                    .Select(z => z.Customer);
            }
            else if (monthOfBirth > 0)
            {
                string dateOfBirthStr = "-" + monthOfBirth.ToString("00", CultureInfo.InvariantCulture) + "-";
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { Customer = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "Customer" &&
                                 z.Attribute.Key == SystemCustomerAttributeNames.DateOfBirth &&
                                 z.Attribute.Value.Contains(dateOfBirthStr)))
                    .Select(z => z.Customer);
            }
            /*search by name*/
            if (!string.IsNullOrEmpty(name))
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, x => x.EntityId, (x, y) => new { Customer = x, Attribute = y })
                    .Where(z => z.Attribute.KeyGroup == "Customer" &&
                                z.Attribute.Key == SystemCustomerAttributeNames.Name &&
                                z.Attribute.Value.Contains(name))
                    .Select(p => p.Customer);
            }
            /*search by phone*/
            if (!string.IsNullOrWhiteSpace(phone))
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { Customer = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "Customer" &&
                                 z.Attribute.Key == SystemCustomerAttributeNames.Phone &&
                                 z.Attribute.Value.Contains(phone)))
                    .Select(z => z.Customer);
            }

            if (!string.IsNullOrWhiteSpace(ipAddress) && CommonHelper.IsValidIpAddress(ipAddress))
                query = query.Where(w => w.LastIpAddress == ipAddress);

            query = query.OrderByDescending(c => c.CreatedOnUtc);

            var customers = new PagedList<Customer>(query, pageIndex, pageSize);
            return customers;
        }

        /// <summary>
        /// 获取最后天数前活跃用户列表
        /// </summary>
        /// <param name="lastActivityFromUtc">用户最后活动日期（从）</param>
        /// <param name="customerRoleIds">通过用户角色标识符列表来匹配过滤;传递Null为不限制</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>用户列表</returns>
        public IPagedList<Customer> GetOnlineCustomers(DateTime lastActivityFromUtc, int[] customerRoleIds, int pageIndex = 0,
            int pageSize = Int32.MaxValue)
        {
            var query = _customerRepository.Table;
            query = query.Where(c => lastActivityFromUtc <= c.LastActivityDateUtc);
            query = query.Where(c => !c.Deleted);
            if (customerRoleIds != null && customerRoleIds.Length > 0)
                query = query.Where(c => c.CustomerRoles.Select(cr => cr.Id).Intersect(customerRoleIds).Any());

            query = query.OrderByDescending(c => c.LastActivityDateUtc);
            var customers = new PagedList<Customer>(query, pageIndex, pageSize);
            return customers;
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
        /// 添加游客用户
        /// </summary>
        /// <returns>用户</returns>
        public virtual Customer InsertGuestCustomer()
        {
            var customer = new Customer
            {
                Guid = Guid.NewGuid(),
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
            };

            var guestRole = GetCustomerRoleBySystemName(SystemCustomerRoleNames.Guests);
            if (guestRole == null)
                throw new SiteException("'游客'角色无法加载");

            customer.CustomerRoles.Add(guestRole);
            _customerRepository.Insert(customer);

            return customer;
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

            if (!string.IsNullOrEmpty(customer.Email))
                customer.Email += "-DELETED";
            if (!string.IsNullOrEmpty(customer.Username))
                customer.Username += "-DELETED";

            customer.Deleted = true;

            _customerRepository.Update(customer);

            //发布删除通知
            _eventPublisher.EntityDeleted(customer);
        }

        #endregion

        #region Customer roles

        /// <summary>
        /// 通过系统名称获取用户权限角色
        /// </summary>
        /// <param name="systemName">角色系统名称</param>
        /// <returns>用户权限角色</returns>
        public CustomerRole GetCustomerRoleBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            string key = string.Format(CUSTOMERROLES_BY_SYSTEMNAME_KEY, systemName);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _customerRoleRepository.Table
                            orderby cr.Id
                            where cr.SystemName == systemName
                            select cr;
                var customerRole = query.FirstOrDefault();
                return customerRole;
            });
        }

        /// <summary>
        /// 通过标识符获取权限角色
        /// </summary>
        /// <param name="customerRoleId">权限角色标识符</param>
        /// <returns>权限角色</returns>
        public CustomerRole GetCustomerRoleById(int customerRoleId)
        {
            if (customerRoleId == 0)
                return null;

            return _customerRoleRepository.GetById(customerRoleId);
        }

        /// <summary>
        /// 获取所有权限角色
        /// </summary>
        /// <param name="showHidden">是否显示已隐藏的权限角色</param>
        /// <returns>权限角色</returns>
        public IList<CustomerRole> GetAllCustomerRoles(bool showHidden = false)
        {
            string key = string.Format(CUSTOMERROLES_ALL_KEY, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _customerRoleRepository.Table
                            orderby cr.Name
                            where showHidden || cr.Active
                            select cr;
                var customerRoles = query.ToList();
                return customerRoles;
            });
        }

        /// <summary>
        /// 添加权限角色
        /// </summary>
        /// <param name="customerRole">权限角色</param>
        public void InsertCustomerRole(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException(nameof(customerRole));

            _customerRoleRepository.Insert(customerRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            _eventPublisher.EntityInserted(customerRole);
        }

        /// <summary>
        /// 更新权限角色
        /// </summary>
        /// <param name="customerRole">权限角色</param>
        public void UpdateCustomerRole(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException(nameof(customerRole));

            _customerRoleRepository.Update(customerRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            _eventPublisher.EntityUpdated(customerRole);
        }

        /// <summary>
        /// 删除权限角色
        /// </summary>
        /// <param name="customerRole">权限角色</param>
        public void DeleteCustomerRole(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException(nameof(customerRole));

            if (customerRole.IsSystemRole)
                throw new SiteException("无法删除系统角色。");

            _customerRoleRepository.Delete(customerRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            _eventPublisher.EntityDeleted(customerRole);
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
        public virtual IList<CustomerPassword> GetCustomerPasswords(int? customerId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null)
        {
            var query = _customerPasswordRepository.Table;

            if (customerId.HasValue)
                query = query.Where(password => password.CustomerId == customerId.Value);
            if (passwordFormat.HasValue)
                query = query.Where(password => password.PasswordFormatId == (int)passwordFormat.Value);
            if (passwordsToReturn.HasValue)
                query = query.OrderByDescending(password => password.CreatedOnUtc).Take(passwordsToReturn.Value);

            return query.ToList();
        }

        /// <summary>
        /// 通过用户标识符获取对应用户当前的密码
        /// </summary>
        /// <param name="customerId">用户标识符</param>
        /// <returns>用户密码</returns>
        public virtual CustomerPassword GetCurrentPassword(int customerId)
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

        #endregion

    }
}
