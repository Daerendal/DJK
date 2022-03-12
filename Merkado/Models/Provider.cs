namespace Merkado.Models
{
    public class Provider
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
