using Microsoft.AspNetCore.Mvc;

namespace NEWLOOK.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
