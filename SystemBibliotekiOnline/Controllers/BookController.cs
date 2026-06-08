using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SystemBibliotekiOnline.Models;
using SystemBibliotekiOnline.Models.DbModels;
using SystemBibliotekiOnline.Models.ViewModels;

namespace SystemBibliotekiOnline.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _environment;

        public BooksController(DatabaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var books = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Category)
                .ToList();

            return View(books);
        }

        [AllowAnonymous]
        public IActionResult Catalog(string? search, int? categoryId)
        {
            var books = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                books = books.Where(x => x.Title.Contains(search));

            if (categoryId.HasValue)
                books = books.Where(x => x.CategoryId == categoryId);

            ViewBag.Categories = _context.Categories.ToList();

            return View(books.ToList());
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Category)
                .FirstOrDefault(x => x.BookId == id);

            if (book == null)
                return NotFound();

            return View(book);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new BookFormViewModel
            {
                Book = new Book(),
                Authors = _context.Authors.Select(a => new SelectListItem
                {
                    Value = a.AuthorId.ToString(),
                    Text = a.Name + " " + a.Surname
                }).ToList(),

                Categories = _context.Categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList()
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookFormViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Book.ISBN))
            {
                bool exists = _context.Books.Any(x => x.ISBN == model.Book.ISBN);

                if (exists)
                {
                    ModelState.AddModelError("Book.ISBN", "Książka z takim ISBN już istnieje.");
                }
            }

            if (!ModelState.IsValid)
            {
                model.Authors = _context.Authors.Select(a => new SelectListItem
                {
                    Value = a.AuthorId.ToString(),
                    Text = a.Name + " " + a.Surname
                }).ToList();

                model.Categories = _context.Categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList();

                return View(model);
            }

            if (model.CoverFile != null)
            {
                string folder = Path.Combine(_environment.WebRootPath, "uploads", "books");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(model.CoverFile.FileName);
                string path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    model.CoverFile.CopyTo(stream);
                }

                model.Book.CoverImagePath = fileName;
            }

            _context.Books.Add(model.Book);
            _context.SaveChanges();

            TempData["Success"] = "Książka została dodana pomyślnie.";

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _context.Books.FirstOrDefault(x => x.BookId == id);
            if (book == null) return NotFound();

            return View(new BookFormViewModel
            {
                Book = book,
                Authors = _context.Authors.Select(a => new SelectListItem
                {
                    Value = a.AuthorId.ToString(),
                    Text = a.Name + " " + a.Surname
                }).ToList(),

                Categories = _context.Categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList()
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookFormViewModel model)
        {
            var book = _context.Books.FirstOrDefault(x => x.BookId == model.Book.BookId);
            if (book == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(model.Book.ISBN))
            {
                bool exists = _context.Books.Any(x =>
                    x.ISBN == model.Book.ISBN &&
                    x.BookId != model.Book.BookId);

                if (exists)
                {
                    ModelState.AddModelError("Book.ISBN", "Książka z takim ISBN już istnieje.");
                }
            }

            if (!ModelState.IsValid)
            {
                model.Authors = _context.Authors.Select(a => new SelectListItem
                {
                    Value = a.AuthorId.ToString(),
                    Text = a.Name + " " + a.Surname
                }).ToList();

                model.Categories = _context.Categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList();

                return View(model);
            }

            book.Title = model.Book.Title;
            book.ISBN = model.Book.ISBN;
            book.Description = model.Book.Description;
            book.PublicationYear = model.Book.PublicationYear;
            book.AuthorId = model.Book.AuthorId;
            book.CategoryId = model.Book.CategoryId;

            if (model.RemoveCover && !string.IsNullOrEmpty(book.CoverImagePath))
            {
                string old = Path.Combine(_environment.WebRootPath, "uploads", "books", book.CoverImagePath);

                if (System.IO.File.Exists(old))
                    System.IO.File.Delete(old);

                book.CoverImagePath = null;
            }

            if (model.CoverFile != null)
            {
                string folder = Path.Combine(_environment.WebRootPath, "uploads", "books");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(model.CoverFile.FileName);
                string path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    model.CoverFile.CopyTo(stream);
                }

                book.CoverImagePath = fileName;
            }

            _context.SaveChanges();

            TempData["Success"] = "Książka została zaktualizowana.";

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var book = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Category)
                .FirstOrDefault(x => x.BookId == id);

            if (book == null) return NotFound();

            return View(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            _context.SaveChanges();

            TempData["Success"] = "Książka została usunięta.";

            return RedirectToAction(nameof(Index));
        }
    }
}