using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models
{
    public record ArticleModel : BaseNopEntityModel, IStoreMappingSupportedModel, ILocalizedModel<ArticleLocalizedModel>
    {
        public ArticleModel()
        {
            SelectedCategoryIds = new List<int>();
            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
            Locales = new List<ArticleLocalizedModel>();
            ArticleCommentSearchModel = new ArticleCommentSearchModel();
        }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.Published")]
        public bool Published { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.SeName")]
        public string SeName { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }

        //categories
        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.Categories")]
        public IList<int> SelectedCategoryIds { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        public IList<int> SelectedStoreIds { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        public IList<ArticleLocalizedModel> Locales { get; set; }

        public ArticleCommentSearchModel ArticleCommentSearchModel { get; set; }
    }

    public partial record ArticleLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Articles.Fields.SeName")]
        public string SeName { get; set; }
    }
}
