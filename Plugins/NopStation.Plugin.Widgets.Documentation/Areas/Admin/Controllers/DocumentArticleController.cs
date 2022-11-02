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
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Controllers
{
    public class DocumentArticleController : NopStationAdminController
    {
        #region Fields

        private readonly IDocumentArticleService _documentArticleService;
        private readonly IDocumentArticleModelFactory _documentArticleModelFactory;
        private readonly IPermissionService _permissionService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IStoreService _storeService;
        private readonly IDocumentCategoryService _documentCategoryService;

        #endregion

        #region Ctor

        public DocumentArticleController(IDocumentArticleService documentArticleService,
            IDocumentArticleModelFactory documentArticleModelFactory,
            IPermissionService permissionService,
            IUrlRecordService urlRecordService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IStoreMappingService storeMappingService,
            ILocalizedEntityService localizedEntityService,
            IStoreService storeService,
            IDocumentCategoryService documentCategoryService)
        {
            _documentArticleService = documentArticleService;
            _documentArticleModelFactory = documentArticleModelFactory;
            _permissionService = permissionService;
            _urlRecordService = urlRecordService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _storeMappingService = storeMappingService;
            _localizedEntityService = localizedEntityService;
            _storeService = storeService;
            _documentCategoryService = documentCategoryService;
        }

        #endregion

        #region Utilities

        protected virtual async Task UpdateLocalesAsync(DocumentArticle article, ArticleModel model)
        {
            foreach (var localized in model.Locales)
            {
                await _localizedEntityService.SaveLocalizedValueAsync(article,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(article,
                    x => x.Description,
                    localized.Description,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(article,
                    x => x.MetaKeywords,
                    localized.MetaKeywords,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(article,
                    x => x.MetaDescription,
                    localized.MetaDescription,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(article,
                    x => x.MetaTitle,
                    localized.MetaTitle,
                    localized.LanguageId);

                //search engine name
                var seName = await _urlRecordService.ValidateSeNameAsync(article, localized.SeName, localized.Name, false);
                await _urlRecordService.SaveSlugAsync(article, seName, localized.LanguageId);
            }
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task SaveStoreMappingsAsync(DocumentArticle article, ArticleModel model)
        {
            article.LimitedToStores = model.SelectedStoreIds.Any();
            await _documentArticleService.UpdateArticleAsync(article);

            var existingStoreMappings = await _storeMappingService.GetStoreMappingsAsync(article);
            var allStores = await _storeService.GetAllStoresAsync();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds.Contains(store.Id))
                {
                    //new store
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        await _storeMappingService.InsertStoreMappingAsync(article, store.Id);
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

        protected virtual async Task SaveCategoryMappingsAsync(DocumentArticle article, ArticleModel model)
        {
            var existingArticleCategories = await _documentCategoryService.GetArticleCategoriesByArticleIdAsync(article.Id, true);

            //delete categories
            foreach (var existingArticleCategory in existingArticleCategories)
                if (!model.SelectedCategoryIds.Contains(existingArticleCategory.CategoryId))
                    await _documentCategoryService.DeleteArticleCategoryAsync(existingArticleCategory);

            //add categories
            foreach (var categoryId in model.SelectedCategoryIds)
            {
                if (_documentCategoryService.FindArticleCategory(existingArticleCategories, article.Id, categoryId) == null)
                {
                    await _documentCategoryService.InsertArticleCategoryAsync(new ArticleCategory
                    {
                        ArticleId = article.Id,
                        CategoryId = categoryId,
                    });
                }
            }
        }

        #endregion

        #region Methods

        #region Article / list

        public async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            var model = await _documentArticleModelFactory.PrepareArticleSearchModelAsync(new ArticleSearchModel());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> List(ArticleSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            var articles = await _documentArticleModelFactory.PrepareArticleListModelAsync(searchModel);
            return Json(articles);
        }

        #endregion

        #region Create / edit / delete

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            var model = await _documentArticleModelFactory.PrepareArticleModelAsync(new ArticleModel(), null);

            return View(model);
        }

        [EditAccess, HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(ArticleModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var article = model.ToEntity<DocumentArticle>();
                article.CreatedOnUtc = DateTime.UtcNow;
                article.UpdatedOnUtc = DateTime.UtcNow;

                await _documentArticleService.InsertArticleAsync(article);

                //search engine name
                model.SeName = await _urlRecordService.ValidateSeNameAsync(article.Id, DocumentationDefaults.DocumentArticle ,model.SeName, article.Name, true);
                await _urlRecordService.SaveSlugAsync(article, model.SeName, 0);

                //locales
                await UpdateLocalesAsync(article, model);

                //categories
                await SaveCategoryMappingsAsync(article, model);

                //stores
                await SaveStoreMappingsAsync(article, model);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Articles.Created"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = article.Id });
            }

            model = await _documentArticleModelFactory.PrepareArticleModelAsync(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            var article = await _documentArticleService.GetArticleByIdAsync(id);
            if (article == null || article.Deleted)
                return RedirectToAction("List");

            var model = await _documentArticleModelFactory.PrepareArticleModelAsync(null, article);

            return View(model);
        }

        [EditAccess, HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(ArticleModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            var article = await _documentArticleService.GetArticleByIdAsync(model.Id);
            if (article == null || article.Deleted)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                article = model.ToEntity(article);
                article.UpdatedOnUtc = DateTime.UtcNow;

                await _documentArticleService.UpdateArticleAsync(article);

                //search engine name
                model.SeName = await _urlRecordService.ValidateSeNameAsync(article, model.SeName, article.Name, true);
                await _urlRecordService.SaveSlugAsync(article, model.SeName, 0);

                //locales
                await UpdateLocalesAsync(article, model);

                //categories
                await SaveCategoryMappingsAsync(article, model);

                //stores
                await SaveStoreMappingsAsync(article, model);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Articles.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = article.Id });
            }

            //prepare model
            model = await _documentArticleModelFactory.PrepareArticleModelAsync(model, null);

            return View(model);
        }

        [EditAccess, HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            var article = await _documentArticleService.GetArticleByIdAsync(id);
            if (article == null)
                return RedirectToAction("List");

            await _documentArticleService.DeleteArticleAsync(article);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Articles.Deleted"));

            return RedirectToAction("List");
        }

        #endregion

        #region Comments

        public async Task<IActionResult> CommentList()
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            var searchModel = await _documentArticleModelFactory.PrepareArticleCommentSearchModelAsync(new ArticleCommentSearchModel(), null);
            return View(searchModel);
        }

        [HttpPost]
        public async Task<IActionResult> CommentList(ArticleCommentSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return await AccessDeniedDataTablesJson();

            var articles = await _documentArticleModelFactory.PrepareArticleCommentListModelAsync(searchModel);
            return Json(articles);
        }

        [EditAccessAjax, HttpPost]
        public async Task<IActionResult> UpdateComment(ArticleCommentModel model)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            var comment = await _documentArticleService.GetArticleCommentByIdAsync(model.Id)
                ?? throw new ArgumentException("No comment found with the specified id");

            comment = model.ToEntity(comment);
            await _documentArticleService.UpdateArticleCommentAsync(comment);

            return new NullJsonResult();
        }

        [EditAccessAjax, HttpPost]
        public async Task<IActionResult> DeleteComment(ArticleCommentModel model)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            var comment = await _documentArticleService.GetArticleCommentByIdAsync(model.Id);
            await _documentArticleService.DeleteArticleCommentAsync(comment);

            return new NullJsonResult();
        }

        [HttpPost]
        public async Task<IActionResult> ApproveSelectedComment(ICollection<int> selectedIds)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            //filter not approved comments
            var comments = await _documentArticleService.GetArticleCommentsByIdsAsync(selectedIds, false);

            foreach (var comment in comments)
            {
                comment.IsApproved = true;
                await _documentArticleService.UpdateArticleCommentAsync(comment);
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public async Task<IActionResult> DisapproveSelectedComment(ICollection<int> selectedIds)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            //filter approved comments
            var comments = await _documentArticleService.GetArticleCommentsByIdsAsync(selectedIds, true);

            foreach (var comment in comments)
            {
                comment.IsApproved = false;
                await _documentArticleService.UpdateArticleCommentAsync(comment);
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelectedComment(ICollection<int> selectedIds)
        {
            if (!await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
                return AccessDeniedView();

            //filter approved comments
            var comments = await _documentArticleService.GetArticleCommentsByIdsAsync(selectedIds, true);
            if (comments.Any())
                await _documentArticleService.DeleteArticleCommentAsync(comments);

            return Json(new { Result = true });
        }

        #endregion

        #endregion
    }
}
