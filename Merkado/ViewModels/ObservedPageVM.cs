using Merkado.Models;

namespace Merkado.ViewModels
{
#nullable disable
    public class ObservedPageVM
    {
        public List <Product> OProducts { get; set; }
        public User Seller { get; set; }

        public List <ObservedProduct> ObservedProduct { get; set; }
    }
}
