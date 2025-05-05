using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEWLOOK.Models.NewLook;
using System.Collections.Generic;
using System.Linq;

namespace NEWLOOK.Controllers
{
    public class TeamController : Controller
    {
        private readonly NewLookContext _context;

        // Inject the context through constructor
        public TeamController(NewLookContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Include related data if needed (TeamType)
            var teams = _context.Teams
                .Include(t => t.TmType) // Include related entity if you need it
                .Where(a => a.Active == "Y")
                .ToList();

            return View(teams);
        }
    }
}