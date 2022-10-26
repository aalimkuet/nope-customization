using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Vendors;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Books
{
    /// <summary>
    /// Represents a vendor attribute value entity builder
    /// </summary>
    public partial class BookAttributeValueBuilder : NopEntityBuilder<BookAttributeValue>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(BookAttributeValue.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(BookAttributeValue.VendorAttributeId)).AsInt32().ForeignKey<VendorAttribute>();
        }

        #endregion
    }
}