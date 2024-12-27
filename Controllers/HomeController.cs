using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TesteDBM.Models; // onde está seu ErrorViewModel (explicado abaixo)

namespace TesteDBM.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Views/Home/Index.cshtml
        }

        // GET: /Home/Privacy
        public IActionResult Privacy()
        {
            return View(); // Views/Home/Privacy.cshtml
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}