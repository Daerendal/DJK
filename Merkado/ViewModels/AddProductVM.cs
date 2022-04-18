using Merkado.Models;

namespace Merkado.ViewModels
{
    public class AddProductVM
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public Product Product { get; set; } = new Product();
        public List<Product> Products { get; set; } = new List<Product>();
        public Category Category { get; set; } = new Category();
        public List<Provider> Providers { get; set; } = new List<Provider>();
        public User Seller { get; set; } = new User();
    }
}
