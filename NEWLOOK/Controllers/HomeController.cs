using Microsoft.AspNetCore.Mvc;
using NEWLOOK.Models;
using NEWLOOK.Models.NewLook;
using System.Diagnostics;

namespace NEWLOOK.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NewLookContext _context;

        public HomeController(NewLookContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                Galleries = _context.Galleries.OrderBy(g => g.Id).Take(6).ToList(),
                Teams = _context.Teams.OrderBy(g => g.Id).Take(3).ToList(),
                ServicesType = _context.ServiceTypes.OrderBy(g => g.Id).Take(3).ToList()
            };
            return View(viewModel);
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