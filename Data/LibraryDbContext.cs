
using Smart_Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Smart_Library_Management_System.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        
         public DbSet<Book> Books { get; set; }
    }
}