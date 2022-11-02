using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using NopStation.Plugin.Widgets.Documentation.Domains;

namespace NopStation.Plugin.Widgets.Documentation.Data
{
    public class ArticleBuilder : NopEntityBuilder<DocumentArticle>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(DocumentArticle.Name)).AsString(1024).NotNullable()
                .WithColumn(nameof(DocumentArticle.Description)).AsString(10240).NotNullable();
        }
    }
}
