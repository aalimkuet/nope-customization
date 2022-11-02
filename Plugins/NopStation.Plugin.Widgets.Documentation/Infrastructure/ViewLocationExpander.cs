using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace NopStation.Plugin.Widgets.Documentation.Infrastructure
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "nop.themename";

        public void PopulateValues(ViewLocationExpanderContext context)
        {

        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.AreaName == "Admin")
            {
                viewLocations = new string[] {
                    $"/Plugins/NopStation.Plugin.Widgets.Documentation/Areas/Admin/Views/Shared/{{0}}.cshtml",
                    $"/Plugins/NopStation.Plugin.Widgets.Documentation/Areas/Admin/Views/{{1}}/{{0}}.cshtml"
                }.Concat(viewLocations);
            }
            else
            {
                viewLocations = new string[] {
                    $"/Plugins/NopStation.Plugin.Widgets.Documentation/Views/Shared/{{0}}.cshtml",
                    $"/Plugins/NopStation.Plugin.Widgets.Documentation/Views/{{1}}/{{0}}.cshtml"
                }.Concat(viewLocations);

                if (context.Values.TryGetValue(THEME_KEY, out string theme))
                {
                    viewLocations = new string[] {
                        $"/Plugins/NopStation.Plugin.Widgets.Documentation/Themes/{theme}/Views/Shared/{{0}}.cshtml",
                        $"/Plugins/NopStation.Plugin.Widgets.Documentation/Themes/{theme}/Views/{{1}}/{{0}}.cshtml"
                    }.Concat(viewLocations);
                }
            }
            return viewLocations;
        }
    }
}
