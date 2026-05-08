using Identity_Login.Data;
using Identity_Login.Models.dbModels;
using Identity_Login.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Identity_Login.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var job = await _context.RouterJobs
                .Include(j => j.Classification)
                .Include(j => j.RunType)
                .Include(j => j.ASF)
                .Include(j => j.Materail)
                .Include(j => j.JobProcess)
                .Include(j => j.UploadImages)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null)
                return NotFound();

            string qrBase64 = null;
            if (!string.IsNullOrEmpty(job.PdfFilePath) && System.IO.File.Exists(job.PdfFilePath))
            {
                var bytes = await System.IO.File.ReadAllBytesAsync(job.PdfFilePath);
                qrBase64 = "data:image/png;base64," + Convert.ToBase64String(bytes);
            }
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
                QrCodeBase64 = qrBase64,
                //ImageBase64 = imageBase64,
                UploadImages = job.UploadImages?.ToList()

            };

            return View(model);
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetResults(string filter, string value)
        {
            // 1. Basic validation to prevent unnecessary database hits
            if (string.IsNullOrEmpty(filter) || string.IsNullOrEmpty(value))
            {
                return Json(new { data = new List<RouterJobVm>() });
            }

            try
            {
                // 2. Query with Logic:
                // - Filter by Customer and Drawing Number
                // - Exclude jobs ONLY IF they have a "Shipped" stage record 
                //   where StageStatus is 1 (Completed) AND DisappearAfterShipped is true.
                var job = _context.RouterJobs //Tunned
                    .Include(j => j.JobProcess)
                    .Include(j => j.Classification)
                    .Include(j => j.Materail)
                    .Include(j => j.RunType)
                    .Include(j => j.ASF)
                    .Where(j => j.CustomerName.ToLower() == filter.ToLower() &&
                                j.DrawingNo.ToLower() == value.ToLower() &&
                                j.IsDeleted == false)
                    .Where(j => !(
                        _context.JobProcessStages.Any(stage =>
                            stage.JobId == j.JobId &&
                            (int)stage.StageStatus == 1 && // Cast Enum to int to avoid operator error
                            _context.ProcessSteps.Any(step =>
                                step.ProcessStepId == stage.ProcessStepId &&
                                step.StepName == "Shipped")
                        )
                        && j.DisappearAfterShipped == true
                    ))
                    .OrderByDescending(j => j.JobId)
                    .FirstOrDefault(); //Tunned

                // 3. Map to ViewModel (In-Memory)
                if (job == null) //Tunned
                {
                    return Json(new { data = new List<RouterJobVm>() }); //Tunned
                }

                var data = new List<RouterJobVm> //Tunned
    {
        new RouterJobVm
        {
            JobId = job.JobId,
            CustomerName = job.CustomerName,
            PartName = job.PartName,
            DrawingNo = job.DrawingNo,
            VerbalNo = job.VerbalNo,
            Quantity = job.Quantity,
            ProcessName = job.JobProcess?.Name ?? "N/A",
            ProcessStep = job.JobProcess?.Name ?? "N/A",
            ClassificationName = job.Classification?.Name ?? "N/A",
            MaterialName = job.Materail?.Name ?? "N/A",
            RunTypeName = job.RunType?.Name ?? "N/A",
            AsfName = job.ASF?.Name ?? "N/A",
            UpdatedOnDisplay = (job.UpdatedOn ?? job.CreatedOn)?.ToString("MM-dd-yyyy") ?? "N/A"
        }
    };

                return Json(new { data });
            }
            catch (Exception ex)
            {
                // Log the error (optional) and return an empty list or error message
                return Json(new { data = new List<RouterJobVm>(), error = "Internal Server Error" });
            }
        }



        private List<RouterJobVm> GetRouterJobsByProcess(int id)
        {
            return (from r in _context.RouterJobs
                    join p in _context.JobProcesses on r.ProcessId equals p.ProcessId
                    where r.ProcessId == id
                    select new RouterJobVm
                    {
                        JobId = r.JobId,
                        CustomerName = r.CustomerName,
                        PartName = r.PartName,
                        ProcessName = p.Name,
                        ClassificationName = r.ClassificationId != null
                            ? _context.classifications
                                .Where(c => c.ClassificationId == r.ClassificationId)
                                .Select(c => c.Name)
                                .FirstOrDefault()
                            : null,
                        MaterialName = r.MaterailId != null
                            ? _context.Materails
                                .Where(m => m.MaterailId == r.MaterailId)
                                .Select(m => m.Name)
                                .FirstOrDefault()
                            : null,
                        RunTypeName = r.RunTypeId != null
                            ? _context.RunTypes
                                .Where(rt => rt.RunTypeId == r.RunTypeId)
                                .Select(rt => rt.Name)
                                .FirstOrDefault()
                            : null,
                        AsfName = r.ASFId != null
                            ? _context.ASFs
                                .Where(a => a.ASFId == r.ASFId)
                                .Select(a => a.Name)
                                .FirstOrDefault()
                            : null
                    })
              .OrderBy(x => x.PartName)
              .ToList();
        }
        private List<RouterJobVm> GetRouterJobsByClasssificationId(int id)
        {
            return (from r in _context.RouterJobs
                    join p in _context.JobProcesses on r.ProcessId equals p.ProcessId
                    where r.ClassificationId == id
                    select new RouterJobVm
                    {
                        JobId = r.JobId,
                        CustomerName = r.CustomerName,
                        PartName = r.PartName,
                        ProcessName = p.Name,
                        ClassificationName = r.ClassificationId != null
                            ? _context.classifications
                                .Where(c => c.ClassificationId == r.ClassificationId)
                                .Select(c => c.Name)
                                .FirstOrDefault()
                            : null,
                        MaterialName = r.MaterailId != null
                            ? _context.Materails
                                .Where(m => m.MaterailId == r.MaterailId)
                                .Select(m => m.Name)
                                .FirstOrDefault()
                            : null,
                        RunTypeName = r.RunTypeId != null
                            ? _context.RunTypes
                                .Where(rt => rt.RunTypeId == r.RunTypeId)
                                .Select(rt => rt.Name)
                                .FirstOrDefault()
                            : null,
                        AsfName = r.ASFId != null
                            ? _context.ASFs
                                .Where(a => a.ASFId == r.ASFId)
                                .Select(a => a.Name)
                                .FirstOrDefault()
                            : null
                    })
              .OrderBy(x => x.PartName)
              .ToList();
        }
        private List<RouterJobVm> GetRouterJobsByMaterialId(int id)
        {
            return (from r in _context.RouterJobs
                    join p in _context.JobProcesses on r.ProcessId equals p.ProcessId
                    where r.MaterailId == id
                    select new RouterJobVm
                    {
                        JobId = r.JobId,
                        CustomerName = r.CustomerName,
                        PartName = r.PartName,
                        ProcessName = p.Name,
                        ClassificationName = r.ClassificationId != null
                            ? _context.classifications
                                .Where(c => c.ClassificationId == r.ClassificationId)
                                .Select(c => c.Name)
                                .FirstOrDefault()
                            : null,
                        MaterialName = r.MaterailId != null
                            ? _context.Materails
                                .Where(m => m.MaterailId == r.MaterailId)
                                .Select(m => m.Name)
                                .FirstOrDefault()
                            : null,
                        RunTypeName = r.RunTypeId != null
                            ? _context.RunTypes
                                .Where(rt => rt.RunTypeId == r.RunTypeId)
                                .Select(rt => rt.Name)
                                .FirstOrDefault()
                            : null,
                        AsfName = r.ASFId != null
                            ? _context.ASFs
                                .Where(a => a.ASFId == r.ASFId)
                                .Select(a => a.Name)
                                .FirstOrDefault()
                            : null
                    })
              .OrderBy(x => x.PartName)
              .ToList();
        }
        private List<RouterJobVm> GetRouterJobsByRunType(int id)
        {
            return (from r in _context.RouterJobs
                    join p in _context.JobProcesses on r.ProcessId equals p.ProcessId
                    where r.RunTypeId == id
                    select new RouterJobVm
                    {
                        JobId = r.JobId,
                        CustomerName = r.CustomerName,
                        PartName = r.PartName,
                        ProcessName = p.Name,
                        ClassificationName = r.ClassificationId != null
                            ? _context.classifications
                                .Where(c => c.ClassificationId == r.ClassificationId)
                                .Select(c => c.Name)
                                .FirstOrDefault()
                            : null,
                        MaterialName = r.MaterailId != null
                            ? _context.Materails
                                .Where(m => m.MaterailId == r.MaterailId)
                                .Select(m => m.Name)
                                .FirstOrDefault()
                            : null,
                        RunTypeName = r.RunTypeId != null
                            ? _context.RunTypes
                                .Where(rt => rt.RunTypeId == r.RunTypeId)
                                .Select(rt => rt.Name)
                                .FirstOrDefault()
                            : null,
                        AsfName = r.ASFId != null
                            ? _context.ASFs
                                .Where(a => a.ASFId == r.ASFId)
                                .Select(a => a.Name)
                                .FirstOrDefault()
                            : null
                    })
              .OrderBy(x => x.PartName)
              .ToList();
        }
        private List<RouterJobVm> GetRouterJobsByAsf(int id)
        {
            return (from r in _context.RouterJobs
                    join p in _context.JobProcesses on r.ProcessId equals p.ProcessId
                    where r.ASFId == id
                    select new RouterJobVm
                    {
                        JobId = r.JobId,
                        CustomerName = r.CustomerName,
                        PartName = r.PartName,
                        ProcessName = p.Name,
                        ClassificationName = r.ClassificationId != null
                            ? _context.classifications
                                .Where(c => c.ClassificationId == r.ClassificationId)
                                .Select(c => c.Name)
                                .FirstOrDefault()
                            : null,
                        MaterialName = r.MaterailId != null
                            ? _context.Materails
                                .Where(m => m.MaterailId == r.MaterailId)
                                .Select(m => m.Name)
                                .FirstOrDefault()
                            : null,
                        RunTypeName = r.RunTypeId != null
                            ? _context.RunTypes
                                .Where(rt => rt.RunTypeId == r.RunTypeId)
                                .Select(rt => rt.Name)
                                .FirstOrDefault()
                            : null,
                        AsfName = r.ASFId != null
                            ? _context.ASFs
                                .Where(a => a.ASFId == r.ASFId)
                                .Select(a => a.Name)
                                .FirstOrDefault()
                            : null
                    })
              .OrderBy(x => x.PartName)
              .ToList();
        }

        private List<RouterJobVm> GetRouterJobsByNameAndDrawingNo(string name, string drawingNo)
        {
            //var jobs = (from r in _context.RouterJobs
            //            join p in _context.JobProcesses on r.ProcessId equals p.ProcessId
            //            where r.DrawingNo.ToLower() == drawingNo.ToLower()
            //               && r.CustomerName.ToLower() == name.ToLower()
            //               //&& !(r.ProcessId == 4 && latestStage.StageStatus.ToString() == "Shipped")
            //            select new
            //            {
            //                r.JobId,
            //                r.CustomerName,
            //                r.PartName,
            //                ProcessName = p.Name,
            //                r.VerbalNo,
            //                r.Quantity,
            //                r.DrawingNo,
            //                r.CreatedOn,
            //                r.UpdatedOn,
            //                ClassificationName = r.ClassificationId != null
            //                    ? _context.classifications
            //                        .Where(c => c.ClassificationId == r.ClassificationId)
            //                        .Select(c => c.Name)
            //                        .FirstOrDefault()
            //                    : null,
            //                MaterialName = r.MaterailId != null
            //                    ? _context.Materails
            //                        .Where(m => m.MaterailId == r.MaterailId)
            //                        .Select(m => m.Name)
            //                        .FirstOrDefault()
            //                    : null,
            //                RunTypeName = r.RunTypeId != null
            //                    ? _context.RunTypes
            //                        .Where(rt => rt.RunTypeId == r.RunTypeId)
            //                        .Select(rt => rt.Name)
            //                        .FirstOrDefault()
            //                    : null,
            //                AsfName = r.ASFId != null
            //                    ? _context.ASFs
            //                        .Where(a => a.ASFId == r.ASFId)
            //                        .Select(a => a.Name)
            //                        .FirstOrDefault()
            //                    : null
            //            })
            //           .OrderBy(x => x.PartName)
            //           .ToList();

            var jobs = (from r in _context.RouterJobs
                        join s in _context.JobProcessStages on r.JobId equals s.JobId into stageGroup
                        from latestStage in stageGroup
                            .OrderByDescending(x => x.CreatedOn)
                            .Take(1)
                            .DefaultIfEmpty()
                        join p in _context.JobProcesses on r.ProcessId equals p.ProcessId
                        where r.DrawingNo.ToLower() == drawingNo.ToLower()
                           && r.CustomerName.ToLower() == name.ToLower()
                            && !(r.ProcessId == 4 && latestStage != null && latestStage.StageStatus == Enums.ProcessStageStatus.Shipped)
                        select new
                        {
                            r.JobId,
                            r.CustomerName,
                            r.PartName,
                            ProcessName = p.Name,
                            r.VerbalNo,
                            r.Quantity,
                            r.DrawingNo,
                            r.CreatedOn,
                            r.UpdatedOn,
                            ClassificationName = r.ClassificationId != null
                                ? _context.classifications
                                    .Where(c => c.ClassificationId == r.ClassificationId)
                                    .Select(c => c.Name)
                                    .FirstOrDefault()
                                : null,
                            MaterialName = r.MaterailId != null
                                ? _context.Materails
                                    .Where(m => m.MaterailId == r.MaterailId)
                                    .Select(m => m.Name)
                                    .FirstOrDefault()
                                : null,
                            RunTypeName = r.RunTypeId != null
                                ? _context.RunTypes
                                    .Where(rt => rt.RunTypeId == r.RunTypeId)
                                    .Select(rt => rt.Name)
                                    .FirstOrDefault()
                                : null,
                            AsfName = r.ASFId != null
                                ? _context.ASFs
                                    .Where(a => a.ASFId == r.ASFId)
                                    .Select(a => a.Name)
                                    .FirstOrDefault()
                                : null
                        })
                        .OrderBy(x => x.PartName)
                        .ToList();

            return jobs.Select(j => new RouterJobVm
            {
                JobId = j.JobId,
                CustomerName = j.CustomerName,
                PartName = j.PartName,
                ProcessName = j.ProcessName,
                VerbalNo = j.VerbalNo,
                Quantity = j.Quantity,
                DrawingNo = j.DrawingNo,
                ProcessStep = j.ProcessName,
                ClassificationName = j.ClassificationName,
                MaterialName = j.MaterialName,
                RunTypeName = j.RunTypeName,
                AsfName = j.AsfName,
                UpdatedOnDisplay = ((j.UpdatedOn ?? j.CreatedOn)?.ToString("MM-dd-yyyy"))
            }).ToList();
        }


        private List<RouterJobVm> GetRouterJobsByName(string id)
        {
            return (from r in _context.RouterJobs
                    join p in _context.JobProcesses on r.ProcessId equals p.ProcessId
                    where r.DrawingNo.ToLower() == id
                    select new RouterJobVm
                    {
                        JobId = r.JobId,
                        CustomerName = r.CustomerName,
                        PartName = r.PartName,
                        ProcessName = p.Name,
                        ClassificationName = r.ClassificationId != null
                            ? _context.classifications
                                .Where(c => c.ClassificationId == r.ClassificationId)
                                .Select(c => c.Name)
                                .FirstOrDefault()
                            : null,
                        MaterialName = r.MaterailId != null
                            ? _context.Materails
                                .Where(m => m.MaterailId == r.MaterailId)
                                .Select(m => m.Name)
                                .FirstOrDefault()
                            : null,
                        RunTypeName = r.RunTypeId != null
                            ? _context.RunTypes
                                .Where(rt => rt.RunTypeId == r.RunTypeId)
                                .Select(rt => rt.Name)
                                .FirstOrDefault()
                            : null,
                        AsfName = r.ASFId != null
                            ? _context.ASFs
                                .Where(a => a.ASFId == r.ASFId)
                                .Select(a => a.Name)
                                .FirstOrDefault()
                            : null
                    })
              .OrderBy(x => x.PartName)
              .ToList();
        }
        private List<RouterJobVm> GetRouterJobsByPartName(string id)
        {
            return (from r in _context.RouterJobs
                    join p in _context.JobProcesses on r.ProcessId equals p.ProcessId
                    where r.PartName.ToLower() == id
                    select new RouterJobVm
                    {
                        JobId = r.JobId,
                        CustomerName = r.CustomerName,
                        PartName = r.PartName,
                        ProcessName = p.Name,
                        ClassificationName = r.ClassificationId != null
                            ? _context.classifications
                                .Where(c => c.ClassificationId == r.ClassificationId)
                                .Select(c => c.Name)
                                .FirstOrDefault()
                            : null,
                        MaterialName = r.MaterailId != null
                            ? _context.Materails
                                .Where(m => m.MaterailId == r.MaterailId)
                                .Select(m => m.Name)
                                .FirstOrDefault()
                            : null,
                        RunTypeName = r.RunTypeId != null
                            ? _context.RunTypes
                                .Where(rt => rt.RunTypeId == r.RunTypeId)
                                .Select(rt => rt.Name)
                                .FirstOrDefault()
                            : null,
                        AsfName = r.ASFId != null
                            ? _context.ASFs
                                .Where(a => a.ASFId == r.ASFId)
                                .Select(a => a.Name)
                                .FirstOrDefault()
                            : null
                    })
              .OrderBy(x => x.PartName)
              .ToList();
        }


        [HttpGet]
        public IActionResult GetDistinctValues(string filter)
        {
            List<object> data = _context.RouterJobs
                    .Where(r => r.CustomerName.ToLower() == filter.ToLower())
                    .Select(c => new { value = c.DrawingNo, text = c.DrawingNo })
                    .Distinct()
                    .OrderBy(x => x.text)
                    .ToList<object>();

            return Json(data);
            //if (filter == "Process")
            //{
            //    data = (from r in _context.RouterJobs
            //            join p in _context.JobProcesses on r.ProcessId equals p.ProcessId
            //            select new
            //            {
            //                value = p.ProcessId,
            //                text = p.Name
            //            })
            //            .Distinct()
            //            .OrderBy(x => x.text)
            //            .ToList<object>();
            //}
            //else if (filter == "Customer")
            //{
            //    data = _context.RouterJobs
            //        .Select(c => new { value = c.DrawingNo, text = c.DrawingNo })
            //        .Distinct()
            //        .OrderBy(x => x.text)
            //        .ToList<object>();
            //}
            //else if (filter == "Part Name")
            //{
            //    data = _context.RouterJobs
            //        .Select(c => new { value = c.PartName, text = c.PartName })
            //        .Distinct()
            //        .OrderBy(x => x.text)
            //        .ToList<object>();
            //}
            //else if (filter == "Classification")
            //{
            //    data = (from r in _context.RouterJobs
            //            join p in _context.classifications on r.ClassificationId equals p.ClassificationId
            //            select new
            //            {
            //                value = p.ClassificationId,
            //                text = p.Name
            //            })
            //            .Distinct()
            //            .OrderBy(x => x.text)
            //            .ToList<object>();
            //}
            //else if (filter == "Material")
            //{
            //    data = (from r in _context.RouterJobs
            //            join p in _context.Materails on r.MaterailId equals p.MaterailId
            //            select new
            //            {
            //                value = p.MaterailId,
            //                text = p.Name
            //            })
            //            .Distinct()
            //            .OrderBy(x => x.text)
            //            .ToList<object>();
            //}
            //else if (filter == "Run Type")
            //{
            //    data = (from r in _context.RouterJobs
            //            join p in _context.RunTypes on r.RunTypeId equals p.RunTypeId
            //            select new
            //            {
            //                value = p.RunTypeId,
            //                text = p.Name
            //            })
            //            .Distinct()
            //            .OrderBy(x => x.text)
            //            .ToList<object>();
            //}
            //else if (filter == "ASF")
            //{
            //    data = (from r in _context.RouterJobs
            //            join p in _context.ASFs on r.ASFId equals p.ASFId
            //            select new
            //            {
            //                value = p.ASFId,
            //                text = p.Name
            //            })
            //            .Distinct()
            //            .OrderBy(x => x.text)
            //            .ToList<object>();
            //}


            //return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> CreateBucket(int id)
        {
            var job = await _context.RouterJobs
                .Include(j => j.UploadImages)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null) return NotFound();

            var vm = new RouterJobEditVm
            {
                JobId = job.JobId,
                CustomerName = job.CustomerName,
                JobDetails = job.JobDetails,
                PartName = job.PartName,
                DrawingNo = job.DrawingNo,
                Date = job.Date,
                VerbalNo = job.VerbalNo,
                SurfaceArea = job.SurfaceArea,
                Quantity = job.Quantity,
                RCVDBy = job.RCVDBy,
                ShippedBy = job.ShippedBy,
                ClassificationId = job.ClassificationId,
                RunTypeId = job.RunTypeId,
                ASFId = job.ASFId,
                MaterailId = job.MaterailId,
                ProcessId = job.ProcessId,
                ImagePath = job.ImagePath,
                TotalIn2OfRunRight = job.TotalIn2OfRunRight
            };

            ViewBag.ClassificationList = new SelectList(_context.classifications, "ClassificationId", "Name", vm.ClassificationId);
            ViewBag.RunTypeList = new SelectList(_context.RunTypes, "RunTypeId", "Name", vm.RunTypeId);
            ViewBag.ASFList = new SelectList(_context.ASFs, "ASFId", "Name", vm.ASFId);
            ViewBag.MaterailList = new SelectList(_context.Materails, "MaterailId", "Name", vm.MaterailId);
            ViewBag.JobProcessList = new SelectList(_context.JobProcesses, "ProcessId", "Name", vm.ProcessId);

            return View("CreateBucket", vm); // specify view name
        }



        //Functions For Create_Job

        private async Task CompleteFirstProcessStepAsync(RouterJob job)
        {
            if (job.ProcessId.HasValue)
            {
                var firstStep = await _context.ProcessSteps
                    .Where(ps => ps.ProcessId == job.ProcessId)
                    .OrderBy(ps => ps.StepOrder)
                    .FirstOrDefaultAsync();

                if (firstStep != null)
                {
                    var processStage = new JobProcessStage
                    {
                        JobId = job.JobId,
                        ProcessStepId = firstStep.ProcessStepId,
                        StageStatus = Enums.ProcessStageStatus.Completed,
                        CompletedOn = DateTime.UtcNow,
                        CreatedOn = DateTime.UtcNow
                    };
                    _context.JobProcessStages.Add(processStage);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // Generate a unique job number
        private string GenerateUniqueJobNumber()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string jobNumber;
            do
            {
                jobNumber = new string(Enumerable.Repeat(chars, 10)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (_context.RouterJobs.Any(j => j.JobNumber == jobNumber));
            return jobNumber;
        }


        private async Task SaveJobImagesAsync(RouterJob job, List<IFormFile>? imageFiles)
        {
            if (imageFiles != null && imageFiles.Count > 0)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "jobimages");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                foreach (var file in imageFiles)
                {
                    if (file.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadFolder, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        var uploadImage = new UploadImage
                        {
                            ImagePath = "/jobimages/" + uniqueFileName,
                            RouterJobId = job.JobId
                        };
                        _context.Add(uploadImage);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        private class QrCodeResult
        {
            public string Message { get; set; }
            public string SavedPath { get; set; }
            public string ImageBase64 { get; set; }
        }


        private async Task GenerateAndSaveQrCodeAsync(RouterJob job)
        {
            var jobVm = new RouterJobVm
            {
                JobId = job.JobId,
                JobNumber = job.JobNumber,
                CustomerName = job.CustomerName,
                JobDetails = job.JobDetails,
                PdfFilePath = ""
            };

            using (var httpClient = new HttpClient())
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var response = await httpClient.PostAsJsonAsync($"{baseUrl}/QRCode/Generate", jobVm);

                if (response.IsSuccessStatusCode)
                {
                    var qrResult = await response.Content.ReadFromJsonAsync<QrCodeResult>();
                    if (qrResult != null && !string.IsNullOrEmpty(qrResult.SavedPath))
                    {
                        job.PdfFilePath = qrResult.SavedPath;
                        _context.Update(job);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBucket([Bind("CustomerName,JobDetails,PartName,DrawingNo,SurfaceArea,Date,VerbalNo,Quantity,Materail,RCVDBy,ShippedBy,ClassificationId,RunTypeId,ASFId,MaterailId,ProcessId,TotalIn2OfRunRight,DisappearAfterShipped")] RouterJob routerJob, List<IFormFile>? ImageFiles, int? sourceJobId, string? existingImages)
        {
            ModelState.Remove(nameof(routerJob.JobNumber));
            ModelState.Remove(nameof(routerJob.Status));
            ModelState.Remove(nameof(routerJob.PdfFilePath));
            ModelState.Remove(nameof(routerJob.QrCodeData));
            if (ModelState.IsValid)
            {
                try
                {

                    routerJob.JobNumber = GenerateUniqueJobNumber();
                    routerJob.CreatedOn = DateTime.UtcNow;
                    routerJob.Status = Enums.JobStatus.Pending;
                    routerJob.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);


                    _context.Add(routerJob);
                    await _context.SaveChangesAsync();

                    var keptExistingIds = (existingImages ?? string.Empty)
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => int.TryParse(id, out var parsed) ? parsed : 0)
                        .Where(id => id > 0)
                        .ToList();

                    if (sourceJobId.HasValue && keptExistingIds.Any())
                    {
                        var oldImages = await _context.Set<UploadImage>()
                            .Where(u => u.RouterJobId == sourceJobId.Value && keptExistingIds.Contains(u.UploadImageId))
                            .ToListAsync();

                        foreach (var img in oldImages)
                        {
                            var newImage = new UploadImage
                            {
                                ImagePath = img.ImagePath,
                                RouterJobId = routerJob.JobId
                            };
                            _context.Add(newImage);
                        }
                        await _context.SaveChangesAsync();
                    }

                    if (ImageFiles != null && ImageFiles.Count > 0)
                    {
                        await SaveJobImagesAsync(routerJob, ImageFiles);
                    }
                    else if (sourceJobId.HasValue && !keptExistingIds.Any())
                    {
                        // No kept existing images, so don't copy any old images.
                    }

                    await CompleteFirstProcessStepAsync(routerJob);
                    //await SaveJobImagesAsync(routerJob, ImageFiles);
                    await GenerateAndSaveQrCodeAsync(routerJob);

                    TempData["success"] = "Job created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Error creating Router Job: " + ex.Message;
                }
            }
            else
            {
                TempData["error"] = "Please correct the errors and try again.";
            }
            return View("CreateBucket", new RouterJobEditVm
            {
                JobId = routerJob.JobId,
                CustomerName = routerJob.CustomerName,
                JobDetails = routerJob.JobDetails,
                PartName = routerJob.PartName,
                DrawingNo = routerJob.DrawingNo,
                Date = routerJob.Date,
                VerbalNo = routerJob.VerbalNo,
                SurfaceArea = routerJob.SurfaceArea,
                Quantity = routerJob.Quantity,
                RCVDBy = routerJob.RCVDBy,
                ShippedBy = routerJob.ShippedBy,
                ClassificationId = routerJob.ClassificationId,
                RunTypeId = routerJob.RunTypeId,
                ASFId = routerJob.ASFId,
                MaterailId = routerJob.MaterailId,
                ProcessId = routerJob.ProcessId,
                ImagePath = routerJob.ImagePath,
                TotalIn2OfRunRight = routerJob.TotalIn2OfRunRight,
                DisappearAfterShipped = routerJob.DisappearAfterShipped
            });

        }





    }
}