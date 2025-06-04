using Microsoft.AspNetCore.Mvc;

namespace NEWLOOK.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index(string success="") {
            if (!string.IsNullOrEmpty(success) && success.ToLower() == "true")
            {
                TempData["SuccessMessage"] = "Your message was sent successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }
       
    }
}
