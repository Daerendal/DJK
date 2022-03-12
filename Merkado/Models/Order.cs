using System.ComponentModel.DataAnnotations;

namespace Merkado.Models
{
    public enum OrderState
    {
        [Display(Name = "Nowe")]
        New,

        [Display(Name = "Wysłane")]
        Shipped,

        [Display(Name = "Zrealizowane")]
        Realized
    }

    public enum PaymentState
    {
        [Display(Name = "Zapłacono")]
        Paid,

        [Display(Name = "Nie zapłacono")]
        Unpaid
    }

    public class Order
    {
        public int OrderId { get; set; }
        public DateTime DateCreated { get; set; }
        public OrderState OrderState { get; set; }
        public PaymentState PaymentState { get; set; }
        public Provider Provider { get; set; }
        public Product Product { get; set; }
    }
}
