using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemBibliotekiOnline.Models;

namespace SystemBibliotekiOnline.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminController(
            DatabaseContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

            var uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "books");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
        }

        public IActionResult Dashboard()
        {
            ViewBag.Books = _context.Books.Count();
            ViewBag.Authors = _context.Authors.Count();
            ViewBag.Categories = _context.Categories.Count();
            ViewBag.ActiveLoans = _context.Loans.Count(x => !x.IsReturned);

            ViewBag.OverdueLoans = _context.Loans.Count(x =>
                !x.IsReturned &&
                DateTime.Today > x.DueDate.Date);

            return View();
        }

        public IActionResult AllLoans()
        {
            var loans = _context.Loans
                .Include(l => l.User)
                .Include(l => l.Book)
                .ToList();

            return View(loans);
        }
    }
}