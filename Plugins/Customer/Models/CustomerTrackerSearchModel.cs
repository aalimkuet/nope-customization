using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.Customer.Models
{
    /// <summary>
    /// Represents a CustomerTracker search model
    /// </summary>
    public partial record CustomerTrackerSearchModel : BaseSearchModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.CustomerTracker.List.SearchName")]
        public string SearchName { get; set; }

        [NopResourceDisplayName("Admin.CustomerTracker.List.SearchContactNo")]
        public string SearchContactNo { get; set; }

        #endregion
    }
}