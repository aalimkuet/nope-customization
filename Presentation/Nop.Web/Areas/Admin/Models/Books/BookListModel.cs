using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Books
{
    /// <summary>
    /// Represents a vendor list model
    /// </summary>
    public partial record BookListModel : BasePagedListModel<BookModel>
    {
    }
}