using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Vendors;

namespace Nop.Data.Mapping.Builders.Books
{
    /// <summary>
    /// Represents a vendor attribute entity builder
    /// </summary>
    public partial class BookAttributeBuilder : NopEntityBuilder<BookAttribute>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(VendorAttribute.Name)).AsString(400).NotNullable();
        }

        #endregion
    }
}