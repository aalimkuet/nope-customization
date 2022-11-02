using System;
using Nop.Core;

namespace NopStation.Plugin.Widgets.Documentation.Domains
{
    public class ArticleComment : BaseEntity
    {
        public int CustomerId { get; set; }

        public int ArticleId { get; set; }

        public string CommentText { get; set; }

        public bool IsApproved { get; set; }

        public DateTime CreatedOnUtc { get; set; }
    }
}
