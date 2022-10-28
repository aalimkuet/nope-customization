using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Drawing.Charts;
using Nop.Core.Domain.Catalog;
using Nop.Web.Areas.Admin.Models.Common;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Books
{
    /// <summary>
    /// Represents a Book model
    /// </summary>
    public partial record BookModel : BaseNopEntityModel 
    {
        #region Ctor

        public BookModel()
        {
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Books.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Books.Fields.Author")]
        public string Author { get; set; }

        [NopResourceDisplayName("Admin.Books.Fields.PublishDate")]
        public DateTime PublishDate { get; set; }  

        [NopResourceDisplayName("Admin.Books.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }        
        #endregion
    }
     
}