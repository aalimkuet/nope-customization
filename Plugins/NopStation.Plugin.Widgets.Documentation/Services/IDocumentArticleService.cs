using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using NopStation.Plugin.Widgets.Documentation.Domains;

namespace NopStation.Plugin.Widgets.Documentation.Services
{
    public interface IDocumentArticleService
    {
        #region Articles

        Task<DocumentArticle> GetArticleByIdAsync(int articleId);

        Task<IList<DocumentArticle>> GetArticlesByIdsAsync(ICollection<int> ids);

        Task InsertArticleAsync(DocumentArticle article);

        Task UpdateArticleAsync(DocumentArticle article);

        Task DeleteArticleAsync(DocumentArticle article);

        Task DeleteArticleAsync(IList<DocumentArticle> articles);

        Task<IPagedList<DocumentArticle>> GetAllArticlesAsync(string keyword = "", int storeId = 0,
            IList<int> categoryIds = null, int pageIndex = 0, int pageSize = int.MaxValue, bool? published = null);

        #endregion

        #region Article comments

        Task<ArticleComment> GetArticleCommentByIdAsync(int id);

        Task<IList<ArticleComment>> GetArticleCommentsByIdsAsync(ICollection<int> ids, bool? approved = null);

        Task InsertArticleCommentAsync(ArticleComment articleComment);

        Task DeleteArticleCommentAsync(ArticleComment articleComment);

        Task UpdateArticleCommentAsync(ArticleComment articleComment);

        Task DeleteArticleCommentAsync(IList<ArticleComment> articleComments);

        Task<IPagedList<ArticleComment>> GetAllArticleCommentsAsync(int customerId = 0, int articleId = 0, bool? approved = null,
            DateTime? fromUtc = null, DateTime? toUtc = null, int pageIndex = 0, int pageSize = int.MaxValue);

        #endregion
    }
}
