using DataAccess.Data;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace Repository.Services
{
    public class BookService :  IBookRepository
    {
        #region Private Variable
        private readonly BookLibraryContext _context;
        #endregion

        #region Constructor
        public BookService(BookLibraryContext context)
        {
            _context = context;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Get All Book List
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            try
            {
                return await _context.Books.Where(b => !b.IsDeleted).ToListAsync();
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        /// <summary>
        /// GET Book Data Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Book?> GetByIdAsync(int id)
        {
            try
            {
                return  await _context.Books.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Book Search
        /// </summary>
        /// <param name="author"></param>
        /// <param name="genre"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Book>> SearchAsync(string? author, string? genre)
        {
            try
            {
                var query = _context.Books.AsQueryable();

                if (!string.IsNullOrEmpty(author))
                    query = query.Where(b => b.Author.Contains(author));

                if (!string.IsNullOrEmpty(genre))
                    query = query.Where(b => b.Genre.Contains(genre));

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Book Add New Record
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task AddAsync(Book book)
        {
            try
            {
                book.Id = 0;
                var validBookName = await bookNameValid(book.Title);
                if (validBookName)
                {
                    _context.Books.Add(book);
                    await _context.SaveChangesAsync();
                }
                else 
                {
                    throw new InvalidOperationException("A book with this title already exists.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Book Name Valid or not
        /// </summary>
        /// <param name="bookName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> bookNameValid(string bookName,int id = 0)
        {
            try
            {
                var normalizedTitle = bookName.Trim().ToLower();

                if (id == 0)
                {
                    //Add for check
                    bool exists = await _context.Books
                        .AnyAsync(x => x.Title.Trim().ToLower() == normalizedTitle && !x.IsDeleted);

                    if (exists)
                    {
                        return false;
                    }

                }
                else
                {
                    //Update for check
                    bool exists = await _context.Books
                        .AnyAsync(x => x.Title.Trim().ToLower() == normalizedTitle && x.Id != id && !x.IsDeleted);

                    if (exists)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Update Book Record 
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task UpdateAsync(Book book)
        {
            try
            {
                var validBookName = await bookNameValid(book.Title, book.Id);
                if (validBookName)
                {
                    _context.Entry(book).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException("A book with this title already exists.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete Book Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book != null)
                {
                    // It's Hard Delete Code
                    //_context.Books.Remove(book);
                    
                    //Soft Delete Book 
                    book.IsDeleted = true;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
