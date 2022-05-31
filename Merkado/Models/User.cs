using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merkado.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Pole Imię jest wymagane.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Pole Nazwisko jest wymagane.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Pole Ulica jest wymagane.")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Pole Miasto jest wymagane.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Pole Kod pocztowy jest wymagane.")]
        public string PostalCode { get; set; }
        public List<Product> UserProducts { get; set; } = new List<Product>();
        public List<Order>? UserOrders { get; set; }
        public List<Opinion> Opinions { get; set; } = new List<Opinion>();
        public List<FavouriteSeller>? FavouriteSellers { get; set; }
        public List<ObservedProduct>? ObservedProducts { get; set; }

        [NotMapped]
        public float? Score
        {
            get
            {
                float rateSum = 0;

                if (Opinions != null)
                {
                    foreach (var opinion in Opinions)
                        rateSum += opinion.Rate;

                    return rateSum / Opinions.Count();
                }
                else
                {
                    return 0;
                }
            }
        }

        
        public int OpinionCounter { get; set; }
        public string PromoCode { get; set; }
    }
}
