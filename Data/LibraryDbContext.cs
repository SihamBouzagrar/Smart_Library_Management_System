
using Smart_Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Smart_Library_Management_System.Data
{
    public class LibraryDbContext : DbContext
    {


        public LibraryDbContext()
        {
        }
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<Borrow> Borrows { get; set; }

        public DbSet<BorrowItem> BorrowItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowItem>()
    .HasOne(bi => bi.Book)
.WithMany(b => b.BorrowItems)
    .HasForeignKey(bi => bi.BookID)
    .OnDelete(DeleteBehavior.Restrict);

           
        }
    }
}
