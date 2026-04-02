using Identity_Login.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Identity_Login.Controllers
{
    public class UnionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        private readonly ApplicationDbContext _context;

        public UnionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MergeCustomers(string primaryCustomer, string mergeCustomer)
        {
            if (string.IsNullOrEmpty(primaryCustomer) || string.IsNullOrEmpty(mergeCustomer))
            {
                TempData["error"] = "Both Primary and Merge customer names are required.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var jobsToUpdate = await _context.RouterJobs
                    .Where(j => j.CustomerName.ToLower() == mergeCustomer.ToLower())
                    .ToListAsync();

                if (jobsToUpdate.Count == 0)
                {
                    TempData["error"] = $"No jobs found for customer: {mergeCustomer}";
                    return RedirectToAction(nameof(Index));
                }

                //  Update the CustomerName to the Primary Customer name
                foreach (var job in jobsToUpdate)
                {
                    job.CustomerName = primaryCustomer;
                    job.UpdatedOn = DateTime.UtcNow;
                    job.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                }

                await _context.SaveChangesAsync();

                TempData["success"] = $"Successfully merged {jobsToUpdate.Count} records from '{mergeCustomer}' into '{primaryCustomer}'.";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error during merge: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
