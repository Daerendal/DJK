using Merkado.DAL;
using Merkado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace Merkado.Controllers
{
    public class SearchController : Controller
    {

        private readonly ILogger<SearchController> _logger;
        private readonly MerkadoDbContext _db;

        public SearchController(ILogger<SearchController> logger, MerkadoDbContext db)
        {
            _logger = logger;
            _db = db;
        }
      
        public async Task<IActionResult> Index(string NameSearch, string currentFilter, string sortOrder, string category, string LocationSearch)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentFilter = NameSearch;
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentLocation = LocationSearch;


            var searchResult = _db.Products
                                .Include(c => c.Category)
                                .Include(i => i.Images)
                                .Include(p => p.Providers)
                                .ToList();

            

            if (!String.IsNullOrEmpty(NameSearch))
            {
                searchResult = searchResult.Where(s => s.Name!.ToLower().Contains(NameSearch.Trim().ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(LocationSearch))
            {
                searchResult = searchResult.Where(s => s.Localization!.ToLower().Contains(LocationSearch.Trim().ToLower())).ToList();
            }



            switch (sortOrder)
            {
                case "name_desc":
                    searchResult = searchResult.OrderByDescending(s => s.Name).ToList();
                    ViewBag.CurrentSortName = "Alfabetycznie Z-A";
                    break;
                case "date":
                    searchResult = searchResult.OrderBy(s => s.AddedDate).ToList();
                    ViewBag.CurrentSortName = "Od Najnowszego";
                    break;
                case "date_desc":
                    searchResult = searchResult.OrderByDescending(s => s.AddedDate).ToList();
                    ViewBag.CurrentSortName = "Od Najstarszego";
                    break;
                case "name":
                    searchResult = searchResult.OrderBy(s => s.Name).ToList();
                    ViewBag.CurrentSortName = "Alfabetycznie A-Z";
                    break;
                default:
                    searchResult = searchResult.OrderBy(s => s.Name).ToList();
                    ViewBag.CurrentSortName = "Alfabetycznie A-Z";
                    ViewBag.CurrentSort = "name";
                    break;
            }
          
            if (!String.IsNullOrEmpty(category))
            {
                searchResult = searchResult.Where(s => s.Category.Name.Equals(category)).ToList();
            }

            return View(searchResult);
        }
       

    }
}
