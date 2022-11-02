using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core.Domain.Cms;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.BookTracker
{
    public class BookTrackerPlugin : BasePlugin, IWidgetPlugin
    {
        #region Fields
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly WidgetSettings _widgetSettings;


        #endregion

        #region Ctor

        public BookTrackerPlugin(
            IActionContextAccessor actionContextAccessor,
            ILocalizationService localizationService,
            ISettingService settingService,
            IUrlHelperFactory urlHelperFactory,
            WidgetSettings widgetSettings)
        {
            _actionContextAccessor = actionContextAccessor;
            _localizationService = localizationService;
            _settingService = settingService;
            _urlHelperFactory = urlHelperFactory;
            _widgetSettings = widgetSettings;
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
            return base.InstallAsync();
        }
        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<BookTrackerSettings>();

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
    }
}
