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
        /// Gets a Books by product identifiers
        /// </summary>
        /// <param name="productIds">Array of product identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Books
        /// </returns>
        Task<IList<Book>> GetBooksByProductIdsAsync(int[] productIds);

        /// <summary>
        /// Gets a Books by customers identifiers
        /// </summary>
        /// <param name="customerIds">Array of customer identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Books
        /// </returns>
        Task<IList<Book>> GetBooksByCustomerIdsAsync(int[] customerIds);

        /// <summary>
        /// Gets a Book by product identifier
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Book
        /// </returns>
        Task<Book> GetBookByProductIdAsync(int productId);

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
        Task<IPagedList<Book>> GetAllBooksAsync(string name = "", string email = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

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

        /// <summary>
        /// Gets a Book note
        /// </summary>
        /// <param name="BookNoteId">The Book note identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Book note
        /// </returns>
        Task<BookNote> GetBookNoteByIdAsync(int BookNoteId);

        /// <summary>
        /// Gets all Book notes
        /// </summary>
        /// <param name="BookId">Book identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Book notes
        /// </returns>
        Task<IPagedList<BookNote>> GetBookNotesByBookAsync(int BookId, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Deletes a Book note
        /// </summary>
        /// <param name="BookNote">The Book note</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteBookNoteAsync(BookNote BookNote);

        /// <summary>
        /// Inserts a Book note
        /// </summary>
        /// <param name="BookNote">Book note</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertBookNoteAsync(BookNote BookNote);

        /// <summary>
        /// Formats the Book note text
        /// </summary>
        /// <param name="BookNote">Book note</param>
        /// <returns>Formatted text</returns>
        string FormatBookNoteText(BookNote BookNote);
    }
}