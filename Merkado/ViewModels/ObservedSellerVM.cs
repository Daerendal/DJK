using DJK.Models;

namespace DJK.ViewModels
{
    public class ObservedSellerVM
    {
        public User Seller { get; set; }
        public User user { get; set; }
        public List<FavouriteSeller> FavouriteSeller { get; set; }
        public List<User> ObservedSeller { get; set; }
    }
}
