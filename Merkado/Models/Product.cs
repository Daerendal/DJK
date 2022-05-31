using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Merkado.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string BuyerId { get; set; } = string.Empty;

        [StringLength(32, ErrorMessage = "{0} musi składać się minimum z {2} znaków.", MinimumLength = 6)]
        [Required(ErrorMessage = "Proszę podać nazwę produktu")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; } = string.Empty;

        [StringLength(64, ErrorMessage = "{0} musi składać się minimum z {2} znaków.", MinimumLength = 3)]
        [Required(ErrorMessage = "Proszę wskazać lokalizację")]
        [Display(Name = "Lokalizacja produktu")]
        public string Localization { get; set; } = string.Empty;

        [StringLength(900, ErrorMessage = "{0} musi składać się minimum z {2} znaków.", MinimumLength = 30)]
        [Required(ErrorMessage = "Proszę uzupełnić opis produktu")]
        [Display(Name = "Opis")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string CoverURL { get; set; } = string.Empty;

        [Required(ErrorMessage = "Proszę wprowadzić cenę")]
        [RegularExpression(@"^(\d{1,9}|\d{0,5}\.\d{1,2})$", ErrorMessage = "Niepoprawna cena, użyj kropki jako separator")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; } = 1.00m;

        [Required]
        [Display(Name = "Data dodania")]
        public DateTime AddedDate { get; set; }

        public bool IsSold { get; set; }

        [Required(ErrorMessage = "Proszę wybrać kategorię")]
        [Display(Name = "Kategoria")]
        public Category Category { get; set; } = new Category();

        [Required(ErrorMessage = "Proszę wybrać sposób wysyłki")]
        [Display(Name = "Dostawa")]
        public List<Provider> Providers { get; set; } = new List<Provider>();

        [Required(ErrorMessage = "Proszę dodać zdjęcie sprzedawanego przedmiotu")]
        [Display(Name = "Zdjęcia")]
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();

    }
}
