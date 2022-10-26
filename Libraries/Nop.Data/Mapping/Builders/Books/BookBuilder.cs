using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Books;
using Nop.Core.Domain.Vendors;

namespace Nop.Data.Mapping.Builders.Books
{
    /// <summary>
    /// Represents a vendor entity builder
    /// </summary>
    public partial class BookBuilder : NopEntityBuilder<Book>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Book.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(Book.Author)).AsString(400).Nullable()
                .WithColumn(nameof(Book.PublishDate)).AsString(400).Nullable();
        }

        #endregion
    }
}