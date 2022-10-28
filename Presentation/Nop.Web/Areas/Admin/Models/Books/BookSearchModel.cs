using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Books
{
    /// <summary>
    /// Represents a book search model
    /// </summary>
    public partial record BookSearchModel : BaseSearchModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.Books.List.SearchName")]
        public string SearchName { get; set; }

        [NopResourceDisplayName("Admin.Books.List.SearchAuthor")]
        public string SearchAuthor { get; set; }

        #endregion
    }
}