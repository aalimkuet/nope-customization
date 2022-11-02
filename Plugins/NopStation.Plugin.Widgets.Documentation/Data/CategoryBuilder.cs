using System;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using NopStation.Plugin.Widgets.Documentation.Domains;

namespace NopStation.Plugin.Widgets.Documentation.Data
{
    public class CategoryBuilder : NopEntityBuilder<DocumentCategory>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(DocumentCategory.Name)).AsString(1024).NotNullable()
                .WithColumn(nameof(DocumentCategory.Description)).AsString(10240);
        }
    }
}
