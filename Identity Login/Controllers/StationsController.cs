using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Identity_Login.Data;
using Identity_Login.Models.dbModels;
using System.Security.Claims;

namespace Identity_Login.Controllers
{
    public class StationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Stations.Include(s => s.CreatedByUser).Include(s => s.ProcessStep).ThenInclude(ps => ps.JobProcess).Include(s => s.UpdatedByUser);
            return View(await applicationDbContext.ToListAsync());
        }
        // JSON result for DataTables

        [HttpGet]
        public async Task<IActionResult> GetStationsData()
        {
            var stations = await _context.Stations
                .Include(s => s.ProcessStep)
                .ThenInclude(ps => ps.JobProcess)
                .Select(s => new {
                    s.StationId,
                    s.Name,
                    Process = s.ProcessStep.JobProcess.ProcessId == 2 || s.ProcessStep.JobProcess.ProcessId == 3 ? "Passivation Process" : s.ProcessStep.JobProcess.Name,
                    ProcessStep = s.ProcessStep.StepName
                })
                .ToListAsync();

            return Json(new { data = stations });
        }

        // GET: Stations/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var station = await _context.Stations
                .Include(s => s.CreatedByUser)
                .Include(s => s.ProcessStep)
                .ThenInclude(ps => ps.JobProcess)
                .Include(s => s.UpdatedByUser)
                .FirstOrDefaultAsync(m => m.StationId == id);
            if (station == null)
            {
                return NotFound();
            }

            return View(station);
        }

        //Json result for ProcessSteps

        [HttpGet]
        public JsonResult GetProcessSteps(int processId)
        {
            var steps = _context.ProcessSteps
                .Where(ps => ps.ProcessId == processId)
                .Select(ps => new { ps.ProcessStepId, ps.StepName })
                .ToList();
            return Json(steps);
        }

        //GET: Stations/Create

        public IActionResult Create()
        {
            //ViewBag.Processes = _context.JobProcesses.ToList();
            ViewBag.Processes = new List<JobProcess>
                {
                    _context.JobProcesses.FirstOrDefault(p => p.ProcessId == 1), 
                    new JobProcess { ProcessId = 2, Name = "Passivation Process" }, 
                    _context.JobProcesses.FirstOrDefault(p => p.ProcessId == 4),  
                        _context.JobProcesses.FirstOrDefault(p => p.ProcessId == 5)  

                };
            ViewData["ProcessStepId"] = new SelectList(_context.ProcessSteps, "ProcessStepId", "StepName");
            return View();
        }

        // POST: Stations/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StationId,Name,ProcessStepId")] Station station)
        {
            if (ModelState.IsValid)
            {
                station.CreatedOn = DateTime.UtcNow;
                station.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier); // Current user ID
                station.IsDeleted = false;

                _context.Add(station);
                await _context.SaveChangesAsync();
                TempData["success"] = "The station has been created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Processes = _context.JobProcesses.ToList();
            ViewData["ProcessStepId"] = new SelectList(_context.ProcessSteps, "ProcessStepId", "StepName", station.ProcessStepId);
            return View(station);
        }


        // GET: Stations/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var station = await _context.Stations
                .Include(s => s.ProcessStep)
                .FirstOrDefaultAsync(s => s.StationId == id);

            if (station == null)
            {
                return NotFound();
            }

            // All processes for dropdown
            ViewBag.Processes = new List<JobProcess>
            {
                _context.JobProcesses.FirstOrDefault(p => p.ProcessId == 1),
                new JobProcess { ProcessId = 2, Name = "Passivation Process" },
                _context.JobProcesses.FirstOrDefault(p => p.ProcessId == 4),
                _context.JobProcesses.FirstOrDefault(p => p.ProcessId == 5)  // Chemical Conversion

            };

            // Get the processId from the current ProcessStep
            int? selectedProcessId = station.ProcessStep?.ProcessId;
            ViewBag.SelectedProcessId = selectedProcessId;

            // Steps for the selected process
            var steps = _context.ProcessSteps
                .Where(ps => ps.ProcessId == selectedProcessId)
                .ToList();

            ViewData["ProcessStepId"] = new SelectList(steps, "ProcessStepId", "StepName", station.ProcessStepId);

            return View(station);
        }

        // POST: Stations/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StationId,Name,ProcessStepId")] Station station)
        {
            if (id != station.StationId)
            {
                TempData["error"] = "Invalid station ID.";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingStation = await _context.Stations.FindAsync(id);
                if (existingStation == null)
                {
                    TempData["error"] = "Station not found.";
                    return NotFound();
                }

                // Update editable fields
                existingStation.Name = station.Name;
                existingStation.ProcessStepId = station.ProcessStepId;

                // Set audit fields
                existingStation.UpdatedOn = DateTime.UtcNow;
                existingStation.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Update(existingStation);
                await _context.SaveChangesAsync();
                TempData["success"] = "The station has been updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if model state is invalid
            ViewBag.Processes = _context.JobProcesses.ToList();

            var processStep = await _context.ProcessSteps.FindAsync(station.ProcessStepId);
            int? selectedProcessId = processStep?.ProcessId;
            ViewBag.SelectedProcessId = selectedProcessId;

            var steps = _context.ProcessSteps
                .Where(ps => ps.ProcessId == selectedProcessId)
                .ToList();

            ViewData["ProcessStepId"] = new SelectList(steps, "ProcessStepId", "StepName", station.ProcessStepId);

            return View(station);
        }

        // GET: Stations/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var station = await _context.Stations
                .Include(s => s.CreatedByUser)
                .Include(s => s.ProcessStep)
                .ThenInclude(ps => ps.JobProcess)
                .Include(s => s.UpdatedByUser)
                .FirstOrDefaultAsync(m => m.StationId == id);
            if (station == null)
            {
                return NotFound();
            }

            return View(station);
        }

        // POST: Stations/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station != null)
            {
                _context.Stations.Remove(station);
                await _context.SaveChangesAsync();
                TempData["success"] = "The station has been deleted successfully!";
            }
            else
            {
                TempData["error"] = "Station not found";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StationExists(int id)
        {
            return _context.Stations.Any(e => e.StationId == id);
        }
    }
}
