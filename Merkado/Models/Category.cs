using System.ComponentModel.DataAnnotations;

namespace DJK.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [StringLength(64)]
        [Required]
        public string Name { get; set; }
    }
}
