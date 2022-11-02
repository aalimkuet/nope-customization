using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Catalog;
using NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models;
using NopStation.Plugin.Widgets.Documentation.Domains;
using NopStation.Plugin.Widgets.Documentation.Services;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Factories
{
    public class DocumentCategoryModelFactory : IDocumentCategoryModelFactory
    {
        private readonly IDocumentCategoryService _documentCategoryService;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly CatalogSettings _catalogSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
        private readonly IDateTimeHelper _dateTimeHelper;

        public DocumentCategoryModelFactory(IDocumentCategoryService documentCategoryService,
            IBaseAdminModelFactory baseAdminModelFactory,
            CatalogSettings catalogSettings,
            ILocalizationService localizationService,
            IUrlRecordService urlRecordService,
            IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
            IDateTimeHelper dateTimeHelper)
        {
            _documentCategoryService = documentCategoryService;
            _baseAdminModelFactory = baseAdminModelFactory;
            _catalogSettings = catalogSettings;
            _localizationService = localizationService;
            _urlRecordService = urlRecordService;
            _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
            _dateTimeHelper = dateTimeHelper;
        }

        public async Task<CategoryModel> PrepareDocumentCategoryModelAsync(CategoryModel model, Domains.DocumentCategory documentCategory, bool excludeProperties = false)
        {
            Action<CategoryLocalizedModel, int> localizedModelConfiguration = null;

            if (documentCategory != null)
            {
                if (model == null)
                {
                    model = documentCategory.ToModel<CategoryModel>();
                    model.SeName = await _urlRecordService.GetSeNameAsync(documentCategory, 0, true, false);
                    model.CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(documentCategory.CreatedOnUtc, DateTimeKind.Utc);
                    model.UpdatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(documentCategory.UpdatedOnUtc, DateTimeKind.Utc);
                }

                //define localized model configuration action
                localizedModelConfiguration = async (locale, languageId) =>
                {
                    locale.Name = await _localizationService.GetLocalizedAsync(documentCategory, entity => entity.Name, languageId, false, false);
                    locale.Description = await _localizationService.GetLocalizedAsync(documentCategory, entity => entity.Description, languageId, false, false);
                    locale.MetaKeywords = await _localizationService.GetLocalizedAsync(documentCategory, entity => entity.MetaKeywords, languageId, false, false);
                    locale.MetaDescription = await _localizationService.GetLocalizedAsync(documentCategory, entity => entity.MetaDescription, languageId, false, false);
                    locale.MetaTitle = await _localizationService.GetLocalizedAsync(documentCategory, entity => entity.MetaTitle, languageId, false, false);
                    locale.SeName = await _urlRecordService.GetSeNameAsync(documentCategory, languageId, false, false);
                };
            }

            if (!excludeProperties)
            {
                var categories = await _documentCategoryService.GetAllCategoriesAsync();
                foreach (var category in categories)
                {
                    model.AvailableCategories.Add(new SelectListItem()
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    });
                }
                model.AvailableCategories.Insert(0, new SelectListItem()
                {
                    Value = "0",
                    Text = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Categories.Fields.ParentCategory.None")
                });
            }

            //prepare model stores
            await _storeMappingSupportedModelFactory.PrepareModelStoresAsync(model, documentCategory, excludeProperties);

            return model;
        }

        public async Task<CategoryListModel> PrepareDocumentCategoryListModelAsync(CategorySearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var published = (bool?)null;
            if (searchModel.SearchPublishedId == 1)
                published = true;
            if (searchModel.SearchPublishedId == 2)
                published = false;

            //get categories
            var categories = await _documentCategoryService.GetAllCategoriesAsync(categoryName: searchModel.SearchKeyword,
                storeId: searchModel.SearchStoreId,
                published: published,
                pageIndex: searchModel.Page - 1, 
                pageSize: searchModel.PageSize);

            //prepare grid model
            var model = await new CategoryListModel().PrepareToGridAsync(searchModel, categories, () =>
            {
                return categories.SelectAwait(async category =>
                {
                    return await PrepareDocumentCategoryModelAsync(null, category, true);
                });
            });

            return model;
        }

        public async Task<CategorySearchModel> PrepareDocumentCategorySearchModelAsync(CategorySearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available stores
            await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);

            searchModel.HideStoresList = _catalogSettings.IgnoreStoreLimitations || searchModel.AvailableStores.SelectionIsNotPossible();

            //prepare "published" filter (0 - all; 1 - published only; 2 - unpublished only)
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Categories.List.SearchPublished.All")
            });
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "1",
                Text = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Categories.List.SearchPublished.PublishedOnly")
            });
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "2",
                Text = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Categories.List.SearchPublished.UnpublishedOnly")
            });

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }
    }
}
