using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.BookTracker.Domain;

namespace Nop.Plugin.Widgets.Customer.Data
{
    public class CustomerTrackerBuilder : NopEntityBuilder<CustomerTracker>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            //map the primary key (not necessary if it is Id field)
            //table.WithColumn(nameof(Book.Id)).AsInt32().PrimaryKey();
            //so we set the same max length used in the product name
            //.WithColumn(nameof(Book.Name)).AsString()
            //.WithColumn(nameof(Book.Author)).AsString()
            //.WithColumn(nameof(Book.PublishedDate)).AsDateTime();
          
        }
    }
}
