using System;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace NopStation.Plugin.Widgets.Documentation.Models
{
    public record ArticleCommentModel: BaseNopEntityModel
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int ArticleId { get; set; }

        [NopResourceDisplayName("NopStation.Documentation.Article.Comments.Fields.CommentText")]
        public string CommentText { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CategoryId { get; set; }
    }
}
