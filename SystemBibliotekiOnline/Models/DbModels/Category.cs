using System.ComponentModel.DataAnnotations;

namespace SystemBibliotekiOnline.Models.DbModels
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public virtual List<Book> Books { get; set; } = new();
    }
}