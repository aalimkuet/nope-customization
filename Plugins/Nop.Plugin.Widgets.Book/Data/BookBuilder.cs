using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.BookTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.BookTracker.Data
{
    public class BookBuilder : NopEntityBuilder<Book>
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
