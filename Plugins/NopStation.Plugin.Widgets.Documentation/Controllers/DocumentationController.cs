using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using NopStation.Plugin.Misc.Core.Controllers;
using NopStation.Plugin.Widgets.Documentation.Domains;
using NopStation.Plugin.Widgets.Documentation.Models;
using NopStation.Plugin.Widgets.Documentation.Services;
using Nop.Services.Customers;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework;
using System.Linq;
using Nop.Services.Helpers;
using Nop.Services.Seo;
using Nop.Services.Localization;

namespace NopStation.Plugin.Widgets.Documentation.Controllers
{
    public class DocumentationController : NopStationPublicController
    {
        private readonly IDocumentCategoryService _documentCategoryService;
        private readonly IDocumentArticleService _documentArticleService;
        private readonly IWorkContext _workContext;
        private readonly DocumentationSettings _documentationSettings;
        private readonly ICustomerService _customerService;
        private readonly IPermissionService _permissionService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;

        public DocumentationController(IDocumentCategoryService documentCategoryService,
            IDocumentArticleService documentArticleService,
            IWorkContext workContext,
            DocumentationSettings documentationSettings,
            ICustomerService customerService,
            IPermissionService permissionService,
            IUrlRecordService urlRecordService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService)
        {
            _documentCategoryService = documentCategoryService;
            _documentArticleService = documentArticleService;
            _workContext = workContext;
            _documentationSettings = documentationSettings;
            _customerService = customerService;
            _permissionService = permissionService;
            _urlRecordService = urlRecordService;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
        }

        public IActionResult Home()
        {
            var model = new DocumentationModel
            {
                HomepageText = _documentationSettings.HomepageText
            };
            return View(model);
        }

        public async Task<IActionResult> Article(int articleId, int categoryId)
        {
            var article = await _documentArticleService.GetArticleByIdAsync(articleId);
            if (article == null || article.Deleted)
                return InvokeHttp404();

            var notAvailable = !article.Published;

            //Check whether the current user has a "Manage blog" permission (usually a store owner)
            //We should allows him (her) to use "Preview" functionality
            var hasAdminAccess = await _permissionService.AuthorizeAsync(StandardPermissionProvider.AccessAdminPanel) && await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles);
            if (notAvailable && !hasAdminAccess)
                return InvokeHttp404();

            //display "edit" (manage) link
            if (hasAdminAccess)
                DisplayEditLink(Url.Action("Edit", "DocumentArticle", new { id = article.Id, area = AreaNames.Admin }));

            var articleCategories = await _documentCategoryService.GetArticleCategoriesByArticleIdAsync(articleId);
            categoryId = articleCategories.Any(x => x.CategoryId == categoryId) ? categoryId :
                articleCategories.FirstOrDefault()?.CategoryId ?? 0;

            var model = new ArticleModel()
            {
                Description = await _localizationService.GetLocalizedAsync(article, x => x.Description),
                Name = await _localizationService.GetLocalizedAsync(article, x => x.Name),
                CategoryId = categoryId,
                CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(article.CreatedOnUtc),
                Id = article.Id,
                SeName = await _urlRecordService.GetSeNameAsync(article, 0, true, false),
                MetaDescription = await _localizationService.GetLocalizedAsync(article, x => x.MetaDescription),
                MetaKeywords = await _localizationService.GetLocalizedAsync(article, x => x.MetaKeywords),
                MetaTitle = await _localizationService.GetLocalizedAsync(article, x => x.MetaTitle)
            };

            var comments = await _documentArticleService.GetAllArticleCommentsAsync(articleId: article.Id, approved: true);
            foreach (var comment in comments)
            {
                model.Comments.Add(new ArticleCommentModel()
                {
                    ArticleId = article.Id,
                    CommentText = comment.CommentText,
                    CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(comment.CreatedOnUtc),
                    CustomerName = await _customerService.GetCustomerFullNameAsync(await _customerService.GetCustomerByIdAsync(comment.CustomerId)),
                    Id = comment.Id,
                    CategoryId = categoryId
                });
            }

