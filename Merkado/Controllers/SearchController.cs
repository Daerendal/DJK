using DJK.DAL;
using DJK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using DJK.ViewModels;

namespace DJK.Controllers
{
    public class SearchController : Controller
    {

        private readonly ILogger<SearchController> _logger;
        private readonly DJKDbContext _db;

        public SearchController(ILogger<SearchController> logger, DJKDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(string NameSearch, string sortOrder, string category, string LocationSearch)
        {



            if (LocationSearch != null)
            {
                SearchVM.UsedLocation = LocationSearch;
                SearchVM.UsedFilter = null;
            }

            if (NameSearch != null)
            {
                SearchVM.UsedFilter = NameSearch;

                if (LocationSearch == null)
                {
                    SearchVM.UsedLocation = null;
                }
            }

            if (category != null)
            {
                SearchVM.UsedCategory = category;

            }
            else if (SearchVM.UsedCategory == null)
            {

                SearchVM.UsedCategory = "Wszystkie";
            }
           
            if (sortOrder != null)
            {
                SearchVM.UsedSort = sortOrder;
            }
            
           



            ViewBag.CurrentSort = SearchVM.UsedSort;
            ViewBag.CurrentFilter = SearchVM.UsedFilter;
            ViewBag.CurrentCategory = SearchVM.UsedCategory;
            ViewBag.CurrentLocation = SearchVM.UsedLocation;




            var searchResult = _db.Products
                                .Include(c => c.Category)
                                .Include(i => i.Images)
                                .Include(p => p.Providers)
                                .Where(s => !s.IsSold)
                                .ToList();

            

            if (!String.IsNullOrEmpty(SearchVM.UsedFilter))
            {
                searchResult = searchResult.Where(s => s.Name!.ToLower().Contains(SearchVM.UsedFilter.Trim().ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(SearchVM.UsedLocation))
            {
                searchResult = searchResult.Where(s => s.Localization!.ToLower().Contains(SearchVM.UsedLocation.Trim().ToLower())).ToList();
            }



            switch (SearchVM.UsedSort)
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
          
            if (!String.IsNullOrEmpty(SearchVM.UsedCategory) && SearchVM.UsedCategory != "Wszystkie")
            {
                searchResult = searchResult.Where(s => s.Category.Name.Equals(SearchVM.UsedCategory)).ToList();
            }

            SearchVM.CurrentProduct = searchResult;

            return View(searchResult);
        }
       

    }
}
