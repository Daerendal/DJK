using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Merkado.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [StringLength(64)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string CoverURL { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public List<Provider> Providers { get; set; }
        public List<ProductImage>? Images { get; set; }

    }
}
