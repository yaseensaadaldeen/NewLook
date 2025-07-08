using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NEWLOOK.Models.NewLook;
using static NEWLOOK.Models.NewLook.NewLookContext;


namespace NEWLOOK.Controllers
{
    public class GalleryController : Controller
    {
        private readonly NewLookContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ImageSettings _imgSettings;
        public GalleryController(NewLookContext context, IWebHostEnvironment env, IOptions<ImageSettings> imgSettings)
        {
            _context = context;
            _env = env;
            _imgSettings = imgSettings.Value;
        }
        public async Task<IActionResult> Index()
        {
            var galleryItems = await _context.Galleries.ToListAsync();
            return View(galleryItems);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Gallery gallery)
        {
            try
            {


            if (gallery.ImageFile != null && gallery.ImageFile.Length > 0)
            {
                long maxBytes = _imgSettings.MaxImageSizeInMB * 1024 * 1024;

                if (gallery.ImageFile.Length > maxBytes)
                {
                    TempData["ErrorMessage"] = $"Image must be {_imgSettings.MaxImageSizeInMB} MB or smaller.";
                    ModelState.AddModelError("ImageFile", $"Image must be {_imgSettings.MaxImageSizeInMB} MB or smaller.");
                    return View(gallery);
                }
                var fileName = Path.GetFileName(gallery.ImageFile.FileName);
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/gallery");
                Directory.CreateDirectory(uploads);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await gallery.ImageFile.CopyToAsync(stream);
                }

                gallery.ImageLink = "images/gallery/" + fileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(gallery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(gallery);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error with adding the gallary image";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Galleries.FindAsync(id);
            if (item != null)
            {
                _context.Galleries.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
