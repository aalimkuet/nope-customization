using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Catalog;
using NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models;
using NopStation.Plugin.Widgets.Documentation.Domains;
using NopStation.Plugin.Widgets.Documentation.Services;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;
using Nop.Core.Caching;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Factories
{
    public class DocumentArticleModelFactory : IDocumentArticleModelFactory
    {
        #region Fields

        private readonly IDocumentArticleService _documentArticleService;
        private readonly IDocumentCategoryService _documentCategoryService;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly CatalogSettings _catalogSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ICustomerService _customerService;
        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public DocumentArticleModelFactory(IDocumentArticleService documentArticleService,
            IDocumentCategoryService documentCategoryService,
            IBaseAdminModelFactory baseAdminModelFactory,
            CatalogSettings catalogSettings,
            ILocalizationService localizationService,
            IUrlRecordService urlRecordService,
            IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
            IDateTimeHelper dateTimeHelper,
            ICustomerService customerService,
            IStaticCacheManager cacheManager)
        {
            _documentArticleService = documentArticleService;
            _documentCategoryService = documentCategoryService;
            _baseAdminModelFactory = baseAdminModelFactory;
            _catalogSettings = catalogSettings;
            _localizationService = localizationService;
            _urlRecordService = urlRecordService;
            _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
            _dateTimeHelper = dateTimeHelper;
            _customerService = customerService;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Utilities

        protected virtual async Task<List<SelectListItem>> GetCategoryListAsync(bool showHidden = true)
        {
            var cacheKey = _cacheManager.PrepareKeyForDefaultCache(DocumentationDefaults.CategoriesSelectListKey, showHidden);
            var listItems = await _cacheManager.GetAsync(cacheKey, async () =>
            {
                var categories = await _documentCategoryService.GetAllCategoriesAsync();
                var listItems = new List<SelectListItem>();
                foreach (var category in categories)
                {
                    var listItem = new SelectListItem
                    {
                        Text = await _documentCategoryService.GetFormattedBreadCrumbAsync(category, categories),
                        Value = category.Id.ToString()
                    };
                    listItems.Add(listItem);
                }
                return listItems;
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

        protected virtual async Task PrepareCategoriesAsync(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available categories
            var availableCategoryItems = await GetCategoryListAsync();
            foreach (var categoryItem in availableCategoryItems)
            {
                items.Add(categoryItem);
            }

            //whether to insert the first special item for the default value
            if (!withSpecialDefaultItem)
                return;

            //at now we use "0" as the default value
            const string value = "0";

            //prepare item text
            defaultItemText ??= await _localizationService.GetResourceAsync("Admin.Common.All");

            //insert this default item at first
            items.Insert(0, new SelectListItem { Text = defaultItemText, Value = value });
        }

        #endregion

        #region Methods

        #region Articles

        public async Task<ArticleModel> PrepareArticleModelAsync(ArticleModel model, DocumentArticle article, bool excludeProperties = false)
        {
            Func<ArticleLocalizedModel, int, Task> localizedModelConfiguration = null;

            if (article != null)
            {
                if (model == null)
                {
                    model = article.ToModel<ArticleModel>();
                    model.SeName = await _urlRecordService.GetSeNameAsync(article.Id,DocumentationDefaults.DocumentArticle, 0, true, false);
                }

                model.CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(article.CreatedOnUtc, DateTimeKind.Utc);
                model.UpdatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(article.UpdatedOnUtc, DateTimeKind.Utc);

                model.SelectedCategoryIds = (await _documentCategoryService.GetArticleCategoriesByArticleIdAsync(article.Id, true))
                    .Select(productCategory => productCategory.CategoryId).ToList();

                //define localized model configuration action
                localizedModelConfiguration = async (locale, languageId) =>
                {
                    locale.Name = await _localizationService.GetLocalizedAsync(article, entity => entity.Name, languageId, false, false);
                    locale.Description = await _localizationService.GetLocalizedAsync(article, entity => entity.Description, languageId, false, false);
                    locale.MetaKeywords = await _localizationService.GetLocalizedAsync(article, entity => entity.MetaKeywords, languageId, false, false);
                    locale.MetaDescription = await _localizationService.GetLocalizedAsync(article, entity => entity.MetaDescription, languageId, false, false);
                    locale.MetaTitle = await _localizationService.GetLocalizedAsync(article, entity => entity.MetaTitle, languageId, false, false);
                    locale.SeName = await _urlRecordService.GetSeNameAsync(article, languageId, false, false);
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

                foreach (var categoryItem in model.AvailableCategories)
                {
                    categoryItem.Selected = int.TryParse(categoryItem.Value, out var categoryId)
                        && model.SelectedCategoryIds.Contains(categoryId);
                }

                //prepare model stores
                await _storeMappingSupportedModelFactory.PrepareModelStoresAsync(model, article, excludeProperties);

                await PrepareArticleCommentSearchModelAsync(model.ArticleCommentSearchModel, article);
            }

            return model;
        }

        public async Task<ArticleListModel> PrepareArticleListModelAsync(ArticleSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var published = (bool?)null;
            if (searchModel.SearchPublishedId == 1)
                published = true;
            if (searchModel.SearchPublishedId == 2)
                published = false;

            //get categories
            var articles = await _documentArticleService.GetAllArticlesAsync(keyword: searchModel.SearchKeyword,
                storeId: searchModel.SearchStoreId,
                categoryIds: new List<int> { searchModel.SearchCategoryId },
                published: published,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            //prepare grid model
            var model = await new ArticleListModel().PrepareToGridAsync(searchModel, articles, () =>
            {
                return articles.SelectAwait(async category =>
                {
                    return await PrepareArticleModelAsync(null, category, true);
                });
            });

            return model;
        }

        public async Task<ArticleSearchModel> PrepareArticleSearchModelAsync(ArticleSearchModel searchModel)
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
                Text = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Articles.List.SearchPublished.All")
            });
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "1",
                Text = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Articles.List.SearchPublished.PublishedOnly")
            });
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "2",
                Text = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Articles.List.SearchPublished.UnpublishedOnly")
            });

            var categories = await _documentCategoryService.GetAllCategoriesAsync();
            foreach (var category in categories)
            {
                searchModel.AvailableCategories.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            }
            searchModel.AvailableCategories.Insert(0, new SelectListItem()
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Articles.List.SearchCategory.All")
            });

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        #endregion

        #region Article comments

        public async Task<ArticleCommentListModel> PrepareArticleCommentListModelAsync(ArticleCommentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var isApprovedOnly = searchModel.SearchApprovedId == 0 ? null : searchModel.SearchApprovedId == 1 ? true : (bool?)false;

            var fromUtc = (DateTime?)null;
            var toUtc = (DateTime?)null;
            if (searchModel.CreatedOnFrom.HasValue)
                fromUtc = _dateTimeHelper.ConvertToUtcTime(searchModel.CreatedOnFrom.Value);
            if (searchModel.CreatedOnTo.HasValue)
                toUtc = _dateTimeHelper.ConvertToUtcTime(searchModel.CreatedOnTo.Value);

            //get categories
            var comments = await _documentArticleService.GetAllArticleCommentsAsync(
                articleId: searchModel.SearchArticleId,
                approved: isApprovedOnly,
                fromUtc: fromUtc,
                toUtc: toUtc,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            //prepare grid model
            var model = await new ArticleCommentListModel().PrepareToGridAsync(searchModel, comments, () =>
            {
                return comments.SelectAwait(async comment =>
                {
                    return await PrepareArticleCommentModelAsync(null, comment);
                });
            });

            return model;
        }

        public async Task<ArticleCommentSearchModel> PrepareArticleCommentSearchModelAsync(ArticleCommentSearchModel searchModel, DocumentArticle article)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            searchModel.SearchArticleId = article?.Id ?? 0;
            searchModel.AvailableArticles = (await _documentArticleService.GetAllArticlesAsync())
                .Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public async Task<ArticleCommentModel> PrepareArticleCommentModelAsync(ArticleCommentModel model, ArticleComment comment)
        {
            if (comment != null)
            {
                if (model == null)
                {
                    model = comment.ToModel<ArticleCommentModel>();
                }

                model.CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(comment.CreatedOnUtc, DateTimeKind.Utc);
                model.CustomerName = await _customerService.GetCustomerFullNameAsync(await _customerService.GetCustomerByIdAsync(comment.CustomerId));
                model.Article = (await _documentArticleService.GetArticleByIdAsync(comment.ArticleId)).Name;
            }

            return model;
        }

        #endregion

        #endregion
    }
}
