using System.Collections.Generic;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Security;
using Nop.Services.Security;

namespace NopStation.Plugin.Widgets.Documentation
{
    public class DocumentationPermissionProvider: IPermissionProvider
    {
        public static readonly PermissionRecord ManageConfiguration = new PermissionRecord { Name = "NopStation Documentation. Manage Documentation Configuration", SystemName = "ManageNopStationDocumentationConfiguration", Category = "NopStation" };
        public static readonly PermissionRecord ManageDocumentationCategories = new PermissionRecord { Name = "NopStation Documentation. Manage Documentation Category", SystemName = "ManageNopStationDocumentationCategory", Category = "NopStation" };
        public static readonly PermissionRecord ManageDocumentationArticles = new PermissionRecord { Name = "NopStation Documentation. Manage Documentation Article", SystemName = "ManageNopStationDocumentationArticle", Category = "NopStation" };

        public HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
        {
            return new HashSet<(string, PermissionRecord[])>
            {
                (
                    NopCustomerDefaults.AdministratorsRoleName,
                    new[]
                    {
                        ManageConfiguration,
                        ManageDocumentationCategories,
                        ManageDocumentationArticles
                    }
                )
            };
        }

        public IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                ManageConfiguration,
                ManageDocumentationCategories,
                ManageDocumentationArticles
            };
        }
    }
}
