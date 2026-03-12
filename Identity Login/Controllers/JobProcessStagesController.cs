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
    public class JobProcessStagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobProcessStagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobProcessStages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.JobProcessStages.Include(j => j.CreatedByUser).Include(j => j.ProcessStep).Include(j => j.RouterJob).Include(j => j.UpdatedByUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: JobProcessStages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobProcessStage = await _context.JobProcessStages
                .Include(j => j.CreatedByUser)
                .Include(j => j.ProcessStep)
                .Include(j => j.RouterJob)
                .Include(j => j.UpdatedByUser)
                .FirstOrDefaultAsync(m => m.JobProcessStageId == id);
            if (jobProcessStage == null)
            {
                return NotFound();
            }

            return View(jobProcessStage);
        }

        // GET: JobProcessStages/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id");
            ViewData["ProcessStepId"] = new SelectList(_context.ProcessSteps, "ProcessStepId", "StepName");
            ViewData["JobId"] = new SelectList(_context.RouterJobs, "JobId", "JobNumber");
            ViewData["UpdatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id");
            return View();
        }

        // POST: JobProcessStages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobProcessStageId,JobId,ProcessStepId,StageStatus,CompletedOn,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,IsDeleted")] JobProcessStage jobProcessStage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobProcessStage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", jobProcessStage.CreatedBy);
            ViewData["ProcessStepId"] = new SelectList(_context.ProcessSteps, "ProcessStepId", "StepName", jobProcessStage.ProcessStepId);
            ViewData["JobId"] = new SelectList(_context.RouterJobs, "JobId", "JobNumber", jobProcessStage.JobId);
            ViewData["UpdatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", jobProcessStage.UpdatedBy);
            return View(jobProcessStage);
        }

        // GET: JobProcessStages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobProcessStage = await _context.JobProcessStages.FindAsync(id);
            if (jobProcessStage == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", jobProcessStage.CreatedBy);
            ViewData["ProcessStepId"] = new SelectList(_context.ProcessSteps, "ProcessStepId", "StepName", jobProcessStage.ProcessStepId);
            ViewData["JobId"] = new SelectList(_context.RouterJobs, "JobId", "JobNumber", jobProcessStage.JobId);
            ViewData["UpdatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", jobProcessStage.UpdatedBy);
            return View(jobProcessStage);
        }

        // POST: JobProcessStages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobProcessStageId,JobId,ProcessStepId,StageStatus,CompletedOn,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,IsDeleted")] JobProcessStage jobProcessStage)
        {
            if (id != jobProcessStage.JobProcessStageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobProcessStage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobProcessStageExists(jobProcessStage.JobProcessStageId))
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
            ViewData["CreatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", jobProcessStage.CreatedBy);
            ViewData["ProcessStepId"] = new SelectList(_context.ProcessSteps, "ProcessStepId", "StepName", jobProcessStage.ProcessStepId);
            ViewData["JobId"] = new SelectList(_context.RouterJobs, "JobId", "JobNumber", jobProcessStage.JobId);
            ViewData["UpdatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", jobProcessStage.UpdatedBy);
            return View(jobProcessStage);
        }

        // GET: JobProcessStages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobProcessStage = await _context.JobProcessStages
                .Include(j => j.CreatedByUser)
                .Include(j => j.ProcessStep)
                .Include(j => j.RouterJob)
                .Include(j => j.UpdatedByUser)
                .FirstOrDefaultAsync(m => m.JobProcessStageId == id);
            if (jobProcessStage == null)
            {
                return NotFound();
            }

            return View(jobProcessStage);
        }

        // POST: JobProcessStages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobProcessStage = await _context.JobProcessStages.FindAsync(id);
            if (jobProcessStage != null)
            {
                _context.JobProcessStages.Remove(jobProcessStage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobProcessStageExists(int id)
        {
            return _context.JobProcessStages.Any(e => e.JobProcessStageId == id);
        }
    }
}
