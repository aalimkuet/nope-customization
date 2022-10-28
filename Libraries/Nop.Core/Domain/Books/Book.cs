using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;
using System;
using System.ComponentModel;

namespace Nop.Core.Domain.Books
{
    /// <summary>
    /// Represents a vendor
    /// </summary>
    public partial class Book : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }
        public string Author { get; set; }
        [DisplayName("Publish Date")]
        public DateTime PublishDate { get; set; }
              
        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        
    }
}
