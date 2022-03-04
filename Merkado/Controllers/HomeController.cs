using Merkado.DAL;
using Merkado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Merkado.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MerkadoDbContext _db;

        public HomeController(ILogger<HomeController> logger, MerkadoDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            //Category newCategory = new Category { Name = "Telefony" };
            //_db.Categories.Add(newCategory);

            //ProductImage image1 = new ProductImage() { ImageURL = "zdjecie1" };
            //ProductImage image2 = new ProductImage() { ImageURL = "zdjecie2" };
            //ProductImage image3 = new ProductImage() { ImageURL = "zdjecie3" };

            //var images = new List<ProductImage>() { image1, image2, image3 };

            //Product newProduct = new Product 
            //{ 
            //    Category = newCategory, 
            //    AddedDate = DateTime.Now, 
            //    CoverURL = "www.okladka.pl", 
            //    Description = "Opis", 
            //    Name = "Nokia 3333", 
            //    Price = 1233, 
            //    Images = images
            //};
            //_db.Products.Add(newProduct);


            //User newSeller = new User
            //{
            //    City = "Polska",
            //    Email = "user@wp.pl",
            //    FirstName = "Andrzej",
            //    LastName = "Piaseczny",
            //    Street = "Akacjowa",
            //    PostalCode = "44-333",
            //    UserProducts = new List<Product>() { newProduct }
            //};


            //User newBuyer = new User
            //{
            //    City = "Polska",
            //    Email = "buyer@wp.pl",
            //    FirstName = "Krzysztof",
            //    LastName = "Graczyk",
            //    Street = "Miodowa",
            //    PostalCode = "99-333"
            //};

            //Order newOrder = new Order() 
            //{ 
            //    DateCreated = DateTime.Now, 
            //    OrderState = OrderState.New, 
            //    PaymentState = PaymentState.Unpaid,
            //    Product = newProduct 
            //};

            //newBuyer.UserOrders = new List<Order> { newOrder };


            //_db.Users.Add(newSeller);
            //_db.Users.Add(newBuyer);
            //_db.SaveChanges();

            //var UsersList = _db.Users
            //    .Include(o => o.UserOrders)
            //    .Include(p => p.UserProducts)
            //        .ThenInclude(i => i.Images)
            //    .ToList();


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}