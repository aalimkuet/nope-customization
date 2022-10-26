using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widget.MyWidget.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widget.MyWidget.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class MyWidgetController : BasePluginController
    {

        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly MyWidgetSettings _myWidgetSettings;

        #endregion

        #region Ctor

        public MyWidgetController(
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext,
            MyWidgetSettings myWidgetSettings)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _myWidgetSettings = myWidgetSettings;
        }

        #endregion

        #region Methods

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var myWidgetSettings = await _settingService.LoadSettingAsync<MyWidgetSettings>(storeScope);

            var model = new ConfigurationModel
            {
                CustomText = myWidgetSettings.CustomText,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.CustomText_OverrideForStore = await _settingService.SettingExistsAsync(myWidgetSettings, x => x.CustomText, storeScope);
               
            }
            return View("~/Plugins/Widget.MyWidget/Views/Configure.cshtml", model);
           
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var myWidgetSettings = await _settingService.LoadSettingAsync<MyWidgetSettings>(storeScope);

            myWidgetSettings.CustomText = model.CustomText;
            

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            await _settingService.SaveSettingOverridablePerStoreAsync(myWidgetSettings, x => x.CustomText, model.CustomText_OverrideForStore, storeScope, false); 

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        #endregion
         
        public async Task<IActionResult> Hello()
        {
          

            return View("~/Plugins/Widget.MyWidget/Views/Hello.cshtml");
        }
    }
}