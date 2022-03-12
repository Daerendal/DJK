namespace Merkado.Models
{
    public class ObservedProduct
    {
        public int ObservedProductId { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
