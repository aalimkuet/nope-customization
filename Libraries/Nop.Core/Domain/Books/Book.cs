using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;
using System;

namespace Nop.Core.Domain.Books
{
    /// <summary>
    /// Represents a vendor
    /// </summary>
    public partial class Book : BaseEntity, ILocalizedEntity, ISlugSupported, ISoftDeletedEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public bool Deleted { get; set; }
    }
}
