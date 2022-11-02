using System.Threading.Tasks;
using NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models;
using NopStation.Plugin.Widgets.Documentation.Domains;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Factories
{
    public interface IDocumentArticleModelFactory
    {
        Task<ArticleModel> PrepareArticleModelAsync(ArticleModel model, DocumentArticle article, bool excludeProperties = false);

        Task<ArticleSearchModel> PrepareArticleSearchModelAsync(ArticleSearchModel searchModel);

        Task<ArticleListModel> PrepareArticleListModelAsync(ArticleSearchModel searchModel);

        Task<ArticleCommentListModel> PrepareArticleCommentListModelAsync(ArticleCommentSearchModel searchModel);

        Task<ArticleCommentSearchModel> PrepareArticleCommentSearchModelAsync(ArticleCommentSearchModel searchModel, DocumentArticle article);

        Task<ArticleCommentModel> PrepareArticleCommentModelAsync(ArticleCommentModel model, ArticleComment comment);
    }
}
