using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Identity_Login.Data;
using Identity_Login.Models.dbModels;

namespace Identity_Login.Controllers
{
    public class MaterialTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaterialTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAllMaterialTypes()
        {
            // Using your model name 'Materails' and 'MaterailId'
            var data = await _context.Materails
                .OrderBy(m => m.Name)
                .Select(m => new { m.MaterailId, m.Name })
                .ToListAsync();
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterialType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(new { success = false, message = "Name is required." });

            var material = new Materail { Name = name };
            _context.Materails.Add(material);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Material added successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> EditMaterialType(int id, string newName)
        {
            var material = await _context.Materails.FindAsync(id);
            if (material == null) return Json(new { success = false, message = "Not found." });

            material.Name = newName;
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Updated successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMaterialType(int id)
        {
            var material = await _context.Materails.FindAsync(id);
            if (material == null)
                return Json(new { success = false, message = "Material not found." });

            // Check if linked to any RouterJobs (using the column name MaterailId)
            bool isLinked = await _context.RouterJobs.AnyAsync(j => j.MaterailId == id);

            if (isLinked)
            {
                return Json(new
                {
                    success = false,
                    message = "This Material cannot be deleted because it is currently linked to existing Router Jobs."
                });
            }

            _context.Materails.Remove(material);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Material deleted successfully!" });
        }
    }
}