using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Data;
using NopStation.Plugin.Widgets.Documentation.Domains;
using Nop.Services.Stores;
using Nop.Core.Caching;

namespace NopStation.Plugin.Widgets.Documentation.Services
{
    public class DocumentCategoryService : IDocumentCategoryService
    {
        private readonly IRepository<DocumentCategory> _categoryRepository;
        private readonly IRepository<DocumentArticle> _articleRepository;
        private readonly IRepository<ArticleCategory> _articleCategoryRepository;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStaticCacheManager _cacheManager;

        public DocumentCategoryService(IRepository<DocumentCategory> categoryRepository,
            IRepository<DocumentArticle> articleRepository,
            IRepository<ArticleCategory> articleCategoryRepository,
            IStoreMappingService storeMappingService,
            IStaticCacheManager cacheManager)
        {
            _categoryRepository = categoryRepository;
            _articleRepository = articleRepository;
            _articleCategoryRepository = articleCategoryRepository;
            _storeMappingService = storeMappingService;
            _cacheManager = cacheManager;
        }

        public async Task<DocumentCategory> GetCategoryByIdAsync(int categoryId)
        {
            if (categoryId == 0)
                return null;

            return await _categoryRepository.GetByIdAsync(categoryId, cache => default);
        }

        public async Task<IList<DocumentCategory>> GetCategoriesByIdsAsync(int[] categoryIds)
        {
            if (categoryIds == null)
                return new List<DocumentCategory>();

            var query = from dc in _categoryRepository.Table
                        where !dc.Deleted && categoryIds.Contains(dc.Id)
                        select dc;

            return await query.ToListAsync();
        }

        public async Task<IPagedList<DocumentCategory>> GetAllCategoriesAsync(string categoryName = "", bool? published = null,
            int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var cacheKey = _cacheManager.PrepareKeyForDefaultCache(DocumentationDefaults.CategoriesListKey,
                categoryName, storeId, published, pageIndex, pageSize);

            return await _cacheManager.Get(cacheKey, async () =>
            {
                var query = from dc in _categoryRepository.Table
                            where (string.IsNullOrWhiteSpace(categoryName) || dc.Name.Contains(categoryName)) &&
                                (!published.HasValue || dc.Published == published.Value)
                            select dc;

                //apply store mapping constraints
                query = await _storeMappingService.ApplyStoreMapping(query, storeId);

                query = query.OrderBy(dc => dc.DisplayOrder);

                return await query.ToPagedListAsync(pageIndex, pageSize);
            });
        }

        public async Task InsertCategoryAsync(DocumentCategory category)
        {
            await _categoryRepository.InsertAsync(category);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.CategoryCachePrefix);
        }

        public async Task UpdateCategoryAsync(DocumentCategory category)
        {
            //validate category hierarchy
            var parentCategory = await GetCategoryByIdAsync(category.ParentCategoryId);
            while (parentCategory != null)
            {
                if (category.Id == parentCategory.Id)
                {
                    category.ParentCategoryId = 0;
                    break;
                }

                parentCategory = await GetCategoryByIdAsync(parentCategory.ParentCategoryId);
            }

            await _categoryRepository.UpdateAsync(category);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.CategoryCachePrefix);
        }

        public async Task DeleteCategoryAsync(DocumentCategory category)
        {
            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<List<DocumentCategory>> GetCategoriesByParentIdAsync(int parentCategoryId, bool publishedOnly = true)
        {
            var cacheKey = _cacheManager.PrepareKeyForDefaultCache(DocumentationDefaults.CategoriesByParentIdtKey,
                parentCategoryId, publishedOnly);

            return await _cacheManager.GetAsync(cacheKey, async () =>
            {
                var query = from dc in _categoryRepository.Table
                            where dc.ParentCategoryId == parentCategoryId
                                && !dc.Deleted && (!publishedOnly || dc.Published)
                            orderby dc.DisplayOrder
                            select dc;

                return await query.ToListAsync();
            });
        }

        public async Task<IList<ArticleCategory>> GetArticleCategoriesByArticleIdAsync(int articleId, bool showHidden = false)
        {
            if (articleId == 0)
                return new List<ArticleCategory>();

            var cacheKey = _cacheManager.PrepareKeyForDefaultCache(DocumentationDefaults.ArticleCategoriesByArticleIdCacheKey,
                articleId, showHidden);

            return await _cacheManager.GetAsync(cacheKey, async () =>
            {
                var query = from ac in _articleCategoryRepository.Table
                            join c in _categoryRepository.Table on ac.CategoryId equals c.Id
                            join a in _articleRepository.Table on ac.ArticleId equals a.Id
                            where ac.ArticleId == articleId &&
                                  !c.Deleted &&
                                  (showHidden || c.Published)
                            orderby c.DisplayOrder
                            select ac;

                return await query.ToListAsync();
            });
        }

        public async Task<ArticleCategory> GetArticleCategoryByIdAsync(int articleCategoryId)
        {
            if (articleCategoryId == 0)
                return null;

            return await _articleCategoryRepository.GetByIdAsync(articleCategoryId, cache => default);
        }

        public async Task InsertArticleCategoryAsync(ArticleCategory articleCategory)
        {
            await _articleCategoryRepository.InsertAsync(articleCategory);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.CategoryCachePrefix);
        }

        public async Task UpdateArticleCategoryAsync(ArticleCategory articleCategory)
        {
            await _articleCategoryRepository.UpdateAsync(articleCategory);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.CategoryCachePrefix);
        }

        public async Task DeleteArticleCategoryAsync(ArticleCategory articleCategory)
        {
            await _articleCategoryRepository.DeleteAsync(articleCategory);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.CategoryCachePrefix);
        }

        public ArticleCategory FindArticleCategory(IList<ArticleCategory> articleCategories, int articleId, int categoryId)
        {
            if (articleCategories == null)
                return null;

            return articleCategories.FirstOrDefault(x => x.ArticleId == articleId && x.CategoryId == categoryId);
        }

        public async Task<string> GetFormattedBreadCrumbAsync(DocumentCategory category, IList<DocumentCategory> allCategories = null,
           string separator = ">>")
        {
            var result = string.Empty;

            var breadcrumb = await GetCategoryBreadCrumbAsync(category, allCategories, true);
            for (var i = 0; i <= breadcrumb.Count - 1; i++)
            {
                var categoryName = breadcrumb[i].Name;
                result = string.IsNullOrEmpty(result) ? categoryName : $"{result} {separator} {categoryName}";
            }

            return result;
        }

        public async Task<IList<DocumentCategory>> GetCategoryBreadCrumbAsync(DocumentCategory category, IList<DocumentCategory> allCategories = null,
            bool showHidden = false)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var breadcrumbCacheKey = _cacheManager.PrepareKeyForDefaultCache(DocumentationDefaults.CategoryBreadcrumbCacheKey,
                category);

            return await _cacheManager.Get(breadcrumbCacheKey, async () =>
            {
                var result = new List<DocumentCategory>();

                //used to prevent circular references
                var alreadyProcessedCategoryIds = new List<int>();

                while (category != null && //not null
                       !category.Deleted && //not deleted
                       (showHidden || category.Published) && //published
                       !alreadyProcessedCategoryIds.Contains(category.Id)) //prevent circular references
                {
                    result.Add(category);

                    alreadyProcessedCategoryIds.Add(category.Id);

                    category = allCategories != null
                        ? allCategories.FirstOrDefault(c => c.Id == category.ParentCategoryId)
                        : await GetCategoryByIdAsync(category.ParentCategoryId);
                }

                result.Reverse();

                return result;
            });
        }
    }
}
