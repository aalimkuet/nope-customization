using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.Students.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        public string Id { get; set; }
        [NopResourceDisplayName("My Text")]
        public string CustomText { get; set; }
        public bool CustomText_OverrideForStore { get; set; }
        public bool IsActive { get; set; }
        
    }
}