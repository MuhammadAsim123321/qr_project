using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Identity_Login.Data;
using Identity_Login.Models;
using System.Linq;
using System.Threading.Tasks;
using Identity_Login.Models.dbModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Identity_Login.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // For DataTables AJAX
        [HttpGet]
        public async Task<IActionResult> GetUsersData()
        {
            var users = await _context.applicationUsers.Where(r=>r.IsDeleted==null || r.IsDeleted == false)
                .Select(u => new {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    Password = "********",
                    // Get role (first role if multiple)
                    //Role = (from userRole in _context.UserRoles
                    //        join role in _context.Roles on userRole.RoleId equals role.Id
                    //        where userRole.UserId == u.Id
                    //        select role.Name).FirstOrDefault(),
                    // Get station (first mapping if multiple)
                    Station = (from map in _context.StaffStationMappings
                               join station in _context.Stations on map.StationId equals station.StationId
                               where map.Id == u.Id
                               select station.Name).FirstOrDefault()
                })
                .ToListAsync();

            return Json(new { data = users });
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.applicationUsers
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            // Get station name
            var station = await (from map in _context.StaffStationMappings
                                 join s in _context.Stations on map.StationId equals s.StationId
                                 where map.Id == user.Id
                                 select s.Name).FirstOrDefaultAsync();
            ViewBag.Station = station;
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.applicationUsers
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            // Get station name
            var station = (from map in _context.StaffStationMappings
                           join s in _context.Stations on map.StationId equals s.StationId
                           where map.Id == user.Id
                           select s.Name).FirstOrDefault();

            ViewBag.Station = station;
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Remove all StaffStationMappings for this user
            var mappings = _context.StaffStationMappings.Where(m => m.Id == id);
            _context.StaffStationMappings.RemoveRange(mappings);

            // Now remove the user
            var user = await _context.applicationUsers.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                _context.applicationUsers.Update(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.applicationUsers
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            // Get station mapping
            var stationId = await _context.StaffStationMappings
                .Where(m => m.Id == user.Id)
                .Select(m => (int?)m.StationId)
                .FirstOrDefaultAsync();

            // Get roles
            var userRole = await (from Urole in _context.UserRoles
                                  join role in _context.Roles on Urole.RoleId equals role.Id
                                  where Urole.UserId == user.Id
                                  select role.Name).FirstOrDefaultAsync();

            // Prepare role list (same as Register)
            var roleList = _context.Roles
                .Where(r => r.Name == "Admin" || r.Name == "Supervisor")
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList();

            // Prepare stations list
            var stations = new SelectList(_context.Stations.ToList(), "StationId", "Name");

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = userRole,
                RoleList = roleList,
                StationId = stationId,
                Stations = stations
            };

            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns
                model.RoleList = _context.Roles
                    .Where(r => r.Name == "Admin" || r.Name == "Supervisor")
                    .Select(r => new SelectListItem
                    {
                        Value = r.Name,
                        Text = r.Name
                    }).ToList();
                model.Stations = new SelectList(_context.Stations.ToList(), "StationId", "Name");
                return View(model);
            }

            var user = await _context.applicationUsers.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (user == null)
                return NotFound();

            // Update fields
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;

            // Update role
            var userRoles = await _context.UserRoles.Where(ur => ur.UserId == user.Id).ToListAsync();
            _context.UserRoles.RemoveRange(userRoles);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == model.Role);
            if (role != null)
            {
                _context.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
            }

            // Update station mapping
            var mapping = await _context.StaffStationMappings.FirstOrDefaultAsync(m => m.Id == user.Id);
            if (mapping != null)
            {
                mapping.StationId = model.StationId ?? mapping.StationId;
            }
            else if (model.StationId.HasValue)
            {
                _context.StaffStationMappings.Add(new StaffStationMapping
                {
                    Id = user.Id,
                    StationId = model.StationId.Value,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}