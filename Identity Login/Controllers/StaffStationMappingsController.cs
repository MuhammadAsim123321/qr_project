using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Identity_Login.Data;
using Identity_Login.Models.dbModels;
using static System.Collections.Specialized.BitVector32;
using System.Security.Claims;

namespace Identity_Login.Controllers
{
    public class StaffStationMappingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaffStationMappingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StaffStationMappings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StaffStationMappings.Include(s => s.ApplicationUser).Include(s => s.CreatedByUser).Include(s => s.Station).Include(s => s.UpdatedByUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // JSON result for DataTables

        [HttpGet]
        public async Task<IActionResult> GetStaffStationMappingsData()
        {
            var mappings = await _context.StaffStationMappings
                .Include(s => s.ApplicationUser)
                .Include(s => s.Station)
                .Where(s => s.ApplicationUser != null && !s.ApplicationUser.IsDeleted)
                .Select(s => new
                {
                    s.MappingId,
                    StaffName = s.ApplicationUser.FirstName + " " + s.ApplicationUser.LastName,
                    StationName = s.Station.Name
                })
                .ToListAsync();

            return Json(new { data = mappings });
        }

        public IActionResult Create()
        {
            // Get all user IDs in the Supervisor role
            var supervisorIds = _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Supervisor")))
                .Select(u => u.Id)
                .ToList();

            // Get only supervisor users
            var supervisors = _context.applicationUsers
                .Where(u => supervisorIds.Contains(u.Id) && !u.IsDeleted)
                .Select(u => new { u.Id, FullName = u.FirstName + " " + u.LastName })
                .ToList();

            ViewBag.Users = new SelectList(supervisors, "Id", "FullName");
            ViewBag.Stations = new SelectList(_context.Stations.ToList(), "StationId", "Name");
            return View();
        }

        // POST: StaffStationMappings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StationId")] StaffStationMapping staffStationMapping)
        {
            if (ModelState.IsValid)
            {
                staffStationMapping.CreatedOn = DateTime.UtcNow;
                staffStationMapping.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier); // Current user ID
                staffStationMapping.IsDeleted = false;
                _context.Add(staffStationMapping);
                await _context.SaveChangesAsync();
                TempData["success"] = "The staff-station mapping has been created successfully!";
                return RedirectToAction(nameof(Index));
            }
            // Repopulate dropdowns as SelectLists (same as GET)
            var supervisorIds = _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Supervisor")))
                .Select(u => u.Id)
                .ToList();

            var supervisors = _context.applicationUsers
                .Where(u => supervisorIds.Contains(u.Id) && !u.IsDeleted)
                .Select(u => new { u.Id, FullName = u.FirstName + " " + u.LastName })
                .ToList();

            ViewBag.Users = new SelectList(supervisors, "Id", "FullName");
            ViewBag.Stations = new SelectList(_context.Stations.ToList(), "StationId", "Name");

            // Return the same view with validation errors
            return View(staffStationMapping);

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var staffStationMapping = await _context.StaffStationMappings
                .Include(ssm => ssm.ApplicationUser) 
                .FirstOrDefaultAsync(ssm => ssm.MappingId == id);

            if (staffStationMapping == null)
                return NotFound();

            // Get supervisor IDs
            var supervisorIds = await _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Supervisor")))
                .Select(u => u.Id)
                .ToListAsync();

            // Get supervisors for the dropdown
            var supervisors = await _context.applicationUsers
                .Where(u => supervisorIds.Contains(u.Id) && !u.IsDeleted)
                .Select(u => new { u.Id, FullName = u.FirstName + " " + u.LastName })
                .ToListAsync();

            // Create SelectList for users, ensuring the current staffStationMapping.Id is selected if valid
            ViewBag.Users = new SelectList(supervisors, "Id", "FullName", staffStationMapping.Id);

            // Create SelectList for stations
            ViewBag.Stations = new SelectList(
                await _context.Stations.ToListAsync(),
                "StationId",
                "Name",
                staffStationMapping.StationId
            );

            return View(staffStationMapping);
        }
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("MappingId,Id,StationId")] StaffStationMapping staffStationMapping)
        {

            if (ModelState.IsValid)
            {
                var existingMapping = await _context.StaffStationMappings.FindAsync(staffStationMapping.MappingId);
                if (existingMapping == null)
                {
                    TempData["error"] = "Mapping not found.";
                    return NotFound();
                }

                existingMapping.Id = staffStationMapping.Id;
                existingMapping.StationId = staffStationMapping.StationId;
                existingMapping.UpdatedOn = DateTime.UtcNow;
                existingMapping.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Update(existingMapping);
                await _context.SaveChangesAsync();
                TempData["success"] = "The staff-station mapping has been updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if validation fails
            var supervisorIds = _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Supervisor")))
                .Select(u => u.Id)
                .ToList();

            var supervisors = _context.applicationUsers
                .Where(u => supervisorIds.Contains(u.Id) && !u.IsDeleted)
                .Select(u => new { u.Id, FullName = u.FirstName + " " + u.LastName })
                .ToList();

            ViewBag.Users = new SelectList(supervisors, "Id", "FullName", staffStationMapping.Id);
            ViewBag.Stations = new SelectList(_context.Stations.ToList(), "StationId", "Name", staffStationMapping.StationId);

            return View(staffStationMapping);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mapping = await _context.StaffStationMappings
                .Include(s => s.ApplicationUser)
                .Include(s => s.Station)
                .FirstOrDefaultAsync(m => m.MappingId == id && (m.ApplicationUser == null || !m.ApplicationUser.IsDeleted));

            if (mapping == null)
            {
                return NotFound();
            }

            return View(mapping);
        }

        // GET: StaffStationMappings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mapping = await _context.StaffStationMappings
                .Include(s => s.ApplicationUser)
                .Include(s => s.Station)
                .FirstOrDefaultAsync(m => m.MappingId == id && (m.ApplicationUser == null || !m.ApplicationUser.IsDeleted));

            if (mapping == null)
            {
                return NotFound();
            }

            return View(mapping);
        }

        // POST: StaffStationMappings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mapping = await _context.StaffStationMappings.FindAsync(id);
            if (mapping != null)
            {
                _context.StaffStationMappings.Remove(mapping);
                await _context.SaveChangesAsync();
                TempData["success"] = "The mapping has been deleted successfully!";
            }
            else
            {
                TempData["error"] = "Mapping not found.";
            }
            return RedirectToAction(nameof(Index));
        }
        private bool StaffStationMappingExists(int id)
        {
            return _context.StaffStationMappings.Any(e => e.MappingId == id);
        }
    }
}
