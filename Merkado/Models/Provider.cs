using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merkado.Models
{
#nullable disable
    public class Provider
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
