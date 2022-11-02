using Nop.Core;
using Nop.Core.Domain.Books;
using Nop.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Services.Books
{
    /// <summary>
    /// Book service
    /// </summary>
    public partial class BookService : IBookService
    {
        #region Fields

        private readonly IRepository<Book> _BookRepository;

        #endregion

        #region Ctor

        public BookService( 
            IRepository<Book> BookRepository )
        {             
            _BookRepository = BookRepository;             
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a Book by Book identifier
        /// </summary>
        /// <param name="BookId">Book identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Book
        /// </returns>
        public virtual async Task<Book> GetBookByIdAsync(int BookId)
        {
            return await _BookRepository.GetByIdAsync(BookId, cache => default);
        }

        /// <summary>
        /// Delete a Book
        /// </summary>
        /// <param name="Book">Book</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteBookAsync(Book Book)
        {
            await _BookRepository.DeleteAsync(Book);
        }

        public virtual async Task<IPagedList<Book>> GetAllBooksAsync(string name = "", string author = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var Books = await _BookRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(v => v.Name.Contains(name));

                if (!string.IsNullOrWhiteSpace(author))
                    query = query.Where(v => v.Author.Contains(author));
                query = query.OrderBy(v => v.Name).ThenBy(v => v.Author);

                return query;
            }, pageIndex, pageSize);

            return Books;
        }

        public virtual async Task<List<Book>> GetAllBookList(Book book)
        {
            //var books = _BookRepository.GetAllAsync(book);

            var books = await _BookRepository.GetAllAsync(query =>
            {                  
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id);

                return query;
            });

            return (List<Book>)books;
        }

        /// <summary>
        /// Inserts a Book
        /// </summary>
        /// <param name="Book">Book</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertBookAsync(Book Book)
        {
            await _BookRepository.InsertAsync(Book);
        }

        /// <summary>
        /// Updates the Book
        /// </summary>
        /// <param name="Book">Book</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateBookAsync(Book Book)
        {
            await _BookRepository.UpdateAsync(Book);
        }

    

        #endregion
    }
}