using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NopStation.Plugin.Widgets.Documentation.Domains;
using NopStation.Plugin.Widgets.Documentation.Models;
using NopStation.Plugin.Widgets.Documentation.Services;
using Nop.Services.Seo;
using Nop.Web.Framework.Components;
using System.Threading.Tasks;

namespace NopStation.Plugin.Widgets.Documentation.Components
{
    public class DocumentationCategoryViewComponent : NopViewComponent
    {
        private readonly IUrlRecordService _urlRecordService;
        private readonly IDocumentCategoryService _documentCategoryService;
        private readonly IDocumentArticleService _documentArticleService;

        public DocumentationCategoryViewComponent(IUrlRecordService urlRecordService,
            IDocumentCategoryService documentCategoryService,
            IDocumentArticleService documentArticleService)
        {
            _urlRecordService = urlRecordService;
            _documentCategoryService = documentCategoryService;
            _documentArticleService = documentArticleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId, int articleId)
        {
            var model = new NavigationModel();
            model.CurrentCategoryId = categoryId;
            model.CurrentArticleId = articleId;

            var categoryIds = await GetSelectedCategoryIdsAsync(categoryId);

            var categories = await _documentCategoryService.GetCategoriesByParentIdAsync(0);
            foreach (var category in categories)
                model.MenuItems.Add(await GenerateCategoryModelAsync(category, articleId, categoryIds, 0));

            return View(model);
        }

        private async Task<IList<int>> GetSelectedCategoryIdsAsync(int categoryId)
        {
            if (categoryId == 0)
                return new List<int>();

            var cids = new List<int>();
            while (true)
            {
                var category = await _documentCategoryService.GetCategoryByIdAsync(categoryId);
                if (category == null || category.Deleted)
                    break;

                cids.Add(category.Id);
                categoryId = category.ParentCategoryId;
            }

            return cids;
        }

        private async Task<CategoryOverviewModel> GenerateCategoryModelAsync(DocumentCategory category, int articleId, IList<int> cids, int menuLevel)
        {
            var categoryModel = new CategoryOverviewModel()
            {
                Id = category.Id,
                Name = category.Name,
                SeName = await _urlRecordService.GetSeNameAsync(category),
                Selected = cids.Contains(category.Id),
                MenuLevel = menuLevel++
            };

            var subCategories = await _documentCategoryService.GetCategoriesByParentIdAsync(category.Id);
            foreach (var subCategory in subCategories)
                categoryModel.SubCategoryList.Add(await GenerateCategoryModelAsync(subCategory, articleId, cids, menuLevel));

            var articles = await _documentArticleService.GetAllArticlesAsync(published: true, categoryIds: new List<int> { category.Id });
            foreach (var article in articles)
            {
                categoryModel.ArticleList.Add(new ArticleOverviewModel()
                {
                    CategoryId = category.Id,
                    Id = article.Id,
                    Selected = categoryModel.Selected && article.Id == articleId,
                    Name = article.Name,
                    SeName = await _urlRecordService.GetSeNameAsync(article)
                });
            }

            return categoryModel;
        }
    }
}