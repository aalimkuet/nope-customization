using Nop.Plugin.Widgets.Customer.Models;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a customer picture list model
    /// </summary>
    public partial record CustomerPictureListModel : BasePagedListModel<CustomerPictureModel>
    {
    }
}