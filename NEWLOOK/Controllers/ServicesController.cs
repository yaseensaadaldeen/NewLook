using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEWLOOK.Models.NewLook;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static NEWLOOK.Models.NewLook.NewLookContext;
using Microsoft.Extensions.Options;

namespace NEWLOOK.Controllers
{
    public class ServicesController : Controller
    {
        private readonly NewLookContext _context;
        private readonly IWebHostEnvironment _env;

        public ServicesController(NewLookContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.MstServices
              .Where(ms => ms.Active == "Y")
              .Include(ms => ms.ServiceTypes.Where(st => st.Active == "Y"))
              .Include(ms => ms.MstServiceImages)
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
            try
            {

           
            ModelState.Remove("Team");
            ModelState.Remove("SerDesc");
                if (!string.IsNullOrEmpty(service.SerName))
                {
                    service.SerDesc="";
                    _context.Add(service);
                    await _context.SaveChangesAsync();
                    ViewBag.Teams = _context.Teams.ToList();
                    return RedirectToAction(nameof(Index));
                }
                else {
                    TempData["ErrorMessage"] = "Error with adding the service";
                    return RedirectToAction(nameof(Index));
                }
              
            
         
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error with adding the service";
                return RedirectToAction(nameof(Index));
            }
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
                return NotFound();
            ModelState.Remove("Team");
            ModelState.Remove("SerDesc");
            ModelState.Remove("Active");

            if (!ModelState.IsValid)
            {
                ViewBag.Teams = _context.Teams.ToList();
                return View(service);
            }

            try
            {
                if (string.IsNullOrEmpty(service.SerDesc))
                    service.SerDesc = "";
                service.Active = "Y";

                _context.Update(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.MstServices.Any(e => e.Id == service.Id))
                    return NotFound();
                throw;
            }
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
                .Include(m => m.ServiceTypes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            try
            {
                // Soft delete sub-services
                foreach (var subService in service.ServiceTypes)
                {
                    subService.Active = "N";
                    _context.ServiceTypes.Update(subService);
                }

                // Soft delete master service
                service.Active = "N";
                _context.MstServices.Update(service);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Master service and its sub-services were marked as inactive.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "Unable to update service. Error: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred: " + ex.Message;
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
            try
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
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error with adding the service";
                return RedirectToAction(nameof(Index));
            }
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
                return NotFound();
               ModelState.Remove("MstSer");
            ModelState.Remove("active");
            if (ModelState.IsValid)
            {
                try
                {
                    serviceType.Active = "Y"; // optional, ensure active
                    _context.Update(serviceType);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceTypeExists(serviceType.Id))
                        return NotFound();
                    else
                        throw;
                }
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
                    TempData["ErrorMessage"] = "Sub-service not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Soft delete by setting Active to "N"
                serviceType.Active = "N";
                _context.ServiceTypes.Update(serviceType);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Sub-service was marked as inactive.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
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

        [HttpPost]
        public IActionResult StoreCartItems(string cartJson)
        {
            if (!string.IsNullOrEmpty(cartJson))
            {
                try
                {
                    // Step 1: Deserialize JSON to List<ServiceType>
                    var services = JsonConvert.DeserializeObject<List<ServiceType>>(cartJson);

                    if (services != null && services.Any())
                    {
                        // Optional: Process/validate services here if needed

                        // Step 2: Store it back in session as a JSON string
                        string serializedServices = JsonConvert.SerializeObject(services);
                        HttpContext.Session.SetString("CartItems", serializedServices);

                        // Redirect to Booking/Create since services are present
                        return RedirectToAction("Create", "Booking");
                    }
                    else
                    {
                        // Services is empty, add model error and return to the view with message
                        TempData["ErrorMessage"] =  "No services provided. Please add at least one service.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Invalid service data provided.";
                    return View("Create");
                }
            }

            TempData["ErrorMessage"] =  "No services provided. Please add at least one service.";
            return RedirectToAction(nameof(Index));
        }


    }
}