using Identity_Login.Data;
using Identity_Login.Models.ViewModels;
using Identity_Login.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity_Login.Controllers
{
    public class ShippedController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobStorageService _blobStorageService;

        public ShippedController(ApplicationDbContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRouterJobsData()
        {
            Console.WriteLine("=== GetRouterJobsData called ===");

            var jobs = await _context.RouterJobs
                .Include(j => j.Classification)
                .Include(j => j.RunType)
                .Include(j => j.ASF)
                .Include(j => j.Materail)
                .Select(j => new {
                    j.JobId,
                    j.JobNumber,
                    j.CustomerName,
                    j.JobDetails,
                    j.QrCodeData,
                    j.PdfFilePath,
                    j.VerbalNo,
                    j.Quantity,
                    j.DrawingNo,
                    j.CreatedOn,
                    j.UpdatedOn,
                    j.ProcessId,
                    ClassificationName = j.Classification != null ? j.Classification.Name : "",
                    RunTypeName = j.RunType != null ? j.RunType.Name : "",
                    ASFName = j.ASF != null ? j.ASF.Name : "",
                    MaterailName = j.Materail != null ? j.Materail.Name : "",
                    Status = j.Status.ToString(),
                    LatestStage = _context.JobProcessStages
                        .Where(s => s.JobId == j.JobId)
                        .OrderByDescending(s => s.CreatedOn)
                        .Select(s => new {
                            ProcessStep = s.ProcessStep.StepName,
                            StageStatus = s.StageStatus.ToString()
                        })
                    .FirstOrDefault()
                })
                .OrderByDescending(j => j.CreatedOn)
                .ToListAsync();

            var result = jobs
                .Where(j => (j.LatestStage?.ProcessStep ?? "")
                    .Trim()
                    .Equals("Shipped", StringComparison.OrdinalIgnoreCase))
                .Select(j => new {
                    j.JobId,
                    j.JobNumber,
                    j.CustomerName,
                    j.JobDetails,
                    j.QrCodeData,
                    j.PdfFilePath,
                    j.VerbalNo,
                    j.Quantity,
                    j.DrawingNo,
                    j.CreatedOn,
                    j.UpdatedOn,
                    ClassificationName = j.ClassificationName,
                    RunTypeName = j.RunTypeName,
                    ASFName = j.ASFName,
                    MaterailName = j.MaterailName,
                    j.Status,
                    ProcessStep = j.LatestStage?.ProcessStep ?? "N/A",
                    StageStatus = j.LatestStage?.StageStatus ?? "N/A",
                    UpdatedOnDisplay = (j.UpdatedOn ?? j.CreatedOn)?.ToString("MM-dd-yyyy")
                });

            return Json(new { data = result });
        }

        // ✅ NEW: Details action for shipped jobs
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var job = await _context.RouterJobs
                .AsNoTracking()
                .Include(j => j.Classification)
                .Include(j => j.RunType)
                .Include(j => j.ASF)
                .Include(j => j.Materail)
                .Include(j => j.JobProcess)
                .Include(j => j.UploadImages)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null)
                return NotFound();

            // ✅ Verify that this job has "Shipped" status
            var latestStage = await _context.JobProcessStages
                .Where(s => s.JobId == id)
                .OrderByDescending(s => s.CreatedOn)
                .Select(s => s.ProcessStep.StepName)
                .FirstOrDefaultAsync();

            if (latestStage == null || !latestStage.Trim().Equals("Shipped", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index"); // Redirect if not shipped

            // ✅ Pass blob URL instead of base64
            string qrBlobUrl = job.PdfFilePath;

            var model = new RouterjobPdfVM
            {
                JobId = job.JobId,
                JobNumber = job.JobNumber,
                CustomerName = job.CustomerName,
                PartName = job.PartName,
                DrawingNo = job.DrawingNo,
                Date = job.Date,
                VerbalNo = job.VerbalNo,
                Quantity = job.Quantity,
                RCVDBy = job.RCVDBy,
                ShippedBy = job.ShippedBy,
                JobDetails = job.JobDetails,
                JobProcessName = job.JobProcess?.Name,
                Status = job.Status.ToString(),
                ClassificationName = job.Classification?.Name,
                RunTypeName = job.RunType?.Name,
                ASFName = job.ASF?.Name,
                MaterailName = job.Materail?.Name,
                QrCodeBase64 = qrBlobUrl,
                UploadImages = job.UploadImages?.ToList()
            };

            return View(model);
        }
    }
}