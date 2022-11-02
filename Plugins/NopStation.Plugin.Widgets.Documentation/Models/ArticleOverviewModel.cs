using Nop.Web.Framework.Models;

namespace NopStation.Plugin.Widgets.Documentation.Models
{
    public record ArticleOverviewModel : BaseNopEntityModel
    {
        public string SeName { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public bool Selected { get; set; }
    }
}
