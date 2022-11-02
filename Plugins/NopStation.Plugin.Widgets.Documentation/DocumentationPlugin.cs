using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using NopStation.Plugin.Misc.Core;
using NopStation.Plugin.Misc.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NopStation.Plugin.Widgets.Documentation
{
    public class DocumentationPlugin : BasePlugin, IAdminMenuPlugin, INopStationPlugin, IWidgetPlugin
    {
        private readonly IWebHelper _webHelper;
        private readonly INopStationCoreService _nopStationCoreService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;

        public bool HideInWidgetList => false;

        public DocumentationPlugin(IWebHelper webHelper,
            ILocalizationService localizationService,
            INopStationCoreService nopStationCoreService,
            IPermissionService permissionService)
        {
            _webHelper = webHelper;
            _nopStationCoreService = nopStationCoreService;
            _localizationService = localizationService;
            _permissionService = permissionService;
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/Documentation/Configure";
        }

        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                Title = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Menu.Documentation"),
                Visible = true,
                IconClass = "far fa-dot-circle",
            };

            if (await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageConfiguration))
            {
                var configItem = new SiteMapNode()
                {
                    Title = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Menu.Configuration"),
                    Url = "~/Admin/CustomerTracker/List",
                    Visible = true,
                    IconClass = "far fa-circle",
                    SystemName = "CustomerTracker.Customers"
                };
                menuItem.ChildNodes.Add(configItem);
            }

            if (await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationArticles))
            {
                var articleItem = new SiteMapNode()
                {
                    Title = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Menu.Articles"),
                    Url = "~/Admin/DocumentArticle/List",
                    Visible = true,
                    IconClass = "far fa-circle",
                    SystemName = "Documentation.Articles"
                };
                menuItem.ChildNodes.Add(articleItem);

                var commentItem = new SiteMapNode()
                {
                    Title = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Menu.Comments"),
                    Url = "~/Admin/DocumentArticle/CommentList",
                    Visible = true,
                    IconClass = "far fa-circle",
                    SystemName = "Documentation.Comments"
                };
                menuItem.ChildNodes.Add(commentItem);
            }

            if (await _permissionService.AuthorizeAsync(DocumentationPermissionProvider.ManageDocumentationCategories))
            {
                var categoryItem = new SiteMapNode()
                {
                    Title = await _localizationService.GetResourceAsync("Admin.NopStation.Documentation.Menu.Categories"),
                    Url = "~/Admin/DocumentCategory/List",
                    Visible = true,
                    IconClass = "far fa-circle",
                    SystemName = "Documentation.Categories"
                };
                menuItem.ChildNodes.Add(categoryItem);
            }

            await _nopStationCoreService.ManageSiteMapAsync(rootNode, menuItem, NopStationMenuType.Plugin);
        }

        public override async Task InstallAsync()
        {
            await this.InstallPluginAsync(new DocumentationPermissionProvider());
            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await this.UninstallPluginAsync(new DocumentationPermissionProvider());
            await base.UninstallAsync();
        }

        public List<KeyValuePair<string, string>> PluginResouces()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Menu.Documentation", "Documentation"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Menu.Configuration", "Configuration"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Menu.Articles", "Articles"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Menu.Categories", "Categories"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Menu.Comments", "Comments"),

                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Created", "Article created successfully"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Updated", "Article updated successfully"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Deleted", "Article deleted successfully"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Category.Created", "Category created successfully"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Category.Updated", "Category updated successfully"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Category.Deleted", "Category deleted successfully"),

                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.List.SearchPublished.All", "All"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.List.SearchPublished.PublishedOnly", "Published only"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.List.SearchPublished.UnpublishedOnly", "Unpublished only"),

                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.Fields.Customer", "Customer"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.Fields.Article", "Article"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.Fields.CommentText", "Comment text"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.Fields.CreatedOn", "Created on"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.Fields.IsApproved", "Is approved"),

                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.List.Article", "Article"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.List.Article.Hint", "Search by article."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.List.CreatedOnFrom", "Created on from"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.List.CreatedOnFrom.Hint", "Search by created on from date."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.List.CreatedOnTo", "Created on to"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.List.CreatedOnTo.Hint", "Search by created on to date."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.List.SearchText", "Text"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.List.SearchText.Hint", "Search by comment text."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.List.SearchApproved", "Approved"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.List.SearchApproved.Hint", "Search by approve status."),

                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration.Fields.HomepageText", "Homepage text"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration.Fields.HomepageText.Hint", "Enter documentation homepage text."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration.Fields.AllowGuestUsersToLeaveComments", "Allow guest users to leave comments"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration.Fields.AllowGuestUsersToLeaveComments.Hint", "Check to allow guest users to leave comments."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration.Fields.DocumentationCommentsMustBeApproved", "Documentation comments must be approved"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration.Fields.DocumentationCommentsMustBeApproved.Hint", "By checking this article comments will be approved manually by admin."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration.Fields.MinimumIntervalToAddComments", "Minimum interval to add comments"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration.Fields.MinimumIntervalToAddComments.Hint", "Specify minimum interval in seconds to add comments."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration.Fields.ShowLinkOnTopMenu", "Show link on top menu"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration.Fields.ShowLinkOnTopMenu.Hint", "Check to show documentation link on top menu."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration.Fields.ShowLinkInFooterMenu", "Show link in footer menu"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration.Fields.ShowLinkInFooterMenu.Hint", "Check to show documentation link in footer menu."),

                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.Name", "Name"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.Name.Hint", "Documentation article name."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.Description", "Body"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.Description.Hint", "Documentation article body."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.DemoUrl", "Demo url"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.DemoUrl.Hint", "Demo url for related product."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.AdminDemoUrl", "Admin demo url"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.AdminDemoUrl.Hint", "Admin demo url for related product."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.PurchaseUrl", "Purchase url"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.PurchaseUrl.Hint", "Purchase url for related product."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.DisplayOrder", "Display order"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.DisplayOrder.Hint", "The article display order. 1 represents the first item in the list."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.Published", "Published"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.Published.Hint", "Check to mark as published."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.CreatedOn", "Created on"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.CreatedOn.Hint", "Article create date."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.MetaKeywords", "Meta keywords"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.MetaKeywords.Hint", "Meta keywords to be added to article page header."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.MetaDescription", "Meta description"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.MetaDescription.Hint", "Meta description to be added to article page header."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.MetaTitle", "Meta title"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.MetaTitle.Hint", "Override the page title. The default is the title of the article."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.SeName", "Search engine friendly page name"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.SeName.Hint", "Set a search engine friendly page name e.g. 'the-best-article' to make your page URL 'http://www.yourStore.com/the-best-article'. Leave empty to generate it automatically based on the title of the article."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.Categories", "Categories"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.Categories.Hint", "Choose categories. You can manage document categories by selecting Nop Station &gt; Documentation &gt; Categories."),

                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchArticleName", "Article name"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchArticleName.Hint", "Search by article name."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchPublished", "Published"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchPublished.Hint", "Search by publish status."),

                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.Name", "Name"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.Name.Hint", "Documentation category name."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.Description", "Description"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.Description.Hint", "Documentation category description."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.DisplayOrder", "Display order"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.DisplayOrder.Hint", "The documentation category display order. 1 represents the first item in the list."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.Published", "Published"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.Published.Hint", "Check to mark as published."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.MetaKeywords", "Meta keywords"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.MetaKeywords.Hint", "Meta keywords to be added to category page header."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.MetaDescription", "Meta description"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.MetaDescription.Hint", "Meta description to be added to category page header."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.MetaTitle", "Meta title"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.MetaTitle.Hint", "Override the page title. The default is the name of the category."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.SeName", "Search engine friendly page name"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.SeName.Hint", "Set a search engine friendly page name e.g. 'the-best-category' to make your page URL 'http://www.yourStore.com/the-best-category'. Leave empty to generate it automatically based on the name of the category."),

                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.List.SearchCategoryName", "Category name"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.List.SearchCategoryName.Hint", "Search by category name."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.List.SearchPublished", "Published"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.List.SearchPublished.Hint", "Search by publish status."),

                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Info", "Info"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Comments", "Comments"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.List", "Article comments"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List", "Articles"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.AddNew", "Add new article"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.EditDetails", "Edit article details"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.BackToList", "back to article list"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Configuration", "Documentation settings"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Info", "Info"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.List", "Categories"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.AddNew", "Add new category"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.EditDetails", "Edit category details"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.BackToList", "back to category list"),

                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.Title.Required", "Title is required."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.Description.Required", "Body is required."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.Name.Required", "Name is required."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.Description.Required", "Description is required."),

                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.ApproveSelected", "Approve selected"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.DisapproveSelected", "Disapprove selected"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Article.Comments.DeleteSelected", "Delete selected"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.Categories.NoCategoriesAvailable", "No categories available"),

                new KeyValuePair<string, string>("NopStation.Documentation.HomepageTitle", "Knowledgebase"),
                new KeyValuePair<string, string>("NopStation.Documentation.TopMenu.Documentation", "Docs"),
                new KeyValuePair<string, string>("NopStation.Documentation.Footer.Documentation", "Knowledgebase"),
                new KeyValuePair<string, string>("NopStation.Documentation.Menu", "Documentation Menu"),

                new KeyValuePair<string, string>("NopStation.Documentation.Article.Comment.Added", "Article comment successfully added"),
                new KeyValuePair<string, string>("NopStation.Documentation.Article.Comment.SeeAfterApproving", "See article comment after approving"),
                new KeyValuePair<string, string>("NopStation.Documentation.Article.Comments.Fields.CommentText", "Comment Text"),
                new KeyValuePair<string, string>("NopStation.Documentation.Article.Comments.Fields.CommentText.Required", "Article comment text is required"),

                //4.50.1.1
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.UpdatedOn", "UpdatedOn on"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.Fields.UpdatedOn.Hint", "Article update date."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchKeyword", "Keyword"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchKeyword.Hint", "Search by keyword."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchCategory", "Category"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchCategory.Hint", "Search by Category."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchCategory.All", "Search in All Category"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchCategory.All.Hint", "Search by all Categories."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchPublished.All", "All"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchPublished.PublishedOnly", "Published Only"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Articles.List.SearchPublished.UnpublishedOnly", "Unpublished Only"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.List.SearchKeyword", "Keyword"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.ParentCategory", "Parent Category"),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.ParentCategory.Hint", "Select parent category."),
                new KeyValuePair<string, string>("Admin.NopStation.Documentation.Categories.Fields.ParentCategory.None", "None"),
            };

            return list;
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string>
            {
                PublicWidgetZones.HeaderMenuAfter, 
                PublicWidgetZones.Footer
            });
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "Documentation";
        }

        /// <summary>
        /// Update plugin
        /// </summary>
        /// <param name="currentVersion">Current version of plugin</param>
        /// <param name="targetVersion">New version of plugin</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UpdateAsync(string currentVersion, string targetVersion)
        {
            if(targetVersion == "4.50.1.1" && currentVersion != targetVersion)
            {
                //locales
                await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
                {
                    ["Admin.NopStation.Documentation.Articles.Fields.UpdatedOn"] = "UpdatedOn on",
                    ["Admin.NopStation.Documentation.Articles.Fields.UpdatedOn.Hint"] = "Article update date.",
                    ["Admin.NopStation.Documentation.Articles.List.SearchKeyword"] = "Keyword",
                    ["Admin.NopStation.Documentation.Articles.List.SearchKeyword.Hint"] = "Search by keyword.",
                    ["Admin.NopStation.Documentation.Articles.List.SearchCategory"] = "Category",
                    ["Admin.NopStation.Documentation.Articles.List.SearchCategory.Hint"] = "Search by Category.",
                    ["Admin.NopStation.Documentation.Articles.List.SearchCategory.All"] = "Search in All Category",
                    ["Admin.NopStation.Documentation.Articles.List.SearchCategory.All.Hint"] = "Search by all Categories.",
                    ["Admin.NopStation.Documentation.Articles.List.SearchPublished.All"] = "All",
                    ["Admin.NopStation.Documentation.Articles.List.SearchPublished.PublishedOnly"] = "Published Only",
                    ["Admin.NopStation.Documentation.Articles.List.SearchPublished.UnpublishedOnly"] = "Unpublished Only",
                    ["Admin.NopStation.Documentation.Categories.List.SearchKeyword"] = "Keyword",
                    ["Admin.NopStation.Documentation.Categories.Fields.ParentCategory"] = "Parent Category",
                    ["Admin.NopStation.Documentation.Categories.Fields.ParentCategory.Hint"] = "Select parent category.",
                    ["Admin.NopStation.Documentation.Categories.Fields.ParentCategory.None"] = "None",
                });
            }
        }
    }
}
