using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace NopStation.Plugin.Widgets.Documentation.Models
{
    public record CategoryOverviewModel : BaseNopEntityModel
    {
        public CategoryOverviewModel()
        {
            ArticleList = new List<ArticleOverviewModel>();
            SubCategoryList = new List<CategoryOverviewModel>();
        }

        public string SeName { get; set; }

        public string Name { get; set; }

        public int MenuLevel { get; set; }

        public bool Selected { get; set; }

        public List<ArticleOverviewModel> ArticleList { get; set; }

        public List<CategoryOverviewModel> SubCategoryList { get; set; }
    }
}
