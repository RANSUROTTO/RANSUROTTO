using System.Collections.Generic;
using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Admin.Models.Customers;

namespace RANSUROTTO.BLOG.Admin.Models.Security
{
    public class PermissionMappingModel : BaseModel
    {
        public PermissionMappingModel()
        {
            AvailablePermissions = new List<PermissionRecordModel>();
            AvailableCustomerRoles = new List<CustomerRoleModel>();
        }

        public IList<PermissionRecordModel> AvailablePermissions { get; set; }
        public IList<CustomerRoleModel> AvailableCustomerRoles { get; set; }

        //[permission system name] / [customer role id] / [allowed]
        public IDictionary<string, IDictionary<long, bool>> Allowed { get; set; }
    }
}