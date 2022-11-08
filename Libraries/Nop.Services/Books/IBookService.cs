using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Books;

namespace Nop.Services.Books
{
    /// <summary>
    /// Book service interface
    /// </summary>
    public partial interface IBookService
    {
        /// <summary>
        /// Gets a Book by Book identifier
        /// </summary>
        /// <param name="BookId">Book identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Book
        /// </returns>
        Task<Book> GetBookByIdAsync(int BookId);

        /// <summary>
        /// Delete a Book
        /// </summary>
        /// <param name="Book">Book</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteBookAsync(Book Book);

        /// <summary>
        /// Gets all Books
        /// </summary>
        /// <param name="name">Book name</param>
        /// <param name="email">Book email</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Books
        /// </returns>
        Task<IPagedList<Book>> GetAllBooksAsync(string name = "", string author = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Inserts a Book
        /// </summary>
        /// <param name="Book">Book</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertBookAsync(Book Book);

        /// <summary>
        /// Updates the Book
        /// </summary>
        /// <param name="Book">Book</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateBookAsync(Book Book);
         Task<IList<Book>> GetAllBookList(Book book);
    }
}