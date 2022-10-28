using System.Threading.Tasks;
using Nop.Core.Domain.Books;
using Nop.Web.Areas.Admin.Models.Books;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the Book model factory
    /// </summary>
    public partial interface IBookModelFactory
    {
        /// <summary>
        /// Prepare Book search model
        /// </summary>
        /// <param name="searchModel">Book search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Book search model
        /// </returns>
        Task<BookSearchModel> PrepareBookSearchModelAsync(BookSearchModel searchModel);

        /// <summary>
        /// Prepare paged Book list model
        /// </summary>
        /// <param name="searchModel">Book search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Book list model
        /// </returns>
        Task<BookListModel> PrepareBookListModelAsync(BookSearchModel searchModel);

        /// <summary>
        /// Prepare Book model
        /// </summary>
        /// <param name="model">Book model</param>
        /// <param name="Book">Book</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Book model
        /// </returns>
        Task<BookModel> PrepareBookModelAsync(BookModel model, Book Book, bool excludeProperties = false);
 
    }
}