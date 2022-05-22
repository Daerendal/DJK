using Merkado.DAL;
using Merkado.Models;
using Merkado.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Merkado.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly MerkadoDbContext _db;
        private UserManager<User> _userManager;

        public AdminPanelController(MerkadoDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var loggedUser = _userManager.GetUserAsync(User);

            if (loggedUser != null && User.IsInRole("Admin"))
            {
                var loggedUserID = loggedUser.Result.Id;
                var model = new AdminPanelVM();
                model.Users = _db.Users.Where(i => i.Id != loggedUserID).ToList();
                model.Products = _db.Products.Include(c => c.Category).ToList();
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult EditUser(string id)
        {

            var user = _db.Users.Include(o => o.Opinions)
                                .Include(p => p.UserProducts)
                            .FirstOrDefault(u => u.Id == id);

            if (user != null && User.IsInRole("Admin"))
            {
                return View(user);
            }
            else
            {
                return NotFound();
            }

        }

        public IActionResult EditUserCommand(User editedUser)
        {
            var user = _db.Users.Find(editedUser.Id);
            if (user != null && User.IsInRole("Admin"))
            {
                user.FirstName = editedUser.FirstName;
                user.LastName = editedUser.LastName;
                user.Email = editedUser.Email;
                user.UserName = editedUser.UserName;
                user.City = editedUser.City;
                user.Street = editedUser.Street;
                user.PostalCode = editedUser.PostalCode;

                _db.SaveChanges(); 
                return Redirect("/AdminPanel/Index");
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult DeleteUser(string userID)
        {
            var user = _db.Users.Include(o => o.Opinions)
                                .Include(p => p.UserProducts)
                                    .ThenInclude(i => i.Images)
                                .Include(ob => ob.ObservedProducts)
                                .Include(f => f.FavouriteSellers)
                                .FirstOrDefault(u => u.Id == userID);

            if (user != null && User.IsInRole("Admin"))
            {
                _db.ObservedProducts.RemoveRange(user.ObservedProducts);
                _db.Opinions.RemoveRange(user.Opinions);
                _db.FavouriteSeller.RemoveRange(user.FavouriteSellers);
                
                if (user.UserProducts != null)
                {
                    var productImages = user.UserProducts.SelectMany(i => i.Images);
                    _db.ProductImage.RemoveRange(productImages);
                    _db.Products.RemoveRange(user.UserProducts);
                }

                _db.Users.Remove(user);
                _db.SaveChanges();
                return Redirect("/AdminPanel/Index");
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult DeleteUserOpinion(int opinionID)
        {
            var opinion = _db.Opinions.Find(opinionID);

            if (opinion != null && User.IsInRole("Admin"))
            {
                _db.Opinions.Remove(opinion);
                _db.SaveChanges();
                return Redirect("/AdminPanel/Index");
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult DeleteProduct(int productId)
        {
            var product = _db.Products.Include(i=>i.Images).FirstOrDefault(x=>x.ProductId == productId);

            if (product != null && User.IsInRole("Admin"))
            {
                var productImages = product.Images;
                _db.ProductImage.RemoveRange(productImages);
                _db.Products.Remove(product);
                _db.SaveChanges();
                return Redirect("/AdminPanel/Index");
            }
            else
            {
                return NotFound();
            }
        }


    }
}
