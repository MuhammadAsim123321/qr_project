using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Identity_Login.Data;
using Identity_Login.Models.dbModels;

namespace Identity_Login.Controllers
{
    public class ProcessStepsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessStepsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProcessSteps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProcessSteps.Include(p => p.CreatedByUser).Include(p => p.JobProcess).Include(p => p.UpdatedByUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProcessSteps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processStep = await _context.ProcessSteps
                .Include(p => p.CreatedByUser)
                .Include(p => p.JobProcess)
                .Include(p => p.UpdatedByUser)
                .FirstOrDefaultAsync(m => m.ProcessStepId == id);
            if (processStep == null)
            {
                return NotFound();
            }

            return View(processStep);
        }

        // GET: ProcessSteps/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id");
            ViewData["ProcessId"] = new SelectList(_context.JobProcesses, "ProcessId", "Name");
            ViewData["UpdatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id");
            return View();
        }

        // POST: ProcessSteps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ProcessStepId,ProcessId,StepName,StepOrder,IsOptional,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,IsDeleted")] ProcessStep processStep)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var maxOrder = await _context.ProcessSteps
        //   .Where(x => x.ProcessId == processStep.ProcessId)
        //   .MaxAsync(x => (int?)x.StepOrder) ?? 0;


        //        processStep.StepOrder = maxOrder + 1;



        //        _context.Add(processStep);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CreatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", processStep.CreatedBy);
        //    ViewData["ProcessId"] = new SelectList(_context.JobProcesses, "ProcessId", "Name", processStep.ProcessId);
        //    ViewData["UpdatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", processStep.UpdatedBy);
        //    return View(processStep);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProcessStepId,ProcessId,StepName,StepOrder,IsOptional,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,IsDeleted")] ProcessStep processStep)
        {
            var steps = await _context.ProcessSteps
                .Where(x => x.ProcessId == processStep.ProcessId)
                .OrderBy(x => x.StepOrder)
                .ToListAsync();

            var maxOrder = steps.Count;

            if (processStep.StepOrder == 0) // user not passed
            {
                processStep.StepOrder = (steps.Max(x => (int?)x.StepOrder) ?? 0) + 1;
            }
            else
            {
                if (processStep.StepOrder > maxOrder + 1)
                {
                    ModelState.AddModelError("StepOrder", $"Invalid StepOrder. Max allowed is {maxOrder + 1}");
                    ViewData["ProcessId"] = new SelectList(_context.JobProcesses, "ProcessId", "Name", processStep.ProcessId);
                    return View(processStep);
                }

                foreach (var s in steps.Where(s => s.StepOrder >= processStep.StepOrder))
                {
                    s.StepOrder++;
                    _context.Update(s);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(processStep);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProcessId"] = new SelectList(_context.JobProcesses, "ProcessId", "Name", processStep.ProcessId);
            return View(processStep);
        }


        // GET: ProcessSteps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processStep = await _context.ProcessSteps.FindAsync(id);
            if (processStep == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", processStep.CreatedBy);
            ViewData["ProcessId"] = new SelectList(_context.JobProcesses, "ProcessId", "Name", processStep.ProcessId);
            ViewData["UpdatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", processStep.UpdatedBy);
            return View(processStep);
        }

        // POST: ProcessSteps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProcessStepId,ProcessId,StepName,StepOrder,IsOptional,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,IsDeleted")] ProcessStep processStep)
        {
            if (id != processStep.ProcessStepId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processStep);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessStepExists(processStep.ProcessStepId))
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
            ViewData["CreatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", processStep.CreatedBy);
            ViewData["ProcessId"] = new SelectList(_context.JobProcesses, "ProcessId", "Name", processStep.ProcessId);
            ViewData["UpdatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", processStep.UpdatedBy);
            return View(processStep);
        }

        // GET: ProcessSteps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processStep = await _context.ProcessSteps
                .Include(p => p.CreatedByUser)
                .Include(p => p.JobProcess)
                .Include(p => p.UpdatedByUser)
                .FirstOrDefaultAsync(m => m.ProcessStepId == id);
            if (processStep == null)
            {
                return NotFound();
            }

            return View(processStep);
        }

        // POST: ProcessSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var processStep = await _context.ProcessSteps.FindAsync(id);
            if (processStep != null)
            {
                _context.ProcessSteps.Remove(processStep);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        [HttpPost]
        public async Task<IActionResult> DeleteProcessStep(int id)
        {
            var processStep = await _context.ProcessSteps.FindAsync(id);
            if (processStep == null)
            {
                return Json(new { success = false, message = "Process Step not found." });
            }

            // Check if linked to any JobProcessStages (router job stages)
            bool isLinkedToStages = await _context.JobProcessStages.AnyAsync(s => s.ProcessStepId == id);

            // Check if linked to any Stations
            bool isLinkedToStations = await _context.Stations.AnyAsync(s => s.ProcessStepId == id);

            if (isLinkedToStages || isLinkedToStations)
            {
                return Json(new
                {
                    success = false,
                    message = "This Process Step cannot be deleted because it is currently linked to existing job stages or stations."
                });
            }

            _context.ProcessSteps.Remove(processStep);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Deleted successfully!" });
        }
        private bool ProcessStepExists(int id)
        {
            return _context.ProcessSteps.Any(e => e.ProcessStepId == id);
        }
    }
}
