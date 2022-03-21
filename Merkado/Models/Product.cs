using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Merkado.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [StringLength(64)]
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Required]
        public string CoverURL { get; set; }
        [Required]
        [Precision(18, 2)]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        [Display(Name = "Kategoria")]
        public Category Category { get; set; }
        [Required]
        public List<Provider> Providers { get; set; }
        [Display(Name = "Obraz")]
        public List<ProductImage>? Images { get; set; }
    }
}
