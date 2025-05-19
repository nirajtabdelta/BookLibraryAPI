using DataAccess.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Data
{
    public class BookLibraryContext : DbContext
    {
        public BookLibraryContext(DbContextOptions<BookLibraryContext> options) : base(options)
        {
            
        }
        public DbSet<Book> Books { get; set; }
    }
}
