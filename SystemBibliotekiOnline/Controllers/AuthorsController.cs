using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemBibliotekiOnline.Models;
using SystemBibliotekiOnline.Models.DbModels;

namespace SystemBibliotekiOnline.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorsController : Controller
    {
        private readonly DatabaseContext _context;

        public AuthorsController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var authors = _context.Authors.ToList();
            return View(authors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author author)
        {
            if (!ModelState.IsValid)
                return View(author);

            author.Name = author.Name?.Trim();
            author.Surname = author.Surname?.Trim();

            var exists = _context.Authors.Any(a =>
                a.Name.ToLower() == author.Name.ToLower() &&
                a.Surname.ToLower() == author.Surname.ToLower());

            if (exists)
            {
                ModelState.AddModelError("", "Taki autor już istnieje.");
                return View(author);
            }

            _context.Authors.Add(author);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = _context.Authors.Find(id);

            if (author == null)
                return NotFound();

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Author author)
        {
            if (!ModelState.IsValid)
                return View(author);

            author.Name = author.Name?.Trim();
            author.Surname = author.Surname?.Trim();

            _context.Authors.Update(author);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var author = _context.Authors.Find(id);

            if (author == null)
                return NotFound();

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var author = _context.Authors.Find(id);

            if (author == null)
                return NotFound();

            bool hasBooks = _context.Books.Any(x => x.AuthorId == id);

            if (hasBooks)
            {
                ModelState.AddModelError("", "Nie można usunąć autora, ponieważ są przypisane do niego książki.");
                return View("Delete", author);
            }

            _context.Authors.Remove(author);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}