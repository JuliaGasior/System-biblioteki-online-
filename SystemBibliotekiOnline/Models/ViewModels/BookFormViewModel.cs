using Microsoft.AspNetCore.Mvc.Rendering;
using SystemBibliotekiOnline.Models.DbModels;

namespace SystemBibliotekiOnline.Models.ViewModels
{
    public class BookFormViewModel
    {
        public Book Book { get; set; } = new();

        public IFormFile? CoverFile { get; set; }

        public bool RemoveCover { get; set; }

        public List<SelectListItem> Authors { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = new();
    }
}