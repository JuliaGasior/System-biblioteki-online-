using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemBibliotekiOnline.Data;
using SystemBibliotekiOnline.Models;
using SystemBibliotekiOnline.Models.DbModels;

namespace SystemBibliotekiOnline.Controllers
{
    [Authorize]
    public class LoansController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoansController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Borrow(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);

            var activeLoansCount = await _context.Loans
                .CountAsync(l => l.UserId == user.Id && !l.IsReturned);

            if (activeLoansCount >= 5)
            {
                TempData["Error"] = "Nie możesz wypożyczyć więcej niż 5 książek.";
                return RedirectToAction("Catalog", "Books");
            }

            var book = await _context.Books.FindAsync(bookId);

            if (book == null || !book.IsAvailable)
                return RedirectToAction("Catalog", "Books");

            var loan = new Loan
            {
                BookId = bookId,
                UserId = user.Id,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                IsReturned = false
            };

            book.IsAvailable = false;

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Książka została wypożyczona.";

            return RedirectToAction("MyLoans");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.LoanId == loanId);

            if (loan == null || loan.IsReturned)
            {
                TempData["Error"] = "Nie znaleziono wypożyczenia.";
                return RedirectToAction("MyLoans");
            }

            loan.IsReturned = true;
            loan.ReturnDate = DateTime.Now;

            if (loan.Book != null)
            {
                loan.Book.IsAvailable = true;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Książka została zwrócona.";

            return RedirectToAction("MyLoans");
        }

        public async Task<IActionResult> MyLoans()
        {
            var user = await _userManager.GetUserAsync(User);

            var loans = await _context.Loans
                .Include(l => l.Book)
                .Where(l => l.UserId == user.Id)
                .ToListAsync();

            return View(loans);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllLoans()
        {
            var loans = await _context.Loans
                .Include(l => l.User)
                .Include(l => l.Book)
                .ToListAsync();

            return View(loans);
        }
    }
}