using System.ComponentModel.DataAnnotations;

namespace SystemBibliotekiOnline.Models.DbModels
{
    public class Author
    {
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [StringLength(50, ErrorMessage = "Imię nie może przekraczać 50 znaków.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [StringLength(50, ErrorMessage = "Nazwisko nie może przekraczać 50 znaków.")]
        public string Surname { get; set; }

        public virtual List<Book> Books { get; set; } = new();
    }
}