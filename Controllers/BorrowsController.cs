using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Library_Management_System.Models;
using Smart_Library_Management_System.Data;

namespace Smart_Library_Management_System.Controllers
{
    public class BorrowsController : Controller
    {
        private readonly LibraryDbContext _context;

        public BorrowsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Borrows
        public async Task<IActionResult> Index()
        {
            var borrows = await _context.Borrows
                .Include(b => b.BorrowItems)
                .ThenInclude(bi => bi.Book)
                .AsNoTracking()
                .ToListAsync();

            return View(borrows);
        }

        // GET: Borrows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var borrow = await _context.Borrows
                .Include(b => b.BorrowItems)
                .ThenInclude(bi => bi.Book)
                .ThenInclude(b => b!.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.BorrowID == id);

            if (borrow == null) return NotFound();

            return View(borrow);
        }

        // GET: Borrows/Create

        [HttpGet]
        public IActionResult Create()
        {
            // Charger uniquement les livres disponibles (stock > 0)
            // avec leur catégorie pour l'affichage dans la vue
            ViewBag.Books = _context.Books
                .AsNoTracking()
                .Include(b => b.Category)
                .Where(b => b.StockQuantity > 0)
                .ToList();

            // Initialiser la date d'emprunt à aujourd'hui
            return View(new Borrow { BorrowDate = DateTime.Now });
        }


        // POST: Borrows/Create
        // Gère la création d'emprunt avec plusieurs livres et transaction
        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Create(Borrow borrow, List<int> bookIds, List<int> quantities)
        {
           
            ModelState.Remove("BorrowItems");

            if (!ModelState.IsValid)
            {
                ReloadBooks();
                return View(borrow);
            }

            // Vérifier qu'au moins un livre est sélectionné
            if (bookIds == null || bookIds.Count == 0)
            {
                ModelState.AddModelError("", "Veuillez sélectionner au moins un livre.");
                ReloadBooks();
                return View(borrow);
            }

            // Vérifier que bookIds et quantities ont la même taille
            if (quantities == null || bookIds.Count != quantities.Count)
            {
                ModelState.AddModelError("", "Données invalides. Veuillez réessayer.");
                ReloadBooks();
                return View(borrow);
            }

            // Transaction : valider stock + mettre à jour + sauvegarder
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Borrows.Add(borrow);

                for (int i = 0; i < bookIds.Count; i++)
                {
                    var book = await _context.Books.FirstOrDefaultAsync(b => b.BookID == bookIds[i]);

                    //  Validate stock
                    if (book.StockQuantity < quantities[i])
                        throw new Exception($"Stock insuffisant pour '{book.Title}'.");

                    // Update stock
                    book.StockQuantity -= quantities[i];

                    _context.BorrowItems.Add(new BorrowItem
                    {
                        Borrow = borrow,
                        BookID = bookIds[i],
                        Quantity = quantities[i]
                    });
                }

                //  Save all changes
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                TempData["Success"] = "Emprunt créé avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //Rollback in case of error
                await transaction.RollbackAsync();
                ModelState.AddModelError("", ex.Message);
                ReloadBooks();
                return View(borrow);
            }
        }
        // Recharger les livres disponibles
        private void ReloadBooks()
        {
            ViewBag.Books = _context.Books
                .AsNoTracking()
                .Include(b => b.Category)
                .Where(b => b.StockQuantity > 0)
                .ToList();
        }
    
        // GET: Borrows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var borrow = await _context.Borrows
                .Include(b => b.BorrowItems)
                .ThenInclude(bi => bi.Book)
                .FirstOrDefaultAsync(b => b.BorrowID == id);

            if (borrow == null) return NotFound();

            return View(borrow);
        }

        // POST: Borrows/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Borrow borrow)
        {
            if (id != borrow.BorrowID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowExists(borrow.BorrowID)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(borrow);
        }

        // GET: Borrows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var borrow = await _context.Borrows
                .Include(b => b.BorrowItems)
                .ThenInclude(bi => bi.Book)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.BorrowID == id);

            if (borrow == null) return NotFound();

            return View(borrow);
        }


        // POST: Borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var borrow = await _context.Borrows
                    .Include(b => b.BorrowItems)
                    .FirstOrDefaultAsync(b => b.BorrowID == id);

                if (borrow == null) return NotFound();

                foreach (var item in borrow.BorrowItems)
                {
                    var book = await _context.Books.FindAsync(item.BookID);
                    if (book != null)
                        book.StockQuantity += item.Quantity;
                }

                _context.Borrows.Remove(borrow);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                await transaction.RollbackAsync();
                return BadRequest();
            }
        }
        // GET: /Borrows/Return/5
public async Task<IActionResult> Return(int? id)
{
    if (id == null) return NotFound();

    var borrow = await _context.Borrows
        .Include(b => b.BorrowItems)
        .ThenInclude(bi => bi.Book)
        .FirstOrDefaultAsync(b => b.BorrowID == id);

    if (borrow == null) return NotFound();

    // Vérifier si déjà retourné
    if (borrow.ReturnDate != null)
    {
        TempData["Error"] = "Cet emprunt a déjà été retourné.";
        return RedirectToAction(nameof(Index));
    }

    return View(borrow);
}

// POST: /Borrows/Return/5
[HttpPost, ActionName("Return")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ReturnConfirmed(int id)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        var borrow = await _context.Borrows
            .Include(b => b.BorrowItems)
            .FirstOrDefaultAsync(b => b.BorrowID == id);

        if (borrow == null) return NotFound();

        if (borrow.ReturnDate != null)
        {
            TempData["Error"] = "Cet emprunt a déjà été retourné.";
            return RedirectToAction(nameof(Index));
        }

        // 1️⃣ Remettre le stock pour chaque livre
        foreach (var item in borrow.BorrowItems)
        {
            var book = await _context.Books.FindAsync(item.BookID);
            if (book != null)
                book.StockQuantity += item.Quantity; // ✅ stock restauré
        }

        // 2️⃣ Enregistrer la date de retour
        borrow.ReturnDate = DateTime.Now;

        // 3️⃣ Sauvegarder tout
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        TempData["Success"] = "Retour effectué avec succès. Stock mis à jour.";
        return RedirectToAction(nameof(Index));
    }
    catch (Exception ex)
    {
        // 4️⃣ Rollback en cas d'erreur
        await transaction.RollbackAsync();
        TempData["Error"] = "Erreur lors du retour : " + ex.Message;
        return RedirectToAction(nameof(Index));
    }
}
        private bool BorrowExists(int id)
        {
            return _context.Borrows.Any(e => e.BorrowID == id);
        }

        private void ReloadAvailableBooks()
        {
            ViewBag.AvailableBooks = _context.Books
                .Where(b => b.StockQuantity > 0)
                .Include(b => b.Category)
                .AsNoTracking()
                .ToList();
        }
    }
}