using Microsoft.AspNetCore.Mvc;
using Smart_Library_Management_System.Models;
using Smart_Library_Management_System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Smart_Library_Management_System.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Books
        // Affiche la liste de tous les livres// GET: Books/Index
        public IActionResult Index(string searchTitle)
        {
            var books = _context.Books
                                .Include(b => b.Category)
                                .AsQueryable();

            // Filtre uniquement si un terme est saisi
            if (!string.IsNullOrEmpty(searchTitle))
            {
                books = books.Where(b => b.Title.Contains(searchTitle));
                ViewBag.SearchTitle = searchTitle;
            }

            return View(books.ToList());
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .AsNoTracking()
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BookID == id);

            if (book == null) return NotFound();

            return View(book);
        }
        // GET: /Books/Create
        // Affiche le formulaire de création
        public IActionResult Create()
        {
            // ✅ Charger les catégories pour le dropdown
            ViewBag.Categories = new SelectList(
                _context.Category.AsNoTracking(),
                "CategoryID",
                "Name"
            );
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title, Author, Price, StockQuantity, CategoryId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //  Recharger les catégories si erreur
            ViewBag.Categories = new SelectList(
                _context.Category.AsNoTracking(),
                "CategoryID",
                "Name",
                book.CategoryId //catégorie actuelle présélectionnée
            );
            return View(book);
        }
        // GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // ✅ Chercher dans Books
            var book = await _context.Books.FindAsync(id);

            if (book == null) return NotFound();

            // Charger les catégories pour le dropdown
            ViewBag.Categories = new SelectList(
             _context.Category.AsNoTracking(),
             "CategoryID",
             "Name",
             book.CategoryId  //  sélectionne la catégorie actuelle du livre
         );
            return View(book);
        }

        // POST: /Books/Edit/5
        // Reçoit les modifications et met à jour le livre
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("BookID, Title, Author, Price, StockQuantity, CategoryId")] Book book)
        {
            if (id != book.BookID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID)) return NotFound();
                    else throw;
                }
            }

            ViewBag.Categories = new SelectList(
                _context.Category.AsNoTracking(),
                "CategoryID",
                "Name",
                book.CategoryId
            );

            return View(book);
        }

        //GET/Delete/5
        // Affiche la page de confirmation de suppression
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        //Post
        //Supprime définitivement le livre après confirmation

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }

    }
}