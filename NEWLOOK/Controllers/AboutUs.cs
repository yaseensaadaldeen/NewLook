using Microsoft.AspNetCore.Mvc;

namespace NEWLOOK.Controllers
{
    public class AboutUs : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
