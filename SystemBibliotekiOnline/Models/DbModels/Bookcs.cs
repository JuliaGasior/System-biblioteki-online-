using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SystemBibliotekiOnline.Models.DbModels
{
    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        [StringLength(200, ErrorMessage = "Tytuł nie może przekraczać 200 znaków.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "ISBN jest wymagany.")]
        [StringLength(20, MinimumLength = 10,
            ErrorMessage = "ISBN musi mieć od 10 do 20 znaków.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "ISBN może zawierać tylko cyfry.")]
        public string ISBN { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Rok wydania jest wymagany.")]
        [Range(1000, 2026,
            ErrorMessage = "Rok wydania musi być z zakresu 1000–2026.")]
        public int? PublicationYear { get; set; }

        [Required(ErrorMessage = "Autor jest wymagany.")]
        public int? AuthorId { get; set; }

        [Required(ErrorMessage = "Kategoria jest wymagana.")]
        public int? CategoryId { get; set; }

        public string? CoverImagePath { get; set; }

        public bool IsAvailable { get; set; } = true;

        [ValidateNever]
        public Author? Author { get; set; }

        [ValidateNever]
        public Category? Category { get; set; }
    }
}