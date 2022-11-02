using System;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models
{
    public record ArticleCommentModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.NopStation.Documentation.Article.Comments.Fields.Customer")]
        public int CustomerId { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Article.Comments.Fields.Customer")]
        public string CustomerName { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Article.Comments.Fields.Article")]
        public int ArticleId { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Article.Comments.Fields.Article")]
        public string Article { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Article.Comments.Fields.CommentText")]
        public string CommentText { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Article.Comments.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Article.Comments.Fields.IsApproved")]
        public bool IsApproved { get; set; }
    }
}
