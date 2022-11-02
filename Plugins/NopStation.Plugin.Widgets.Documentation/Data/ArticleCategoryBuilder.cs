using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using NopStation.Plugin.Widgets.Documentation.Domains;

namespace NopStation.Plugin.Widgets.Documentation.Data
{
    public class ArticleCategoryBuilder : NopEntityBuilder<ArticleCategory>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ArticleCategory.ArticleId)).AsInt32().ForeignKey<DocumentArticle>()
                .WithColumn(nameof(ArticleCategory.CategoryId)).AsInt32().ForeignKey<DocumentCategory>();
        }
    }
}
