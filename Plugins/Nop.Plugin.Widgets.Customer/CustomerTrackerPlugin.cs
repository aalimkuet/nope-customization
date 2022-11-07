using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core.Domain.Cms;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Customer
{
    public class CustomerTrackerPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        #region Fields
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly WidgetSettings _widgetSettings;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public CustomerTrackerPlugin(
            IActionContextAccessor actionContextAccessor,
            ILocalizationService localizationService,
            ISettingService settingService,
            IUrlHelperFactory urlHelperFactory,
            WidgetSettings widgetSettings,
            IPermissionService permissionService
            )
        {
            _actionContextAccessor = actionContextAccessor;
            _localizationService = localizationService;
            _settingService = settingService;
            _urlHelperFactory = urlHelperFactory;
            _widgetSettings = widgetSettings;
            _permissionService = permissionService;
        }

        #endregion
        public bool HideInWidgetList => false;

        public override string GetConfigurationPageUrl()
        {
            return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl("Plugin.Widgets.BookTracker.Configure");
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "CustomerTracker";
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HomepageBottom,  });
        }
        public override async Task InstallAsync()
        {
            //register default permissions
            var permissionProviders = new List<Type> { typeof(CustomerPermissionProvider) };
            foreach (var providerType in permissionProviders)
            {
                var provider = (IPermissionProvider)Activator.CreateInstance(providerType);
                await _permissionService.InstallPermissionsAsync(provider);
            }

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Admin.CustomerTrackers.Fields.Name.Required"] = "Customer Name Required",
                ["Admin.CustomerTrackers.Fields.ContactNo.Required"] = "Contact No Required",
                ["Admin.CustomerTrackers.Fields.Address.Required"] = "Address Required",

            });

           await base.InstallAsync();
        }
        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<CustomerTrackerSettings>();
                         
            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.CustomerTracker");

            await base.UninstallAsync();
        }
        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            //var menuItem = new SiteMapNode()
            //{
            //    Title = await _localizationService.GetResourceAsync("Admin.Nop.CustomerTracker.Menu"),
            //    Visible = true,
            //    IconClass = "far fa-dot-circle",
            //};

            //if (await _permissionService.AuthorizeAsync(CustomerPermissionProvider.ManageCustomerTracker))
            //{
            //    var configItem = new SiteMapNode()
            //    {
            //        Title = await _localizationService.GetResourceAsync("Admin.Nop.CustomerTracker.Menu.Customers"),
            //        Url = "~/Admin/CustomerTracker/List",
            //        Visible = true,
            //        IconClass = "far fa-circle",
            //        SystemName = "CustomerTracker.Customers"
            //    };
            //    menuItem.ChildNodes.Add(configItem);
            //}

            var parentNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "MyPlugin");
            if (parentNode == null)
            {
                parentNode = new SiteMapNode()
                {
                    Title = "My Plugin",
                    SystemName = "MyPlugin",
                    IconClass = "fas fa-tags",
                    Visible = true,
                };
                rootNode.ChildNodes.Add(parentNode);
            }
            var customerNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "CustomerTracker");
            if (customerNode == null)
            {
                customerNode = new SiteMapNode()
                {
                    Title = "Customer Tracker",
                    SystemName = "CustomerTracker",
                    IconClass = "far fa-dot-circle",
                    Visible = true,
                };
                parentNode.ChildNodes.Add(customerNode);
            }

            //if (!await _permissionService.AuthorizeAsync(CustomerPermissionProvider.ManageCustomerTracker))
            //    return;

            var ListItem = new SiteMapNode()
            {
                Title = "List",
                Url = "~/Admin/CustomerTracker/List",
                Visible = true,
                IconClass = "far fa-dot-circle",
                SystemName = "CustomerTrackerList"
            };
            customerNode.ChildNodes.Add(ListItem);


            //parentNode.ChildNodes.Insert(index, new SiteMapNode
            //{
            //    SystemName = "CustomerTracker",
            //    Title = "Customer Tracker",
            //    ControllerName = "CustomerTracker",
            //    ActionName = "List",
            //    IconClass = "far fa-dot-circle",
            //    Visible = true,
            //    RouteValues = new RouteValueDictionary { { "area", AreaNames.Admin } }
            //});          

        }
    }
}
