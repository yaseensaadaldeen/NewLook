using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEWLOOK.Models.NewLook;


namespace NEWLOOK.Controllers
{
    public class GalleryController : Controller
    {
        private readonly NewLookContext _context;

        public GalleryController(NewLookContext context)
        {
            _context = context;
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
            if (gallery.ImageFile != null && gallery.ImageFile.Length > 0)
            {
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
