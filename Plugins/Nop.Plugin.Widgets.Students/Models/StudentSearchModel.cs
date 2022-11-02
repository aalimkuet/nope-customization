using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.Students.Models
{
    /// <summary>
    /// Represents a Student search model
    /// </summary>
    public partial record StudentSearchModel : BaseSearchModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.Students.List.SearchName")]
        public string SearchName { get; set; }

        [NopResourceDisplayName("Admin.Students.List.SearchRoll")]
        public string SearchRoll { get; set; }

        #endregion
    }
}