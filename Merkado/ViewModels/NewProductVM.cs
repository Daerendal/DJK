using DJK.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DJK.ViewModels
{
    public class NewProductVM
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public Product Product { get; set; } = new Product();
        public List<Product> Products { get; set; } = new List<Product>();
        
        [Required(ErrorMessage = "Proszę wybrać sposób wysyłki")]
        [Display(Name = "Dostawa")]
        public List<SelectListItem> Providers { get; set; } = new List<SelectListItem>();
        public User Seller { get; set; } = new User();

        [Required(ErrorMessage = "Proszę dodać zdjęcie sprzedawanego przedmiotu")]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}
