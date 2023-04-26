using DJK.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DJK.ViewModels
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
