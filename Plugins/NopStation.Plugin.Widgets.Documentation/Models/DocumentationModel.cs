using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace NopStation.Plugin.Widgets.Documentation.Models
{
    public record DocumentationModel: BaseNopModel
    {
        public string HomepageText { get; set; }
    }
}
