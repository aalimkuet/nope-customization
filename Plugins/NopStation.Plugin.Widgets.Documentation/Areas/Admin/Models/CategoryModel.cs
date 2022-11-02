using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models
{
    public record CategoryModel : BaseNopEntityModel, IStoreMappingSupportedModel, ILocalizedModel<CategoryLocalizedModel>
    {
        public CategoryModel()
        {
            AvailableCategories = new List<SelectListItem>();

            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
            Locales = new List<CategoryLocalizedModel>();
        }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.ParentCategory")]
        public int ParentCategoryId { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.Published")]
        public bool Published { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.SeName")]
        public string SeName { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }

        public IList<CategoryLocalizedModel> Locales { get; set; }

        public List<SelectListItem> AvailableCategories { get; set; }

        public IList<int> SelectedStoreIds { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
    }

    public partial record CategoryLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Categories.Fields.SeName")]
        public string SeName { get; set; }
    }
}
