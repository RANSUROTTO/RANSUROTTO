using System;
using System.Collections.Generic;
using System.Linq;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Security;
using RANSUROTTO.BLOG.Services.Customers;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Services.Security
{
    public class PermissionService : IPermissionService
    {

        #region Constants

        /// <summary>
        /// 权限项缓存
        /// </summary>
        /// <remarks>
        /// {0} : 权限角色ID
        /// {1} : 权限系统名称
        /// </remarks>
        private const string PERMISSIONS_ALLOWED_KEY = "Ransurotto.permission.allowed-{0}-{1}";

        /// <summary>
        /// 缓存清空匹配模式
        /// </summary>
        private const string PERMISSIONS_PATTERN_KEY = "Ransurotto.permission.";

        #endregion

        #region FieIds

        private readonly IRepository<PermissionRecord> _permissionRecordRepository;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Constructor

        public PermissionService(IRepository<PermissionRecord> permissionRecordRepository, ICustomerService customerService, IWorkContext workContext, ILocalizationService localizationService, ILanguageService languageService, ICacheManager cacheManager)
        {
            _permissionRecordRepository = permissionRecordRepository;
            _customerService = customerService;
            _workContext = workContext;
            _localizationService = localizationService;
            _languageService = languageService;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 通过标识符获取权限项
        /// </summary>
        /// <param name="permissionId">权限项标识符</param>
        /// <returns>权限项</returns>
        public virtual PermissionRecord GetPermissionRecordById(long permissionId)
        {
            if (permissionId == 0)
                return null;

            return _permissionRecordRepository.GetById(permissionId);
        }

        /// <summary>
        /// 通过系统名称获取权限项
        /// </summary>
        /// <param name="systemName">权限项系统名称</param>
        /// <returns>权限项</returns>
        public virtual PermissionRecord GetPermissionRecordBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from pr in _permissionRecordRepository.Table
                        where pr.SystemName == systemName
                        orderby pr.Id
                        select pr;

            var permissionRecord = query.FirstOrDefault();
            return permissionRecord;
        }

        /// <summary>
        /// 获取所有权限项
        /// </summary>
        /// <returns>权限项列表</returns>
        public virtual IList<PermissionRecord> GetAllPermissionRecords()
        {
            var query = from pr in _permissionRecordRepository.Table
                        orderby pr.Name
                        select pr;
            var permissions = query.ToList();
            return permissions;
        }

        /// <summary>
        /// 添加权限项
        /// </summary>
        /// <param name="permission">权限项</param>
        public virtual void InsertPermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            _permissionRecordRepository.Insert(permission);

            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// 更新权限项
        /// </summary>
        /// <param name="permission">权限项</param>
        public virtual void UpdatePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            _permissionRecordRepository.Update(permission);

            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// 删除权限项
        /// </summary>
        /// <param name="permission">权限项</param>
        public virtual void DeletePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            _permissionRecordRepository.Delete(permission);

            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// 安装权限
        /// </summary>
        /// <param name="permissionProvider">权限提供商</param>
        public virtual void InstallPermissions(IPermissionProvider permissionProvider)
        {
            var permissions = permissionProvider.GetPermissions();
            foreach (var permission in permissions)
            {
                var permission1 = GetPermissionRecordBySystemName(permission.SystemName);
                if (permission1 == null)
                {
                    //安装新权限
                    permission1 = new PermissionRecord
                    {
                        Name = permission.Name,
                        SystemName = permission.SystemName,
                        Category = permission.Category,
                    };

                    var defaultPermissions = permissionProvider.GetDefaultPermissions();
                    foreach (var defaultPermission in defaultPermissions)
                    {
                        var customerRole = _customerService.GetCustomerRoleBySystemName(defaultPermission.CustomerRoleSystemName);
                        if (customerRole == null)
                        {
                            customerRole = new CustomerRole
                            {
                                Name = defaultPermission.CustomerRoleSystemName,
                                Active = true,
                                SystemName = defaultPermission.CustomerRoleSystemName
                            };
                            _customerService.InsertCustomerRole(customerRole);
                        }
                        var defaultMappingProvided = (from p in defaultPermission.PermissionRecords
                                                      where p.SystemName == permission1.SystemName
                                                      select p).Any();
                        var mappingExists = (from p in customerRole.PermissionRecords
                                             where p.SystemName == permission1.SystemName
                                             select p).Any();
                        if (defaultMappingProvided && !mappingExists)
                        {
                            permission1.CustomerRoles.Add(customerRole);
                        }
                    }

                    InsertPermissionRecord(permission1);
                    permission1.SaveLocalizedPermissionName(_localizationService, _languageService);
                }
            }
        }

        /// <summary>
        /// 卸载权限
        /// </summary>
        /// <param name="permissionProvider">权限提供商</param>
        public virtual void UninstallPermissions(IPermissionProvider permissionProvider)
        {
            var permissions = permissionProvider.GetPermissions();
            foreach (var permission in permissions)
            {
                var permission1 = GetPermissionRecordBySystemName(permission.SystemName);
                if (permission1 != null)
                {
                    DeletePermissionRecord(permission1);
                    permission1.DeleteLocalizedPermissionName(_localizationService, _languageService);
                }
            }
        }

        /// <summary>
        /// 检查当前用户是否可通过权限项
        /// </summary>
        /// <param name="permission">权限项</param>
        /// <returns>结果</returns>
        public virtual bool Authorize(PermissionRecord permission)
        {
            return Authorize(permission, _workContext.CurrentCustomer);
        }

        /// <summary>
        /// 检查指定用户是否可通过权限项
        /// </summary>
        /// <param name="permission">权限项</param>
        /// <param name="customer">用户</param>
        /// <returns>结果</returns>
        public virtual bool Authorize(PermissionRecord permission, Customer customer)
        {
            if (permission == null)
                return false;

            if (customer == null)
                return false;

            return Authorize(permission.SystemName, customer);
        }

        /// <summary>
        /// 检查当前用户是否可通过指定系统名称的权限项
        /// </summary>
        /// <param name="permissionRecordSystemName">权限项的系统名称</param>
        /// <returns>结果</returns>
        public virtual bool Authorize(string permissionRecordSystemName)
        {
            return Authorize(permissionRecordSystemName, _workContext.CurrentCustomer);
        }

        /// <summary>
        /// 检查指定用户是否可通过指定系统名称的权限项
        /// </summary>
        /// <param name="permissionRecordSystemName">权限项的系统名称</param>
        /// <param name="customer">用户</param>
        /// <returns>结果</returns>
        public virtual bool Authorize(string permissionRecordSystemName, Customer customer)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var customerRoles = customer.CustomerRoles.Where(cr => cr.Active);
            foreach (var role in customerRoles)
                if (Authorize(permissionRecordSystemName, role))
                    return true;

            return false;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="permissionRecordSystemName">权限项系统名称</param>
        /// <param name="customerRole">权限角色</param>
        /// <returns>结果</returns>
        protected virtual bool Authorize(string permissionRecordSystemName, CustomerRole customerRole)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            string key = string.Format(PERMISSIONS_ALLOWED_KEY, customerRole.Id, permissionRecordSystemName);
            return _cacheManager.Get(key, () =>
            {
                foreach (var permission1 in customerRole.PermissionRecords)
                    if (permission1.SystemName.Equals(permissionRecordSystemName, StringComparison.InvariantCultureIgnoreCase))
                        return true;

                return false;
            });
        }

        #endregion

    }
}
