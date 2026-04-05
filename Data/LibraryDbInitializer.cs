using Microsoft.EntityFrameworkCore;
using Smart_Library_Management_System.Models;
using System.Linq;

namespace Smart_Library_Management_System.Data
{
    public static class DbInitializer
    {
        public static void Initialize(LibraryDbContext context)
        {
            context.Database.Migrate();

            // Seed Categories
            var categories = new List<Category>
            {
                new Category { Name = "Laptops" },
                new Category { Name = "Accessories" },
                new Category { Name = "Books" },
                new Category { Name = "Science Fiction" }
            };

            foreach (var cat in categories)
            {
                if (!context.Category.Any(c => c.Name == cat.Name))
                    context.Category.Add(cat);
            }
            context.SaveChanges();

            // Seed Books
            if (!context.Books.Any())
            {
                var laptops = context.Category.First(c => c.Name == "Laptops").CategoryID;
                var accessories = context.Category.First(c => c.Name == "Accessories").CategoryID;
                var booksCat = context.Category.First(c => c.Name == "Books").CategoryID;
                var scifi = context.Category.First(c => c.Name == "Science Fiction").CategoryID;

                var books = new List<Book>
                {
                    new Book { Title = "MacBook Pro M3", Author = "Apple", Price = 20000, StockQuantity = 10, CategoryId = laptops },
                    new Book { Title = "Dell XPS 15", Author = "Dell", Price = 1499.99, StockQuantity = 8, CategoryId = laptops },
                    new Book { Title = "HP Spectre x360", Author = "HP", Price = 1299.99, StockQuantity = 12, CategoryId = laptops },
                    new Book { Title = "Wireless Mouse", Author = "Logitech", Price = 29.99, StockQuantity = 50, CategoryId = accessories },
                    new Book { Title = "USB-C Hub", Author = "Anker", Price = 49.99, StockQuantity = 30, CategoryId = accessories },
                    new Book { Title = "Mechanical Keyboard", Author = "Keychron", Price = 89.9, StockQuantity = 25, CategoryId = accessories },
                    new Book { Title = "Clean Code", Author = "Robert C. Martin", Price = 42.99, StockQuantity = 20, CategoryId = booksCat },
                    new Book { Title = "Design Patterns", Author = "Gang of Four", Price = 54.99, StockQuantity = 15, CategoryId = booksCat },
                    new Book { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Price = 49.99, StockQuantity = 18, CategoryId = booksCat },
                    new Book { Title = "Coffee Mug", Author = "Generic", Price = 12.99, StockQuantity = 100, CategoryId = scifi },
                    new Book { Title = "Desk Lamp", Author = "Philips", Price = 35.99, StockQuantity = 40, CategoryId = scifi },
                    new Book { Title = "Notebook", Author = "Moleskine", Price = 19.99, StockQuantity = 60, CategoryId = scifi }
                };

                context.Books.AddRange(books);
                context.SaveChanges();
            }
        }
    }
}