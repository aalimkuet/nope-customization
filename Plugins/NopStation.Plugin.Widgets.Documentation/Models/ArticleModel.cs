using System;
using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace NopStation.Plugin.Widgets.Documentation.Models
{
    public record ArticleModel : BaseNopEntityModel
    {
        public ArticleModel()
        {
            NewComment = new ArticleCommentModel();
            Comments = new List<ArticleCommentModel>();
        }

        public string SeName { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public string MetaTitle { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<ArticleCommentModel> Comments { get; set; }

        public ArticleCommentModel NewComment { get; set; }
    }
}
