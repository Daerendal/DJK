using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Merkado.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        public List<Product>? UserProducts { get; set; }
        public List<Order>? UserOrders { get; set; }
    }
}
