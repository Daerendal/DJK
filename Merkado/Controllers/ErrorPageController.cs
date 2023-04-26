using Microsoft.AspNetCore.Mvc;

namespace DJK.Controllers
{
    [Route("ErrorPage/{statuscode}")]
    public class ErrorPageController : Controller
    {
        public IActionResult Index(int statuscode)
        {
            switch(statuscode)
            {
                case 404:
                    ViewData["Error"] = "UPS! ERROR 404! Strony nie znaleziono. Strona do której próbujesz sie dostać nie istnieje!";
                    break;
                default:
                    break;
            }
            return View("ErrorPage");
        }

    }
}
