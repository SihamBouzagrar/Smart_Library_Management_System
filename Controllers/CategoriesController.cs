using Microsoft.AspNetCore.Mvc;
using Smart_Library_Management_System.Models;
using Smart_Library_Management_System.Data;
using Microsoft.EntityFrameworkCore;


namespace Smart_Library_Management_System.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly LibraryDbContext _context;

        public CategoriesController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: /Categories
        public async Task<IActionResult> Index()
        {
            var categories = _context.Category
                .AsNoTracking()
                .Include(c => c.Books)
                .ToList();

            return View(categories);
          
        }
      


        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create

        [HttpPost]
        [ValidateAntiForgeryToken]

       public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            _context.Category.Add(category);
            _context.SaveChanges();

            TempData["Success"] = "Catégorie crée avec succès.";
            return RedirectToAction(nameof(Index));
        }
        // GET: Categories/Edit/id?
        public async Task<IActionResult> Edit(int? id)
        {
             if (id == null) return NotFound();

        var category = await _context.Category.FindAsync(id);
        if (category == null) return NotFound();

        return View(category);  }

        // POST: Categories/Edit/id?
        [HttpPost]
        [ValidateAntiForgeryToken]
       public IActionResult Edit(int id, Category category)
        {
            if (id != category. CategoryID) return NotFound();

            if (!ModelState.IsValid)
                return View(category);

            var existing = _context.Category.FirstOrDefault(c => c.CategoryID == id);
            if (existing == null) return NotFound();

            existing.Name = category.Name;
            _context.SaveChanges();

            TempData["Success"] = "Catégorie modifiée avec succès.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Category/Delete/id?
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

        var category = await _context.Category
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.CategoryID == id);

        if (category == null) return NotFound();

        return View(category);
        }

        // POST:Category/Delete/id?
      [HttpPost, ActionName("Delete")]
        public IActionResult Deleted(int id)
        {
            var category = _context.Category
                .Include(c => c.Books)
                .FirstOrDefault(c => c.CategoryID == id);

            if (category == null) return NotFound();

            if (category.Books.Any())
            {
                TempData["Error"] = "Impossible de supprimer : cette catégorie contient des livres.";
                return RedirectToAction(nameof(Index));
            }

            _context.Category.Remove(category);
            _context.SaveChanges();

            TempData["Success"] = "Catégorie supprimée.";
            return RedirectToAction(nameof(Index));
        }
    
        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryID == id);
        }
    }
}
