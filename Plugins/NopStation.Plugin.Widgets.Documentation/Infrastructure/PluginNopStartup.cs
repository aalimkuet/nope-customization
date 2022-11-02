using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using NopStation.Plugin.Misc.Core.Infrastructure;
using NopStation.Plugin.Widgets.Documentation.Areas.Admin.Factories;
using NopStation.Plugin.Widgets.Documentation.Services;

namespace NopStation.Plugin.Widgets.Documentation.Infrastructure
{
    public class PluginNopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddNopStationServices("NopStation.Plugin.Widgets.Documentation");

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });

            services.AddScoped<IDocumentCategoryService, DocumentCategoryService>();
            services.AddScoped<IDocumentArticleService, DocumentArticleService>();

            services.AddScoped<IDocumentArticleModelFactory, DocumentArticleModelFactory>();
            services.AddScoped<IDocumentCategoryModelFactory, DocumentCategoryModelFactory>();
        }

        public void Configure(IApplicationBuilder application)
        {
        }

        public int Order => 11;
    }
}