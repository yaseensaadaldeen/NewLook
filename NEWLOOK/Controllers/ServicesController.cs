using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEWLOOK.Models.NewLook;
using Microsoft.AspNetCore.Authorization;

namespace NEWLOOK.Controllers
{
    public class ServicesController : Controller
    {
        private readonly NewLookContext _context;

        public ServicesController(NewLookContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.MstServices
                .Include(s => s.ServiceTypes)
                .Include(s => s.MstServiceImages)
                .ToListAsync();

            return View(services);
        }

        // Master Service CRUD operations
       
        public IActionResult CreateMasterService()
        {
            ViewBag.Teams = _context.Teams.ToList();
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> CreateMasterService(MstService service)
        {
            ModelState.Remove("Team");
            ModelState.Remove("SerDesc");
            if (service.IconFile != null && service.IconFile.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "images", "services");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(service.IconFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await service.IconFile.CopyToAsync(stream);
                }

                service.ServiceIconImage = "/images/services/" + uniqueFileName;

            
                service.SerDesc="";
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Teams = _context.Teams.ToList();
            return View(service);
        }

       
        public async Task<IActionResult> EditMasterService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.MstServices.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            ViewBag.Teams = _context.Teams.ToList();
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> EditMasterService(int id, [Bind("Id,SerName,SerDesc,ServiceIconImage,TeamId")] MstService service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MstServiceExists(service.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Teams = _context.Teams.ToList();
            return View(service);
        }

       
        public async Task<IActionResult> DeleteMasterService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.MstServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost, ActionName("DeleteMasterService")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMasterServiceConfirmed(int id)
        {
            // Include the sub-services in the query
            var service = await _context.MstServices
                .Include(m => m.ServiceTypes) // Include the related sub-services
                .FirstOrDefaultAsync(m => m.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            try
            {
                // First remove all sub-services
                foreach (var subService in service.ServiceTypes.ToList())
                {
                    _context.ServiceTypes.Remove(subService);
                }

                // Then remove the master service
                _context.MstServices.Remove(service);

                await _context.SaveChangesAsync();

                TempData["DeleteSuccess"] = "Master service and all its sub-services were deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                TempData["DeleteError"] = "Unable to delete service. It may be referenced by other records. Error: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["DeleteError"] = "An unexpected error occurred: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // Sub-Service CRUD operations

        public IActionResult CreateSubService()
        {
            ViewBag.MasterServices = _context.MstServices.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> CreateSubService([Bind("SerTypeName,SerTypeDesc,SerTime,Price,MstSerId")] ServiceType serviceType)
        {
            ModelState.Remove("MstSer");
            ModelState.Remove("active");
            if (ModelState.IsValid)
            {
                _context.Add(serviceType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MasterServices = _context.MstServices.ToList();
            return View(serviceType);
        }

       
        public async Task<IActionResult> EditSubService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceType = await _context.ServiceTypes
                .Include(st => st.MstSer)
                .FirstOrDefaultAsync(st => st.Id == id);

            if (serviceType == null)
            {
                return NotFound();
            }

            ViewBag.MasterServices = _context.MstServices.ToList();
            return View(serviceType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> EditSubService(int id, [Bind("Id,SerTypeName,SerTypeDesc,SerTime,Price,MstSerId")] ServiceType serviceType)
        {
            if (id != serviceType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceTypeExists(serviceType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MasterServices = _context.MstServices.ToList();
            return View(serviceType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubService(int id)
        {
            try
            {
                var serviceType = await _context.ServiceTypes.FindAsync(id);
                if (serviceType == null)
                {
                    TempData["DeleteError"] = "Sub-service not found.";
                    return RedirectToAction(nameof(Index));
                }

                _context.ServiceTypes.Remove(serviceType);
                await _context.SaveChangesAsync();

                TempData["DeleteSuccess"] = "Sub-service deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["DeleteError"] = "An error occurred: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


        private bool MstServiceExists(int id)
        {
            return _context.MstServices.Any(e => e.Id == id);
        }

        private bool ServiceTypeExists(int id)
        {
            return _context.ServiceTypes.Any(e => e.Id == id);
        }
    }
}