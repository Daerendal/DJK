using Merkado.Models;

namespace Merkado.ViewModels
{
#nullable disable
    public class ProductPageVM
    {
        public Product CurrentProduct { get; set; }
        public User Seller { get; set; }

        public List<Product> ProductsList { get; set; }


    }
}
