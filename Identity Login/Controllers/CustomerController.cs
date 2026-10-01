using Identity_Login.Data;
using Identity_Login.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Identity_Login.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Search()
        {
            return View();
        }

        [HttpGet]
        [HttpGet]
        public IActionResult GetCustomersList()
        {
            // Filter jobs by looking at their most recent process stage
            var data = _context.RouterJobs
                .Where(j => !(j.DisappearAfterShipped && _context.JobProcessStages
                    .Where(s => s.JobId == j.JobId)
                    .OrderByDescending(s => s.CreatedOn)
                    .Select(s => s.ProcessStep.StepName)
                    .FirstOrDefault() == "Shipped"))
                .Select(c => c.CustomerName)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return Json(data);
        }


        [HttpPost]
        public async Task<IActionResult> Search(string jobNumber)
        {
            var searchTerm = (jobNumber ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ViewBag.Error = "Please enter a Job Number or PO Number.";
                return View();
            }

            // Find the job by JobNumber
            var job = await _context.RouterJobs
                .FirstOrDefaultAsync(j => j.JobNumber == searchTerm);

            if (job == null)
            {
                // Not a Job Number, so try the PO Number (stored in VerbalNo).
                // The column is read live, so an updated PO Number is matched on the next search.
                var poMatches = await _context.RouterJobs
                    .Where(j => j.VerbalNo == searchTerm)
                    .OrderByDescending(j => j.JobId)
                    .Select(j => new CustomerPoMatchVm
                    {
                        JobId = j.JobId,
                        JobNumber = j.JobNumber,
                        CustomerName = j.CustomerName,
                        PartName = j.PartName
                    })
                    .ToListAsync();

                if (poMatches.Count == 0)
                {
                    ViewBag.Error = "No job found with this Job Number or PO Number.";
                    ViewBag.SearchTerm = searchTerm;
                    return View();
                }

                // One PO Number can cover several jobs, so let the customer pick the Job Number.
                if (poMatches.Count > 1)
                {
                    return View(new CustomerJobSearchViewModel
                    {
                        SearchTerm = searchTerm,
                        PoNumber = searchTerm,
                        PoMatches = poMatches
                    });
                }

                job = await _context.RouterJobs
                    .FirstOrDefaultAsync(j => j.JobId == poMatches[0].JobId);
            }

            if (job == null)
            {
                ViewBag.Error = "No job found with this Job Number or PO Number.";
                ViewBag.SearchTerm = searchTerm;
                return View();
            }

            ViewBag.SearchTerm = searchTerm;

            // Find the process via mapping: JobProcessStage → ProcessStep → JobProcess
            var jobProcessStage = await _context.JobProcessStages
                .Include(jps => jps.ProcessStep)
                .ThenInclude(ps => ps.JobProcess)
                .FirstOrDefaultAsync(jps => jps.JobId == job.JobId);

            if (jobProcessStage == null || jobProcessStage.ProcessStep == null || jobProcessStage.ProcessStep.JobProcess == null)
            {
                ViewBag.Error = "No process mapping found for this job.";
                return View();
            }

            var process = jobProcessStage.ProcessStep.JobProcess;

            // Get all steps for this process
            var allSteps = await _context.ProcessSteps
                .Where(ps => ps.ProcessId == process.ProcessId)
                .OrderBy(ps => ps.StepOrder)
                .ToListAsync();

            // Get all completed steps for this job
            var completedStepIds = await _context.JobProcessStages
                .Where(jps => jps.JobId == job.JobId)
                .Select(jps => jps.ProcessStepId)
                .ToListAsync();

            // Find all completed steps and their StepOrder
            var completedSteps = allSteps
                .Where(step => completedStepIds.Contains(step.ProcessStepId))
                .OrderBy(step => step.StepOrder)
                .ToList();

            int? lastCompletedOrder = completedSteps.Any() ? completedSteps.Max(s => s.StepOrder) : (int?)null;

            var stepsVm = allSteps.Select(step =>
            {
                string status;
                if (completedStepIds.Contains(step.ProcessStepId))
                {
                    status = "Completed";
                }
                else if (lastCompletedOrder.HasValue && step.StepOrder < lastCompletedOrder)
                {
                    status = "N/A";
                }
                else
                {
                    status = "InProgress";
                }
                return new CustomerJobProcessStepVm
                {
                    StepOrder = step.StepOrder,
                    StepName = step.StepName,
                    Status = status
                };
            }).ToList();

            //var stepsVm = allSteps.Select(step => new CustomerJobProcessStepVm
            //{
            //    StepOrder = step.StepOrder,
            //    StepName = step.StepName,
            //    Status = completedStepIds.Contains(step.ProcessStepId) ? "Completed" : "InProgress"
            //}).ToList();

            var vm = new CustomerJobSearchViewModel
            {
                JobNumber = job.JobNumber,
                PoNumber = job.VerbalNo,
                SearchTerm = searchTerm,
                Process = process.Name,
                Steps = stepsVm
            };

            return View(vm);
        }

    }
}