            return View(model);
        }

        [HttpPost, ActionName("Article")]
        [FormValueRequired("add-comment")]
        public async Task<IActionResult> ArticleCommentAdd(ArticleModel model)
        {
            var article = await _documentArticleService.GetArticleByIdAsync(model.Id);
            if (article == null || article.Deleted)
                return InvokeHttp404();

            var notAvailable = !article.Published;

            //Check whether the current user has a "Manage blog" permission (usually a store owner)
            //We should allows him (her) to use "Preview" functionality
            var hasAdminAccess = await _permissionService.AuthorizeAsync(StandardPermissionProvider.AccessAdminPanel) && await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles);
            if (notAvailable && !hasAdminAccess)
                return InvokeHttp404();

            if (!_documentationSettings.AllowGuestUsersToLeaveComments &&
                !await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return RedirectToRoute("DocumentArticle", new { SeName = await _urlRecordService.GetSeNameAsync(article), categoryId = model.CategoryId });

            var comment = (await _documentArticleService.GetAllArticleCommentsAsync(
                customerId: (await _workContext.GetCurrentCustomerAsync()).Id,
                pageSize: 1)).FirstOrDefault();

            if (comment != null && comment.CreatedOnUtc.AddSeconds(_documentationSettings.MinimumIntervalToAddComments) > DateTime.UtcNow)
                return RedirectToRoute("DocumentArticle", new { SeName = _urlRecordService.GetSeNameAsync(article), categoryId = model.CategoryId });

            if (ModelState.IsValid)
            {
                var newComment = new ArticleComment
                {
                    ArticleId = model.Id,
                    CustomerId = (await _workContext.GetCurrentCustomerAsync()).Id,
                    CommentText = model.NewComment.CommentText,
                    CreatedOnUtc = DateTime.UtcNow,
                    IsApproved = !_documentationSettings.DocumentationCommentsMustBeApproved
                };

                TempData["nopstation.document.addcomment.result"] = newComment.IsApproved
                    ? _localizationService.GetResourceAsync("NopStation.Documentation.Article.Comment.Added")
                    : _localizationService.GetResourceAsync("NopStation.Documentation.Article.Comment.SeeAfterApproving");
                await _documentArticleService.InsertArticleCommentAsync(newComment);
            }

            return RedirectToRoute("DocumentArticle", new { seName = _urlRecordService.GetSeNameAsync(article), categoryId = model.CategoryId });
        }

        public async Task<IActionResult> Category(int categoryId)
        {
            var category = await _documentCategoryService.GetCategoryByIdAsync(categoryId);
            if (category == null || category.Deleted)
                return InvokeHttp404();

            var notAvailable = !category.Published;

            //Check whether the current user has a "Manage blog" permission (usually a store owner)
            //We should allows him (her) to use "Preview" functionality
            var hasAdminAccess = await _permissionService.AuthorizeAsync(StandardPermissionProvider.AccessAdminPanel) && await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationCategories);
            if (notAvailable && !hasAdminAccess)
                return InvokeHttp404();

            //display "edit" (manage) link
            if (hasAdminAccess)
                DisplayEditLink(Url.Action("Edit", "DocumentCategory", new { id = category.Id, area = AreaNames.Admin }));

            var model = new CategoryDetailsModel()
            {
                Description = await _localizationService.GetLocalizedAsync(category, x => x.Description),
                Id = category.Id,
                Name = await _localizationService.GetLocalizedAsync(category, x => x.Name),
                SeName = await _urlRecordService.GetSeNameAsync(category, 0, true, false),
                MetaDescription = await _localizationService.GetLocalizedAsync(category, x => x.MetaDescription),
                MetaKeywords = await _localizationService.GetLocalizedAsync(category, x => x.MetaKeywords),
                MetaTitle = await _localizationService.GetLocalizedAsync(category, x => x.MetaTitle)
            };

            return View(model);
        }
    }
}
