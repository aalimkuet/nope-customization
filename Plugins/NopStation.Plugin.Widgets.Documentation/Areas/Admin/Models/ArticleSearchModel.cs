using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models
{
    public record ArticleSearchModel: BaseSearchModel
    {
        #region Ctor

        public ArticleSearchModel()
        {
            AvailableStores = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
            AvailablePublishedOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.List.SearchKeyword")]
        public string SearchKeyword { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.List.SearchPublished")]
        public int SearchPublishedId { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.List.SearchCategory")]
        public int SearchCategoryId { get; set; }

        public IList<SelectListItem> AvailablePublishedOptions { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.List.SearchStore")]
        public int SearchStoreId { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        public bool HideStoresList { get; set; }

        #endregion
    }
}
