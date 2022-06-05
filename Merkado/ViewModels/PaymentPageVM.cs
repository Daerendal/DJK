using Merkado.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Merkado.ViewModels
{
#nullable disable
    public class PaymentPageVM
    {
        public Product CurrentProduct { get; set; }
        public User Seller { get; set; }

        public User Buyer { get; set; }

        public  Provider provider{ get; set; } 
    }
}
