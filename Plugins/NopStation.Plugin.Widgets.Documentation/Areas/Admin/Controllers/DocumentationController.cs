using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using NopStation.Plugin.Misc.Core.Controllers;
using NopStation.Plugin.Misc.Core.Filters;
using NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Controllers
{
    public class DocumentationController : NopStationAdminController
    {
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IPermissionService _permissionService;

        public DocumentationController(ILocalizationService localizationService,
            INotificationService notificationService,
            ISettingService settingService,
            IStoreContext storeContext,
            IPermissionService permissionService)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _settingService = settingService;
            _storeContext = storeContext;
            _permissionService = permissionService;
        }

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageConfiguration))
                return AccessDeniedView();

            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var documentationSettings = await _settingService.LoadSettingAsync<DocumentationSettings>(storeScope);

            var model = new ConfigurationModel()
            {
                AllowGuestUsersToLeaveComments = documentationSettings.AllowGuestUsersToLeaveComments,
                DocumentationCommentsMustBeApproved = documentationSettings.DocumentationCommentsMustBeApproved,
                MinimumIntervalToAddComments = documentationSettings.MinimumIntervalToAddComments,
                HomepageText = documentationSettings.HomepageText,
                ShowLinkInFooterMenu = documentationSettings.ShowLinkInFooterMenu,
                ShowLinkOnTopMenu = documentationSettings.ShowLinkOnTopMenu,
                ActiveStoreScopeConfiguration = storeScope
            };

            model.ActiveStoreScopeConfiguration = storeScope;

            if (storeScope > 0)
            {
                model.AllowGuestUsersToLeaveComments_OverrideForStore = await _settingService.SettingExistsAsync(documentationSettings, x => x.AllowGuestUsersToLeaveComments, storeScope);
                model.DocumentationCommentsMustBeApproved_OverrideForStore = await _settingService.SettingExistsAsync(documentationSettings, x => x.DocumentationCommentsMustBeApproved, storeScope);
                model.MinimumIntervalToAddComments_OverrideForStore = await _settingService.SettingExistsAsync(documentationSettings, x => x.MinimumIntervalToAddComments, storeScope);
                model.HomepageText_OverrideForStore = await _settingService.SettingExistsAsync(documentationSettings, x => x.HomepageText, storeScope);
                model.ShowLinkInFooterMenu_OverrideForStore = await _settingService.SettingExistsAsync(documentationSettings, x => x.ShowLinkInFooterMenu, storeScope);
                model.ShowLinkOnTopMenu_OverrideForStore = await _settingService.SettingExistsAsync(documentationSettings, x => x.ShowLinkOnTopMenu, storeScope);
            }

            return View(model);
        }

        [EditAccess, HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageConfiguration))
                return AccessDeniedView();

            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var documentationSettings = await _settingService.LoadSettingAsync<DocumentationSettings>(storeScope);

            documentationSettings.AllowGuestUsersToLeaveComments = model.AllowGuestUsersToLeaveComments;
            documentationSettings.DocumentationCommentsMustBeApproved = model.DocumentationCommentsMustBeApproved;
            documentationSettings.MinimumIntervalToAddComments = model.MinimumIntervalToAddComments;
            documentationSettings.HomepageText = model.HomepageText;
            documentationSettings.ShowLinkInFooterMenu = model.ShowLinkInFooterMenu;
            documentationSettings.ShowLinkOnTopMenu = model.ShowLinkOnTopMenu;

            await _settingService.SaveSettingOverridablePerStoreAsync(documentationSettings, x => x.AllowGuestUsersToLeaveComments, model.AllowGuestUsersToLeaveComments_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(documentationSettings, x => x.DocumentationCommentsMustBeApproved, model.DocumentationCommentsMustBeApproved_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(documentationSettings, x => x.MinimumIntervalToAddComments, model.MinimumIntervalToAddComments_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(documentationSettings, x => x.HomepageText, model.HomepageText_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(documentationSettings, x => x.ShowLinkInFooterMenu, model.ShowLinkInFooterMenu_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(documentationSettings, x => x.ShowLinkOnTopMenu, model.ShowLinkOnTopMenu_OverrideForStore, storeScope, false);

            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

            return RedirectToAction("Configure");
        }
    }
}
