using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.Customer.Models
{
    public partial record CustomerPictureSearchModel : BaseSearchModel
    {
        #region Properties

        public int CustomerId { get; set; }

        #endregion
    }
}