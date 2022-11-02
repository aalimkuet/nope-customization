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
    public class DocumentArticleService : IDocumentArticleService
    {
        private readonly IRepository<DocumentArticle> _articleRepository;
        private readonly IRepository<ArticleCategory> _articleCategoryRepository;
        private readonly IRepository<ArticleComment> _articleCommentRepository;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStaticCacheManager _cacheManager;

        public DocumentArticleService(IRepository<DocumentArticle> articleRepository,
            IRepository<ArticleCategory> articleCategoryRepository,
            IRepository<ArticleComment> articleCommentRepository,
            IStoreMappingService storeMappingService,
            IStaticCacheManager cacheManager)
        {
            _articleRepository = articleRepository;
            _articleCategoryRepository = articleCategoryRepository;
            _articleCommentRepository = articleCommentRepository;
            _storeMappingService = storeMappingService;
            _cacheManager = cacheManager;
        }

        #region Methods

        #region Articles

        public async Task<DocumentArticle> GetArticleByIdAsync(int articleId)
        {
            return await _articleRepository.GetByIdAsync(articleId, cache => default);
        }

        public async Task<IList<DocumentArticle>> GetArticlesByIdsAsync(ICollection<int> ids)
        {
            if (ids == null)
                return new List<DocumentArticle>();

            return await _articleRepository.Table.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task InsertArticleAsync(DocumentArticle article)
        {
            await _articleRepository.InsertAsync(article);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.ArticleCachePrefix);
        }

        public async Task UpdateArticleAsync(DocumentArticle article)
        {
            await _articleRepository.UpdateAsync(article);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.ArticleCachePrefix);
        }

        public async Task DeleteArticleAsync(DocumentArticle article)
        {
            await _articleRepository.DeleteAsync(article);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.ArticleCachePrefix);
        }

        public async Task DeleteArticleAsync(IList<DocumentArticle> articles)
        {
            await _articleRepository.DeleteAsync(articles);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.ArticleCachePrefix);
        }

        public virtual async Task<IPagedList<DocumentArticle>> GetAllArticlesAsync(string keyword = "", int storeId = 0,
            IList<int> categoryIds = null, int pageIndex = 0, int pageSize = int.MaxValue, bool? published = null)
        {
            var cacheKey = _cacheManager.PrepareKeyForDefaultCache(DocumentationDefaults.ArticlesListKey,
                keyword, storeId, published, categoryIds, pageIndex, pageSize);

            return await _cacheManager.GetAsync(cacheKey, async () =>
            {
                var query = from a in _articleRepository.Table
                            where !a.Deleted
                            select a;

                if (!string.IsNullOrWhiteSpace(keyword))
                    query = from p in query
                            where p.Name.Contains(keyword) || p.Description.Contains(keyword)
                            select p;

                if (published.HasValue)
                    query = from p in query
                            where p.Published == published.Value
                            select p;

                //apply store mapping constraints
                query = await _storeMappingService.ApplyStoreMapping(query, storeId);

                if (categoryIds is not null)
                {
                    if (categoryIds.Contains(0))
                        categoryIds.Remove(0);

                    if (categoryIds.Any())
                    {
                        var documrntArticleCategoryQuery =
                            from pc in _articleCategoryRepository.Table
                            where categoryIds.Contains(pc.CategoryId)
                            select pc;

                        query = from p in query
                                where documrntArticleCategoryQuery.Any(pc => pc.ArticleId == p.Id)
                                select p;
                    }
                }

                query = query.OrderBy(x => x.DisplayOrder);

                return await query.ToPagedListAsync(pageIndex, pageSize);
            });
        }

        #endregion

        #region Article comments

        public async Task InsertArticleCommentAsync(ArticleComment articleComment)
        {
            await _articleCommentRepository.InsertAsync(articleComment);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.ArticleCachePrefix);
        }

        public async Task DeleteArticleCommentAsync(ArticleComment articleComment)
        {
            await _articleCommentRepository.DeleteAsync(articleComment);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.ArticleCachePrefix);
        }

        public async Task UpdateArticleCommentAsync(ArticleComment articleComment)
        {
            await _articleCommentRepository.UpdateAsync(articleComment);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.ArticleCachePrefix);
        }

        public async Task DeleteArticleCommentAsync(IList<ArticleComment> articleComments)
        {
            await _articleCommentRepository.DeleteAsync(articleComments);

            //cache
            await _cacheManager.RemoveByPrefixAsync(DocumentationDefaults.ArticleCachePrefix);
        }

        public async Task<IPagedList<ArticleComment>> GetAllArticleCommentsAsync(int customerId = 0, int articleId = 0, bool? approved = null,
            DateTime? fromUtc = null, DateTime? toUtc = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var cacheKey = _cacheManager.PrepareKeyForDefaultCache(DocumentationDefaults.ArticleCommentsListKey,
                customerId, articleId, approved, fromUtc, toUtc, pageIndex, pageSize);

            return await _cacheManager.Get(cacheKey, async () =>
            {
                var query = from ac in _articleCommentRepository.Table
                            where (customerId == 0 || ac.CustomerId == customerId) &&
                                (articleId == 0 || ac.ArticleId == articleId) &&
                                (!approved.HasValue || ac.IsApproved == approved.Value) &&
                                (!fromUtc.HasValue || ac.CreatedOnUtc >= fromUtc.Value) &&
                                (!toUtc.HasValue || ac.CreatedOnUtc <= toUtc.Value)
                            select ac;

                query = query.OrderByDescending(x => x.CreatedOnUtc);

                return await query.ToPagedListAsync(pageIndex, pageSize);
            });
        }

        public async Task<ArticleComment> GetArticleCommentByIdAsync(int id)
        {
            return await _articleCommentRepository.GetByIdAsync(id, cache => default);
        }

        public async Task<IList<ArticleComment>> GetArticleCommentsByIdsAsync(ICollection<int> ids, bool? approved = null)
        {
            if (ids == null)
                return new List<ArticleComment>();

            var query = from ac in _articleCommentRepository.Table
                        where ids.Contains(ac.Id) &&
                            (!approved.HasValue || ac.IsApproved == approved.Value)
                        select ac;

            var comments = await query.ToListAsync();

            var commentList = new List<ArticleComment>();
            foreach (var id in ids)
                if (comments.FirstOrDefault(x => x.Id == id) is ArticleComment comment)
                    commentList.Add(comment);

            return commentList;
        }

        #endregion

        #endregion
    }
}
