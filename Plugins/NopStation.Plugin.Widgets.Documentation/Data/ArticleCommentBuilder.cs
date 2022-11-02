using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using NopStation.Plugin.Widgets.Documentation.Domains;
using Nop.Data.Extensions;

namespace NopStation.Plugin.Widgets.Documentation.Data
{
    public class ArticleCommentBuilder : NopEntityBuilder<ArticleComment>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ArticleComment.ArticleId)).AsInt32().ForeignKey<DocumentArticle>()
                .WithColumn(nameof(ArticleComment.CommentText)).AsString(1024).NotNullable();
        }
    }
}
