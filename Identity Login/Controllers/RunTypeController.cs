using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Identity_Login.Data;
using Identity_Login.Models.dbModels;

namespace Identity_Login.Controllers
{
    public class RunTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RunTypeController(ApplicationDbContext context) => _context = context;

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAllRunTypes()
        {
            var data = await _context.RunTypes
                .OrderBy(r => r.Name)
                .Select(r => new { r.RunTypeId, r.Name })
                .ToListAsync();
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRunType(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Json(new { success = false, message = "Name is required" });

            var runType = new RunType { Name = name };
            _context.RunTypes.Add(runType);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Run Type added successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> EditRunType(int id, string newName)
        {
            var runType = await _context.RunTypes.FindAsync(id);
            if (runType == null) return Json(new { success = false, message = "Not found" });

            runType.Name = newName;
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Updated successfully!" });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteRunType(int id)
        {
            var runType = await _context.RunTypes.FindAsync(id);
            if (runType == null)
                return Json(new { success = false, message = "Run Type not found." });

            // Check if linked to any RouterJobs
            bool isLinked = await _context.RouterJobs.AnyAsync(j => j.RunTypeId == id);

            if (isLinked)
            {
                return Json(new
                {
                    success = false,
                    message = "This Run Type cannot be deleted because it is currently linked to existing Router Jobs."
                });
            }

            _context.RunTypes.Remove(runType);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Deleted successfully!" });
        }
    }
}