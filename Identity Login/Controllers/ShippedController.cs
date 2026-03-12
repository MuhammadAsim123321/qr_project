using Identity_Login.Data;
using Identity_Login.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity_Login.Controllers
{
    public class ShippedController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RazorViewToStringRenderer _renderer;
        public IActionResult Index()
        {
            return View();
        }

        public ShippedController(ApplicationDbContext context)
        {
            _context = context;
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
                    j.VerbalNo, // PO Number
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



    }
}
