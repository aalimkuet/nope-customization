using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core.Domain.Cms;
using Nop.Plugin.Widgets.BookTracker;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
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
        public bool HideInWidgetList => throw new NotImplementedException();

        public override string GetConfigurationPageUrl()
        {
            return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl("Plugin.Widgets.BookTracker.Configure");
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { "" });
        }
        public override Task InstallAsync()
        {
            //await this.InstallPluginAsync(new CustomerPermissionProvider());
            return base.InstallAsync();
        }
        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<CustomerTrackerSettings>();

            //fixed rates
            //var fixedRates = await(await _shippingService.GetAllShippingMethodsAsync())
            //    .SelectAwait(async shippingMethod => await _settingService.GetSettingAsync(
            //        string.Format(FixedByWeightByTotalDefaults.FixedRateSettingsKey, shippingMethod.Id)))
            //    .Where(setting => setting != null).ToListAsync();
          //  await _settingService.DeleteSettingsAsync(fixedRates);

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.BookTracker");

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

            //if (!await _permissionService.AuthorizeAsync(CustomerPermissionProvider.ManageCustomerTracker))
            //    return;

            var cild = parentNode.ChildNodes.FirstOrDefault(node => node.SystemName.Equals("CustomerTracker"));
            //if (cild == null)
            //    return;

            //var index = parentNode.ChildNodes.IndexOf(cild);

            //if (index < 0)
            //    return;

            var configItem = new SiteMapNode()
            {
                Title = "Customer Tracker",
                Url = "~/Admin/CustomerTracker/List",
                Visible = true,
                IconClass = "far fa-circle",
                SystemName = "CustomerTracker.Customers"
            };
            parentNode.ChildNodes.Add(configItem);


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
