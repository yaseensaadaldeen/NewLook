using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NEWLOOK.Models.NewLook;
using static NEWLOOK.Models.NewLook.NewLookContext;

namespace NEWLOOK.Controllers
{
    public class TeamsController : Controller
    {
        private readonly NewLookContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ImageSettings _imageSettings;

        public TeamsController(NewLookContext context, IWebHostEnvironment env, IOptions<ImageSettings> imageSettings)
        {
            _context = context;
            _env = env;
            _imageSettings = imageSettings.Value;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams.Where(a=>a.Active =="Y").ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team team, IFormFile imageFile)
        {
            try
            {


                ModelState.Remove("active");
                if (ModelState.IsValid)
                {

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        long maxFileSizeBytes = _imageSettings.MaxImageSizeInMB * 1024 * 1024;

                        if (imageFile.Length > maxFileSizeBytes)
                        {
                            TempData["ErrorMessage"] = "Image size cannot exceed 3 MB.";
                            ModelState.AddModelError("ImageLink", "Image size cannot exceed 3 MB.");
                            return View(team);
                        }
                        var uploadsFolder = Path.Combine("wwwroot", "images", "team");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        team.ImageLink = "/images/team/" + uniqueFileName; // Save path to DB
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Please select the team member image.";
                        return View(team);
                    }

                    _context.Add(team);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(team);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error with adding the team member";
                return RedirectToAction(nameof(Index));
            }
        }
        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Team team, IFormFile imageFile)
        {
            if (id != team.Id)
                return NotFound();

            ModelState.Remove("active"); // If you want to ignore this during validation
            ModelState.Remove("ImageLink");
            if (ModelState.IsValid)
            {
                try
                {
                    var existingTeam = await _context.Teams.FindAsync(id);
                    if (existingTeam == null)
                        return NotFound();

                    // Update properties
                    existingTeam.EmpName = team.EmpName;
                    existingTeam.EmpSkills = team.EmpSkills;
                    existingTeam.Nationality = team.Nationality;
                    existingTeam.Experiances = team.Experiances;
                    existingTeam.Languages = team.Languages;
                    existingTeam.CountryWork = team.CountryWork;
                    existingTeam.Active = team.Active;

                    // Handle image update
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        long maxFileSizeBytes = _imageSettings.MaxImageSizeInMB * 1024 * 1024;
                        if (imageFile.Length > maxFileSizeBytes)
                        {
                            TempData["ErrorMessage"] = "Image size cannot exceed 3 MB.";
                            return View(team);
                        }

                        var uploadsFolder = Path.Combine("wwwroot", "images", "team");
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        existingTeam.ImageLink = "/images/team/" + uniqueFileName;
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Error editing the team member.";
                }
            }

            return View(team);
        }


        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                // Soft delete by marking Active = "N"
                team.Active = "N";
                _context.Teams.Update(team);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
