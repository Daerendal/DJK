using System.ComponentModel.DataAnnotations;

namespace Merkado.Models
{
    public enum OrderState
    {
        [Display(Name = "Nowe")]
        New,

        [Display(Name = "Wysłane")]
        Shipped
    }

    public enum PaymentState
    {
        [Display(Name = "Zapłacono")]
        Paid,

        [Display(Name = "Nie zapłacono")]
        Unpaid
    }

    public enum Provider
    {
        [Display(Name = "Inpost")]
        Inpost,

        [Display(Name = "Kurier")]
        Courier,

        [Display(Name = "Poczta Polska")]
        Post,

        [Display(Name = "Odbiór osobisty")]
        Self
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
