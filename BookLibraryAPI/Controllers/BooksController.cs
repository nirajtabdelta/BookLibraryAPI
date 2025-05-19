using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.Repositories;

namespace BookLibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        #region Private Variable
        private readonly IBookRepository _bookRepository;

        #endregion

        #region Constructor
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        #endregion

        #region Public Methord

        /// <summary>
        /// Get All Books Data 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var books = await _bookRepository.GetAllAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                // Log exception if logging is available
                return StatusCode(500, "An error occurred while retrieving books.");
            }
        }

        /// <summary>
        /// Get Book by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null)
                    return NotFound();

                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the book.");
            }
        }

        /// <summary>
        /// Search books by author or genre
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? author, [FromQuery] string? genre)
        {
            try
            {
                var result = await _bookRepository.SearchAsync(author, genre);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while searching for books.");
            }
        }

        /// <summary>
        /// Add a new book
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Add(Book book)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _bookRepository.AddAsync(book);
                return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the book.");
            }
        }

        /// <summary>
        /// Update an existing book
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Book book)
        {
            try
            {
                if (id != book.Id)
                    return BadRequest("Book ID mismatch.");

                await _bookRepository.UpdateAsync(book);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the book.");
            }
        }

        /// <summary>
        /// Soft-delete a book
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the book.");
            }
        }
        
        #endregion
    }
}
