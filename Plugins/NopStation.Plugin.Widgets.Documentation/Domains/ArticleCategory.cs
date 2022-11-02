using Nop.Core;

namespace NopStation.Plugin.Widgets.Documentation.Domains
{
    public class ArticleCategory : BaseEntity
    {
        public int ArticleId { get; set; }

        public int CategoryId { get; set; }
    }
}
