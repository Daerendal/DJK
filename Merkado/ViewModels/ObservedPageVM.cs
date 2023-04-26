using DJK.Models;

namespace DJK.ViewModels
{
#nullable disable
    public class ObservedPageVM
    {
        public List <Product> OProducts { get; set; }
        public User Seller { get; set; }

        public List <ObservedProduct> ObservedProduct { get; set; }
    }
}
