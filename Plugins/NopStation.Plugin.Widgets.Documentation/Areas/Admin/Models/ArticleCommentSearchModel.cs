using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models
{
    public record ArticleCommentSearchModel : BaseSearchModel
    {
        #region Ctor

        public ArticleCommentSearchModel()
        {
            AvailableApprovedOptions = new List<SelectListItem>();
            AvailableArticles = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.NopStation.Documentation.Article.Comments.List.Article")]
        public int SearchArticleId { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Article.Comments.List.CreatedOnFrom")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Article.Comments.List.CreatedOnTo")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Article.Comments.List.SearchText")]
        public string SearchText { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Article.Comments.List.SearchApproved")]
        public int SearchApprovedId { get; set; }

        public IList<SelectListItem> AvailableApprovedOptions { get; set; }
        public IList<SelectListItem> AvailableArticles { get; set; }

        #endregion
    }
}
