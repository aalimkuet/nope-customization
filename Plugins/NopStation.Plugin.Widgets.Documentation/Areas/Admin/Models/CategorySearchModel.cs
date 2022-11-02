using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models
{
    public record CategorySearchModel : BaseSearchModel
    {
        #region Ctor

        public CategorySearchModel()
        {
            AvailableStores = new List<SelectListItem>();
            AvailablePublishedOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.List.SearchKeyword")]
        public string SearchKeyword { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.List.SearchPublished")]
        public int SearchPublishedId { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.List.SearchStore")]
        public int SearchStoreId { get; set; }

        public bool HideStoresList { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }
        public IList<SelectListItem> AvailablePublishedOptions { get; set; }

        #endregion
    }
}
