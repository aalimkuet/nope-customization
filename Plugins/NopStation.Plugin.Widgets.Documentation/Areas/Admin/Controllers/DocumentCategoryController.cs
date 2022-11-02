using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NopStation.Plugin.Misc.Core.Controllers;
using NopStation.Plugin.Misc.Core.Filters;
using NopStation.Plugin.Widgets.Documentation.Areas.Admin.Factories;
using NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models;
using NopStation.Plugin.Widgets.Documentation.Domains;
using NopStation.Plugin.Widgets.Documentation.Services;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Mvc.Filters;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Controllers
{
    public class DocumentCategoryController : NopStationAdminController
    {
        #region Fields

        private readonly IDocumentCategoryService _documentCategoryService;
        private readonly IDocumentCategoryModelFactory _documentCategoryModelFactory;
        private readonly IPermissionService _permissionService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IStoreService _storeService;

        #endregion

        #region Ctor

        public DocumentCategoryController(IDocumentCategoryService documentCategoryService,
            IDocumentCategoryModelFactory documentCategoryModelFactory,
            IPermissionService permissionService,
            IUrlRecordService urlRecordService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IStoreMappingService storeMappingService,
            ILocalizedEntityService localizedEntityService,
            IStoreService storeService)
        {
            _documentCategoryService = documentCategoryService;
            _documentCategoryModelFactory = documentCategoryModelFactory;
            _permissionService = permissionService;
            _urlRecordService = urlRecordService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _storeMappingService = storeMappingService;
            _localizedEntityService = localizedEntityService;
            _storeService = storeService;
        }

        #endregion

        #region Utilities

        protected virtual async Task UpdateLocalesAsync(DocumentCategory category, CategoryModel model)
        {
            foreach (var localized in model.Locales)
            {
                await _localizedEntityService.SaveLocalizedValueAsync(category,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(category,
                    x => x.Description,
                    localized.Description,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(category,
                    x => x.MetaKeywords,
                    localized.MetaKeywords,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(category,
                    x => x.MetaDescription,
                    localized.MetaDescription,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(category,
                    x => x.MetaTitle,
                    localized.MetaTitle,
                    localized.LanguageId);

                //search engine name
                var seName = await _urlRecordService.ValidateSeNameAsync(category, localized.SeName, localized.Name, false);
                await _urlRecordService.SaveSlugAsync(category, seName, localized.LanguageId);
            }
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task SaveStoreMappingsAsync(DocumentCategory category, CategoryModel model)
        {
            category.LimitedToStores = model.SelectedStoreIds.Any();
            await _documentCategoryService.UpdateCategoryAsync(category);

            var existingStoreMappings = await _storeMappingService.GetStoreMappingsAsync(category);
            var allStores = await _storeService.GetAllStoresAsync();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds.Contains(store.Id))
                {
                    //new store
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        await _storeMappingService.InsertStoreMappingAsync(category, store.Id);
                }
                else
                {
                    //remove store
                    var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
                    if (storeMappingToDelete != null)
                        await _storeMappingService.DeleteStoreMappingAsync(storeMappingToDelete);
                }
            }
        }

        #endregion

        #region Methods

        #region Category / list

        public async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationCategories))
                return AccessDeniedView();

            var model = await _documentCategoryModelFactory.PrepareDocumentCategorySearchModelAsync(new CategorySearchModel());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> List(CategorySearchModel documentCategorySearchModel)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationCategories))
                return AccessDeniedView();

            var categories = await _documentCategoryModelFactory.PrepareDocumentCategoryListModelAsync(documentCategorySearchModel);
            return Json(categories);
        }

        #endregion

        #region Create / edit / delete

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationCategories))
                return AccessDeniedView();

            var model = await _documentCategoryModelFactory.PrepareDocumentCategoryModelAsync(new CategoryModel(), null);

            return View(model);
        }

        [EditAccess, HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(CategoryModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationCategories))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var category = model.ToEntity<DocumentCategory>();
                category.CreatedOnUtc = DateTime.UtcNow;
                category.UpdatedOnUtc = DateTime.UtcNow;

                await _documentCategoryService.InsertCategoryAsync(category);

                //search engine name
                model.SeName = await _urlRecordService.ValidateSeNameAsync(category, model.SeName, category.Name, true);
                await _urlRecordService.SaveSlugAsync(category, model.SeName, 0);

                //locales
                await UpdateLocalesAsync(category, model);

                //stores
                await SaveStoreMappingsAsync(category, model);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Categories.Created"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = category.Id });
            }

            model = await _documentCategoryModelFactory.PrepareDocumentCategoryModelAsync(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationCategories))
                return AccessDeniedView();

            var category = await _documentCategoryService.GetCategoryByIdAsync(id);
            if (category == null || category.Deleted)
                return RedirectToAction("List");

            var model = await _documentCategoryModelFactory.PrepareDocumentCategoryModelAsync(null, category);

            return View(model);
        }

        [EditAccess, HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(CategoryModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationCategories))
                return AccessDeniedView();

            var category = await _documentCategoryService.GetCategoryByIdAsync(model.Id);
            if (category == null || category.Deleted)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                category = model.ToEntity(category);
                category.UpdatedOnUtc = DateTime.UtcNow;

                await _documentCategoryService.UpdateCategoryAsync(category);

                //search engine name
                model.SeName = await _urlRecordService.ValidateSeNameAsync(category, model.SeName, category.Name, true);
                await _urlRecordService.SaveSlugAsync(category, model.SeName, 0);

                //locales
                await UpdateLocalesAsync(category, model);

                //stores
                await SaveStoreMappingsAsync(category, model);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Categories.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = category.Id });
            }

            //prepare model
            model = await _documentCategoryModelFactory.PrepareDocumentCategoryModelAsync(model, null);

            return View(model);
        }

        [EditAccess, HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationCategories))
                return AccessDeniedView();

            var category = await _documentCategoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return RedirectToAction("List");

            await _documentCategoryService.DeleteCategoryAsync(category);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Categories.Deleted"));

            return RedirectToAction("List");
        }

        [EditAccessAjax, HttpPost]
        public async Task<IActionResult> DeleteSelected(ICollection<int> selectedIds)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationCategories))
                return AccessDeniedView();

            var categories = await _documentCategoryService.GetCategoriesByIdsAsync(selectedIds.ToArray());
            foreach (var category in categories)
                await _documentCategoryService.DeleteCategoryAsync(category);

            return Json(new { Result = true });
        }

        #endregion

        #endregion
    }
}
