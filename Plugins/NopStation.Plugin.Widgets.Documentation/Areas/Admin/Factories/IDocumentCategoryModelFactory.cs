using System.Threading.Tasks;
using NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models;
using NopStation.Plugin.Widgets.Documentation.Domains;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Factories
{
    public interface IDocumentCategoryModelFactory
    {
        Task<CategoryModel> PrepareDocumentCategoryModelAsync(CategoryModel documentCategoryModel, DocumentCategory documentCategory, bool excludeProperties = false);

        Task<CategorySearchModel> PrepareDocumentCategorySearchModelAsync(CategorySearchModel searchModel);

        Task<CategoryListModel> PrepareDocumentCategoryListModelAsync(CategorySearchModel searchModel);
    }
}
