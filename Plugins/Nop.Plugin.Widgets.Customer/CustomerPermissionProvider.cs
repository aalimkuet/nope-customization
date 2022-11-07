using System.Collections.Generic;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Security;
using Nop.Services.Security;

namespace Nop.Plugin.Widgets.Customer
{
    public class CustomerPermissionProvider: IPermissionProvider
    {
        public static readonly PermissionRecord ManageCustomerTracker = new() { Name = "Admin area. Manage CustomerTracker", SystemName = "ManageCustomerTracker", Category = "Configuration" };

        public HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
        {
            return new HashSet<(string, PermissionRecord[])>
            {
                (
                    NopCustomerDefaults.AdministratorsRoleName,
                    new[]
                    {
                      ManageCustomerTracker
                    }
                )
            };
        }

        public IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                ManageCustomerTracker
            };
        }
    }
}
