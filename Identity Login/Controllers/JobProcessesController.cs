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
    public class JobProcessesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobProcessesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobProcesses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.JobProcesses.Include(j => j.CreatedByUser).Include(j => j.UpdatedByUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: JobProcesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobProcess = await _context.JobProcesses
                .Include(j => j.CreatedByUser)
                .Include(j => j.UpdatedByUser)
                .FirstOrDefaultAsync(m => m.ProcessId == id);
            if (jobProcess == null)
            {
                return NotFound();
            }

            return View(jobProcess);
        }

        // GET: JobProcesses/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id");
            ViewData["UpdatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id");
            return View();
        }

        // POST: JobProcesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProcessId,Name,Description,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,IsDeleted")] JobProcess jobProcess)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobProcess);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", jobProcess.CreatedBy);
            ViewData["UpdatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", jobProcess.UpdatedBy);
            return View(jobProcess);
        }

        // GET: JobProcesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobProcess = await _context.JobProcesses.FindAsync(id);
            if (jobProcess == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", jobProcess.CreatedBy);
            ViewData["UpdatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", jobProcess.UpdatedBy);
            return View(jobProcess);
        }

        // POST: JobProcesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProcessId,Name,Description,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,IsDeleted")] JobProcess jobProcess)
        {
            if (id != jobProcess.ProcessId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobProcess);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobProcessExists(jobProcess.ProcessId))
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
            ViewData["CreatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", jobProcess.CreatedBy);
            ViewData["UpdatedBy"] = new SelectList(_context.applicationUsers, "Id", "Id", jobProcess.UpdatedBy);
            return View(jobProcess);
        }

        // GET: JobProcesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobProcess = await _context.JobProcesses
                .Include(j => j.CreatedByUser)
                .Include(j => j.UpdatedByUser)
                .FirstOrDefaultAsync(m => m.ProcessId == id);
            if (jobProcess == null)
            {
                return NotFound();
            }

            return View(jobProcess);
        }

        // POST: JobProcesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobProcess = await _context.JobProcesses.FindAsync(id);
            if (jobProcess != null)
            {
                _context.JobProcesses.Remove(jobProcess);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobProcessExists(int id)
        {
            return _context.JobProcesses.Any(e => e.ProcessId == id);
        }
    }
}
