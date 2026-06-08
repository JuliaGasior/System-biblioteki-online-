using System.ComponentModel.DataAnnotations;

namespace SystemBibliotekiOnline.Models.DbModels
{
    public class Loan
    {
        public int LoanId { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; }

        public bool IsOverdue =>
            !IsReturned && DateTime.Today > DueDate;
    }
}