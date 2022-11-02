using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Domain.Localization;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Infrastructure;

namespace NopStation.Plugin.Widgets.Documentation.Infrastructure
{
    public class RouteProvider : BaseRouteProvider, IRouteProvider
    {
        public int Priority => -1;

        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            var lang = GetLanguageRoutePattern();

            endpointRouteBuilder.MapControllerRoute("Documentations", $"{lang}/knowledgebase/",
                new { controller = "Documentation", action = "Home" });

            //generic routes
            var pattern = $"{lang}/{{SeName}}";

            endpointRouteBuilder.MapDynamicControllerRoute<SlugRouteTransformer>(pattern);

            endpointRouteBuilder.MapControllerRoute("DocumentArticle", pattern,
                new { controller = "Documentation", action = "Article" });

            endpointRouteBuilder.MapControllerRoute("DocumentCategory", pattern,
                new { controller = "Documentation", action = "Category" });
        }
    }
}
