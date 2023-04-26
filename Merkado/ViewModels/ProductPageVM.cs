using DJK.Models;

namespace DJK.ViewModels
{
#nullable disable
    public class ProductPageVM
    {
        public Product CurrentProduct { get; set; }
        public User Seller { get; set; }

        public List<Product> SimilarProducts { get; set; } = new List<Product>();


    }
}
