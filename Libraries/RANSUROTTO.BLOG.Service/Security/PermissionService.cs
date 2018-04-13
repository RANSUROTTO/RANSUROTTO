using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Security;

namespace RANSUROTTO.BLOG.Service.Security
{
    public class PermissionService : IPermissionService
    {

        #region Methods

        public virtual PermissionRecord GetPermissionRecordById(int permissionId)
        {
            throw new NotImplementedException();
        }

        public virtual PermissionRecord GetPermissionRecordBySystemName(string systemName)
        {
            throw new NotImplementedException();
        }

        public virtual IList<PermissionRecord> GetAllPermissionRecords()
        {
            throw new NotImplementedException();
        }

        public virtual void InsertPermissionRecord(PermissionRecord permission)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdatePermissionRecord(PermissionRecord permission)
        {
            throw new NotImplementedException();
        }

        public virtual void DeletePermissionRecord(PermissionRecord permission)
        {
            throw new NotImplementedException();
        }

        public virtual void InstallPermissions(IPermissionProvider permissionProvider)
        {
            throw new NotImplementedException();
        }

        public virtual void UninstallPermissions(IPermissionProvider permissionProvider)
        {
            throw new NotImplementedException();
        }

        public virtual bool Authorize(PermissionRecord permission)
        {
            throw new NotImplementedException();
        }

        public virtual bool Authorize(PermissionRecord permission, Customer customer)
        {
            throw new NotImplementedException();
        }

        public virtual bool Authorize(string permissionRecordSystemName)
        {
            throw new NotImplementedException();
        }

        public virtual bool Authorize(string permissionRecordSystemName, Customer customer)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
