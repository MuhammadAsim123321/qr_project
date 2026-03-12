using System.Security.Claims;
using System.Text.Json;
using Identity_Login.Data;
using Identity_Login.Enums;
using Identity_Login.Models.dbModels;
using Identity_Login.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity_Login.Controllers
{
    public class QrScannerController : Controller
    {

        private readonly ApplicationDbContext _context;

        public QrScannerController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ScanResult(string qrData)
        {
            // Try to find the job by QR code or JobNumber or whatever your logic is
            //var jobCheck = await _context.RouterJobs.FirstOrDefaultAsync(j => j.JobNumber == qrData || j.QrCodeData == qrData);
            //if (jobCheck == null)
            //{
            //    TempData["error"] = "Invalid QR code or job not found.";
            //    return RedirectToAction("Index");
            //}
            int? jobId = null;
            string error = null;
            string success = null;

            if (!string.IsNullOrEmpty(qrData))
            {
                try
                {
                    using var doc = JsonDocument.Parse(qrData);
                    if (doc.RootElement.TryGetProperty("jobId", out var jobIdProp))
                    {
                        jobId = jobIdProp.GetInt32();
                    }
                    else if (doc.RootElement.TryGetProperty("JobId", out var jobIdProp2))
                    {
                        jobId = jobIdProp2.GetInt32();
                    }
                    else
                    {
                        error = "jobId not found in QR data.";
                    }
                }
                catch
                {
                    error = "Invalid QR data format.";
                }
            }

            if (jobId != null && error == null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                var mapping = await _context.StaffStationMappings
                    .Include(m => m.Station)
                    .FirstOrDefaultAsync(m => m.Id == userId);

                if (mapping == null)
                {
                    error = "No station mapping found for current user.";
                }
                else
                {
                    var processStepId = mapping.Station?.ProcessStepId;
                    if (processStepId == null)
                    {
                        error = "No process step found for your station.";
                    }
                    else
                    {
                        
                        var jobProcessStage = new JobProcessStage
                        {
                            JobId = jobId.Value,
                            ProcessStepId = processStepId.Value,
                            StageStatus = ProcessStageStatus.Completed, 
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = userId,
                            IsDeleted = false
                        };
                        _context.JobProcessStages.Add(jobProcessStage);
                        await _context.SaveChangesAsync();
                        // Load related data for the card
                        var job = await _context.RouterJobs.FirstOrDefaultAsync(j => j.JobId == jobId.Value);
                        var processStep = await _context.ProcessSteps.FirstOrDefaultAsync(p => p.ProcessStepId == processStepId.Value);
                        var user = await _context.applicationUsers.FirstOrDefaultAsync(u => u.Id == userId);

                        QrScanResultViewModel cardData = new QrScanResultViewModel
                        {
                            JobNumber = job?.JobNumber ?? "N/A",
                            ProcessStep = processStep?.StepName ?? "N/A",
                            Status = "Completed",
                            ScannedBy = (user?.FirstName + " " + user?.LastName)?.Trim() ?? "N/A"
                        };
                        ViewBag.CardData = cardData;
                        ViewBag.ShowCard = true;


                        success = $"JobProcessStage created for JobId {jobId} and ProcessStepId {processStepId}.";
                    }
                }
            }
            if (success != null)
                TempData["success"] = success;
            if (error != null)
                TempData["error"] = error;

            //return RedirectToAction("Index");

            ViewBag.JobId = jobId;
            ViewBag.Error = error;
            ViewBag.Success = success;
            ViewBag.ScannedData = qrData;
            return View("Index");
        }


    }
}
