using Merkado.DAL;
using Merkado.Models;
using Merkado.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Merkado.Controllers
{
    public class NewProductController : Controller
    {
        private readonly ILogger<NewProductController> _logger;
        private readonly MerkadoDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment webHostEnvironment;




        public NewProductController(ILogger<NewProductController> logger, 
                                    MerkadoDbContext db, 
                                    UserManager<User> userManager, 
                                    IHttpContextAccessor httpContextAccessor,
                                    IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var selectedProviders = _db.Providers.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.ProviderId.ToString()
            }).ToList();

            NewProductVM addProductVM = new NewProductVM();
            addProductVM.Products = _db.Products.ToList();
            addProductVM.Categories = _db.Categories.ToList();
            addProductVM.Providers = selectedProviders;
            addProductVM.Seller = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result;
            return View(addProductVM);
        }

        public async Task<IActionResult> AddProduct(NewProductVM model)
        {

            var imagesListURL = UploadedFile(model);
            var images = new List<ProductImage>();
            foreach (var imageURL in imagesListURL)
            {
                images.Add(new ProductImage() { ImageURL = imageURL });
            }

            var productCategory = _db.Categories.FirstOrDefault(x => x.CategoryId == model.Product.Category.CategoryId);

            var product = new Product()
            {
                Name = model.Product.Name,
                Price = model.Product.Price,
                Category = productCategory,
                Localization = model.Product.Localization,
                AddedDate = DateTime.Now,
                Description = model.Product.Description,
                Images = images,
                CoverURL = images.Select(url => url.ImageURL).LastOrDefault()
            };

            var selectedProvidersId = model.Providers.Where(x => x.Selected).Select(y => y.Value);
            
            List<Provider> providers = new List<Provider>();
            foreach (var provider in selectedProvidersId)
            {
                providers.Add(_db.Providers.Where(x => x.ProviderId == int.Parse(provider)).First());
            }
            product.Providers = providers;

            var seller = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result;
            seller.UserProducts.Add(product);

            _db.Products.Add(product);
            _db.Users.Update(seller);
            await _db.SaveChangesAsync();



            return Redirect("Index");
        }

        private List<string> UploadedFile(NewProductVM model)
        {
            List<string> files = new List<string>();
            string fileNameWithPath = "";
            string uniqueFileName = "";

            if (model.Images != null)
            {
                foreach (var image in model.Images)
                {

                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;

                    fileNameWithPath = Path.Combine(path, uniqueFileName);
                    files.Add(uniqueFileName);


                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                }

            }
            return files;
        }
    }
}
