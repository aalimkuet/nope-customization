using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using NopStation.Plugin.Widgets.Documentation.Domains;

namespace NopStation.Plugin.Widgets.Documentation.Services
{
    public interface IDocumentCategoryService
    {
        Task<DocumentCategory> GetCategoryByIdAsync(int categoryId);

        Task<IList<DocumentCategory>> GetCategoriesByIdsAsync(int[] categoryIds);

        Task InsertCategoryAsync(DocumentCategory category);

        Task UpdateCategoryAsync(DocumentCategory category);

        Task DeleteCategoryAsync(DocumentCategory category);

        Task<IPagedList<DocumentCategory>> GetAllCategoriesAsync(string categoryName = "", bool? published = null,
            int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<List<DocumentCategory>> GetCategoriesByParentIdAsync(int parentCategoryId, bool publishedOnly = true);

        Task<IList<ArticleCategory>> GetArticleCategoriesByArticleIdAsync(int articleId, bool showHidden = false);

        Task<ArticleCategory> GetArticleCategoryByIdAsync(int articleCategoryId);

        Task InsertArticleCategoryAsync(ArticleCategory articleCategory);

        Task UpdateArticleCategoryAsync(ArticleCategory articleCategory);

        Task DeleteArticleCategoryAsync(ArticleCategory articleCategory);

        ArticleCategory FindArticleCategory(IList<ArticleCategory> articleCategories, int articleId, int categoryId);

        Task<string> GetFormattedBreadCrumbAsync(DocumentCategory category, IList<DocumentCategory> allCategories = null,
           string separator = ">>");

        Task<IList<DocumentCategory>> GetCategoryBreadCrumbAsync(DocumentCategory category, IList<DocumentCategory> allCategories = null,
            bool showHidden = false);
    }
}
