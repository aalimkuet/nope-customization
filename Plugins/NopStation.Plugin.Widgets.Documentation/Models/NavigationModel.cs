using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace NopStation.Plugin.Widgets.Documentation.Models
{
    public record NavigationModel : BaseNopModel
    {
        public NavigationModel()
        {
            MenuItems = new List<CategoryOverviewModel>();
        }

        public int CurrentCategoryId { get; set; }

        public int CurrentArticleId { get; set; }

        public List<CategoryOverviewModel> MenuItems { get; set; }
    }
}
