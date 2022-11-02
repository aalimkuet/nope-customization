using System.Threading.Tasks;
using Nop.Services.Events;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Mvc.Routing;

namespace NopStation.Plugin.Widgets.Documentation.Infrastructure
{
    public class RoutingEventConsumer : IConsumer<GenericRoutingEvent>
    {
        public Task HandleEventAsync(GenericRoutingEvent eventMessage)
        {
            var values = eventMessage.RouteValues;
            var urlRecord = eventMessage.UrlRecord;

            if (urlRecord.EntityName.ToLowerInvariant() == DocumentationDefaults.DocumentCategory.ToLowerInvariant())
            {
                values[NopPathRouteDefaults.ControllerFieldKey] = "Documentation";
                values[NopPathRouteDefaults.ActionFieldKey] = "Category";
                values[NopPathRouteDefaults.CategoryIdFieldKey] = urlRecord.EntityId;
                values[NopPathRouteDefaults.SeNameFieldKey] = urlRecord.Slug;
            }
            else if (urlRecord.EntityName.ToLowerInvariant() == DocumentationDefaults.DocumentArticle.ToLowerInvariant())
            {
                values[NopPathRouteDefaults.ControllerFieldKey] = "Documentation";
                values[NopPathRouteDefaults.ActionFieldKey] = "Article";
                values["articleId"] = urlRecord.EntityId;
                values[NopPathRouteDefaults.SeNameFieldKey] = urlRecord.Slug;
            }

            return Task.CompletedTask;
        }
    }
}
