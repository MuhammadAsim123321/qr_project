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
using Identity_Login.Services;
using Azure.Core;
using Identity_Login.Models.ViewModels;
using System.Net.Http;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF.Previewer;
using System.Buffers.Text;
using System.Diagnostics;
using System.Text.Json;

namespace Identity_Login.Controllers
{
    public class RouterJobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PdfService _pdfService;
        private readonly RazorViewToStringRenderer _renderer;

        //private readonly QrCodeService _qrCodeService;

        public RouterJobsController(ApplicationDbContext context, PdfService pdfService, RazorViewToStringRenderer renderer)
        {
            _context = context;
            _pdfService = pdfService;
            _renderer = renderer;
            //_qrCodeService = qrCodeService;
        }


        [HttpGet]
        public IActionResult GetClassifications(int asfId, int? processId)
        {
            //show only the specific classification
            if (processId == 2)
            {
                var result = _context.classifications
                    .Select(c => new { c.ClassificationId, c.Name, c.Minutes }) 
                    .ToList();
                return Json(result);
            }
            else if (processId == 3)
            {
                var result = _context.classifications
                    .Select(c => new { c.ClassificationId, c.Name, c.Minutes }) 
                    .ToList();
                return Json(result);
            }
            else if (processId == 5)
            {
                var result = _context.classifications
                    .Where(c => c.ClassificationId >= 28 && c.ClassificationId <= 31)
                    .Select(c => new { c.ClassificationId, c.Name, c.Minutes }) 
                    .ToList();
                return Json(result);
            }

            // Existing ASF logic
            var asf12 = _context.ASFs.FirstOrDefault(a => a.Name.Contains("12 ASF"));
            var asf16 = _context.ASFs.FirstOrDefault(a => a.Name.Contains("16 ASF"));

            var asf12Classifications = new List<string>
            {
            "Type I, Class 1 (CLEAR)",
            "Type II, Class 1 (CLEAR)",
            "Type II, Class 2 (BLACK)",
            "Type II, Class 2 (BLUE-A)",
            "Type II, Class 2 (BORDEAUX RED)",
            "Type II, Class 2 (CAMO BROWN)",
            "Type II, Class 2 (DARK BLUE)",
            "Type II, Class 2 (GOLD S)",
            "Type II, Class 2 (GREEN AEN)",
            "Type II, Class 2 (GREY)",
            "Type II, Class 2 (LANTZ MEDICAL BLUE)",
            "Type II, Class 2 (NEON PINK)",
            "Type II, Class 2 (OLIVE DRAB)",
            "Type II, Class 2 (ORANGE 2B)",
            "Type II, Class 2 (TEAL)",
            "Type II, Class 2 (VIOLET 3D)",
            "Type II, Class 2 (YELLOW 4A)",
            "Type III, Class 1 (CLEAR)",
            "Type III, Class 1 (CLEAR) W/ PTFE TEFLON",
            "Type III, Class 2 (BLACK)"
           };

            var asf16Classifications = new List<string>
            {
                "Type III, Class 1 (CLEAR)",
                "Type III, Class 1 (CLEAR) W/ PTFE TEFLON",
                "Type III, Class 2 (BLACK)"
            };

            List<Classification> filtered = new List<Classification>();
            if (asf12 != null && asfId == asf12.ASFId)
            {
                filtered = _context.classifications.Where(c => asf12Classifications.Contains(c.Name)).ToList();
               
            }
            else if (asf16 != null && asfId == asf16.ASFId)
            {
                filtered = _context.classifications.Where(c => asf16Classifications.Contains(c.Name)).ToList();
            }
            else
            {
                filtered = _context.classifications
                    .Where(c => c.ClassificationId != 23 && c.ClassificationId != 24)
                    .ToList();
            }

            // ✅ Include Minutes in response
            return Json(filtered.Select(c => new { c.ClassificationId, c.Name, c.Minutes }));
        }
    
        public async Task<IActionResult> PreviewRouterJob(int id)
        {
            var job = await _context.RouterJobs
            .Include(j => j.Classification)
            .Include(j => j.RunType)
            .Include(j => j.ASF)
            .Include(j => j.Materail)
            .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null) return NotFound();

            string qrBase64 = null;
            if (!string.IsNullOrEmpty(job.PdfFilePath) && System.IO.File.Exists(job.PdfFilePath))
            {
                var bytes = await System.IO.File.ReadAllBytesAsync(job.PdfFilePath);
                qrBase64 = "data:image/png;base64," + Convert.ToBase64String(bytes);
            }

            string imageBase64 = null;
            if (!string.IsNullOrEmpty(job.ImagePath))
            {
                var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", job.ImagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(physicalPath))
                {
                    var bytes = await System.IO.File.ReadAllBytesAsync(physicalPath);
                    imageBase64 = "data:image/png;base64," + Convert.ToBase64String(bytes);
                }
            }

            var totalIn2OfRun = (double)(job.Quantity * (job.SurfaceArea ?? 0));
            double asfBasedConstant = 1.0 / 12.0;
            if (job.ASF != null && job.ASF.Name.Contains("16"))
            {
                asfBasedConstant = 1.0 / 9.0;
            }
            var amps = Math.Round(totalIn2OfRun * asfBasedConstant, 3);
            var ampsPerParts = Math.Round((job.SurfaceArea ?? 0) * asfBasedConstant, 3);


            var model = new RouterjobPdfVM
            {
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
                Status = job.Status.ToString(),
                ClassificationName = job.Classification?.Name,
                RunTypeName = job.RunType?.Name,
                ASFName = job.ASF?.Name,
                MaterailName = job.Materail?.Name,
                QrCodeBase64 = qrBase64,
                ImageBase64 = imageBase64,
                SurfaceArea = job.SurfaceArea,
                TotalIn2OfRun = totalIn2OfRun,
                TimeMinutes = job.Classification?.Minutes ?? 0,
                TimeSeconds = (job.Classification?.Minutes ?? 0) * 60,
                AMPS = amps,
                AMPSPerParts = ampsPerParts

            };

            return View("RouterJobPdf", model);
        }

        // Download action
        public async Task<IActionResult> DownloadRouterJobPdf(int id)
        {
            var job = await GetJobWithDetailsAsync(id);
            if (job == null) return NotFound();

            await CompleteSecondProcessStepIfNeededAsync(job);

            var qrBase64 = await GetQrCodeBase64Async(job);
            var imageBase64List = await GetJobImagesBase64Async(job);

            var totalIn2OfRun = (double)(job.Quantity * (job.SurfaceArea ?? 0));
            var totalIn2OfRunRight = job.TotalIn2OfRunRight;

            double asfBasedConstant = 1.0 / 12.0;
            if (job.ASF != null && job.ASF.Name.Contains("16"))
            {
                asfBasedConstant = 1.0 / 9.0;
            }

            //var asfBasedConstant = 0.08333;
            //if(job.ASF != null && job.ASF.Name.Contains("16"))
            //{
            //    asfBasedConstant = 0.11;
            //}

            var amps = Math.Round((totalIn2OfRunRight ?? 0.0) * asfBasedConstant, 3);
            var ampsPerParts = Math.Round((job.SurfaceArea ?? 0) * asfBasedConstant, 3);

            // Flags for hiding
            bool hideRunTypeAndSurface = false;
            bool hideAmps = false;

            if (job.JobProcess?.Name == "Passivation Process (Method 1)" || job.JobProcess?.Name == "Passivation Process (Method 2)")
            {
                hideRunTypeAndSurface = true;
                hideAmps = true;
            }
            else if (job.JobProcess?.Name == "Chemical Conversion")
            {
                hideAmps = true;
            }
            else if (job.JobProcess?.Name == "Black Oxide Process")
            {
                hideRunTypeAndSurface = true;
                hideAmps = true;
            }

            var model = new RouterjobPdfVM
            {
                JobNumber = job.JobNumber,
                CustomerName = job.CustomerName,
                PartName = job.PartName,
                DrawingNo = job.DrawingNo,
                Date = job.Date,
                VerbalNo = job.VerbalNo,
                Quantity = (int)job.Quantity,
                JobProcessName = (job.JobProcess?.Name == "Passivation Process (Method 1)" || job.JobProcess?.Name == "Passivation Process (Method 2)") ? "Passivation Process" : job.JobProcess?.Name,
                RCVDBy = job.RCVDBy,
                ShippedBy = job.ShippedBy,
                JobDetails = job.JobDetails,
                Status = job.Status.ToString(),
                ClassificationName = job.Classification?.Name,
                RunTypeName = job.RunType?.Name,
                ASFName = job.ASF?.Name,
                MaterailName = job.Materail?.Name,
                QrCodeBase64 = qrBase64,
                SurfaceArea = job.SurfaceArea,
                TotalIn2OfRun = totalIn2OfRun,
                TimeMinutes = job.Classification?.Minutes ?? 0,
                TimeSeconds = (job.Classification?.Minutes ?? 0) * 60,
                AMPS = amps,
                AMPSPerParts = ampsPerParts,
                UploadImages = job.UploadImages?.ToList(),
                UploadImageBase64List = imageBase64List,
                TotalIn2OfRunRight = job.TotalIn2OfRunRight,
                HideRunTypeAndSurface = hideRunTypeAndSurface,
                HideAmps = hideAmps
            };

            string html = await _renderer.RenderViewToStringAsync(this, "RouterJobPdf", model);
            // var pdfBytes = _pdfService.GeneratePdf(html);

            try
            {
                var pdfBytes = await Task.Run(() => _pdfService.GeneratePdf(html));

                return File(pdfBytes, "application/pdf", $"RouterJob_{job.JobNumber}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to generate PDF");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRouterJobsData()
        {
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
                    j.DisappearAfterShipped, // Fetch the flag
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
              .Where(j => j.LatestStage == null || j.LatestStage.ProcessStep != "Shipped")
              .Where(j => !(j.ProcessId == 4 && j.LatestStage?.StageStatus == "Shipped"))
              // ✅ MINIMUM CHANGE: Hide only if both conditions are met
              .Where(j => !(j.DisappearAfterShipped == true && j.LatestStage?.ProcessStep == "Shipped"))
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
                  j.ClassificationName,
                  j.RunTypeName,
                  j.ASFName,
                  j.MaterailName,
                  j.Status,
                  ProcessStep = j.LatestStage?.ProcessStep ?? "N/A",
                  StageStatus = j.LatestStage?.StageStatus ?? "N/A",
                  UpdatedOnDisplay = (j.UpdatedOn ?? j.CreatedOn)?.ToString("MM-dd-yyyy")
              });

            return Json(new { data = result });
        }

        // Get : Images 
        [HttpGet]
        public async Task<IActionResult> GetImages(int jobId)
        {
            var images = await _context.UploadImage
                .Where(u => u.RouterJobId == jobId)
                .Select(u => new { u.UploadImageId, u.ImagePath })
                .ToListAsync();

            return Json(images);
        }


        // GET: RouterJobs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RouterJobs.Include(r => r.CreatedByUser).Include(r => r.UpdatedByUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RouterJobs/Details/5
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

        public IActionResult Create()
        {
            ViewBag.ClassificationListWithMinutes = _context.classifications
                .Select(c => new ClassificationDropdownVm
                {
                    ClassificationId = c.ClassificationId,
                    Name = c.Name,
                    Minutes = c.Minutes
                })
                .ToList();
            ViewBag.RunTypeList = new SelectList(_context.RunTypes.ToList(), "RunTypeId", "Name");
            ViewBag.ASFList = new SelectList(_context.ASFs.ToList(), "ASFId", "Name");
            ViewBag.MaterailList = new SelectList(_context.Materails.ToList(), "MaterailId", "Name");
            ViewBag.JobProcessList = new SelectList(_context.JobProcesses.ToList(), "ProcessId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerName,JobDetails,PartName,DrawingNo,SurfaceArea,Date,VerbalNo,Quantity,Materail,RCVDBy,ShippedBy,ClassificationId,RunTypeId,ASFId,MaterailId,ProcessId,TotalIn2OfRunRight,DisappearAfterShipped")] RouterJob routerJob,
    List<IFormFile>? ImageFiles)
        {
            ModelState.Remove(nameof(routerJob.JobNumber));
            ModelState.Remove(nameof(routerJob.Status));
            ModelState.Remove(nameof(routerJob.PdfFilePath));
            ModelState.Remove(nameof(routerJob.QrCodeData));
            if (ModelState.ContainsKey(nameof(routerJob.DisappearAfterShipped)))
            {
                ModelState.ClearValidationState(nameof(routerJob.DisappearAfterShipped));
                ModelState.MarkFieldValid(nameof(routerJob.DisappearAfterShipped));
            }
            var disappearVal = Request.Form["DisappearAfterShipped"];
            routerJob.DisappearAfterShipped = disappearVal.Contains("true") || disappearVal.Contains("on");
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

                    await CompleteFirstProcessStepAsync(routerJob);
                    await SaveJobImagesAsync(routerJob, ImageFiles);
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
            ViewBag.ClassificationListWithMinutes = _context.classifications
                .Select(c => new ClassificationDropdownVm
                {
                    ClassificationId = c.ClassificationId,
                    Name = c.Name,
                    Minutes = c.Minutes
                })
                .ToList();
            ViewBag.RunTypeList = new SelectList(_context.RunTypes.ToList(), "RunTypeId", "Name");
            ViewBag.ASFList = new SelectList(_context.ASFs.ToList(), "ASFId", "Name");
            ViewBag.MaterailList = new SelectList(_context.Materails.ToList(), "MaterailId", "Name");
            ViewBag.JobProcessList = new SelectList(_context.JobProcesses.ToList(), "ProcessId", "Name");

            return View(routerJob);

        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var job = await _context.RouterJobs
                .Include(j => j.UploadImages)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null)
                return NotFound();

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
                TotalIn2OfRunRight = job.TotalIn2OfRunRight,
                DisappearAfterShipped = job.DisappearAfterShipped,
                // Calculated fields:
                TimeMinutes = job.Classification?.Minutes,
                TimeSeconds = (job.Classification?.Minutes ?? 0) * 60,
                AMPS = (job.TotalIn2OfRunRight ?? 0) * (job.ASF != null && job.ASF.Name.Contains("16") ? (1.0 / 9.0) : (1.0 / 12.0)),
                AMPSPerParts = (job.SurfaceArea ?? 0) * (job.ASF != null && job.ASF.Name.Contains("16") ? (1.0 / 9.0) : (1.0 / 12.0))

            };

            ViewBag.ClassificationListWithMinutes = _context.classifications
                .Select(c => new ClassificationDropdownVm
                {
                    ClassificationId = c.ClassificationId,
                    Name = c.Name,
                    Minutes = c.Minutes
                })
                .ToList();
            ViewBag.RunTypeList = new SelectList(_context.RunTypes.ToList(), "RunTypeId", "Name", vm.RunTypeId);
            ViewBag.ASFList = new SelectList(_context.ASFs.ToList(), "ASFId", "Name", vm.ASFId);
            ViewBag.MaterailList = new SelectList(_context.Materails.ToList(), "MaterailId", "Name", vm.MaterailId);
            ViewBag.JobProcessList = new SelectList(_context.JobProcesses.ToList(), "ProcessId", "Name", vm.ProcessId);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RouterJobEditVm vm , List<IFormFile>? ImageFiles)
        {
            if (id != vm.JobId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {

                    var job = await _context.RouterJobs
                        .Include(j => j.JobProcess)
                        .FirstOrDefaultAsync(j => j.JobId == id);

                    if (job == null)
                        return NotFound();

                    // Update fields
                    job.CustomerName = vm.CustomerName;
                    job.JobDetails = vm.JobDetails;
                    job.PartName = vm.PartName;
                    job.DrawingNo = vm.DrawingNo;
                    job.Date = vm.Date;
                    job.VerbalNo = vm.VerbalNo;
                    job.SurfaceArea = vm.SurfaceArea;
                    job.Quantity = vm.Quantity;
                    job.RCVDBy = vm.RCVDBy;
                    job.ShippedBy = vm.ShippedBy;
                    job.ClassificationId = vm.ClassificationId;
                    job.RunType = vm.RunTypeId.HasValue ? await _context.RunTypes.FindAsync(vm.RunTypeId) : null;
                    job.ASFId = vm.ASFId;
                    job.MaterailId = vm.MaterailId;
                    job.JobProcess = vm.ProcessId.HasValue ? await _context.JobProcesses.FindAsync(vm.ProcessId) : null;
                    job.Status = Enums.JobStatus.Pending;
                    job.UpdatedOn = DateTime.UtcNow;
                    job.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    job.TotalIn2OfRunRight = vm.TotalIn2OfRunRight;
                    job.DisappearAfterShipped = vm.DisappearAfterShipped;

                    // 1️⃣ Get IDs of images the user kept
                    var keptIds = (vm.ExistingImages ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToList();

                    // 2️⃣ Load all existing images from DB
                    var existingImages = await _context.Set<UploadImage>()
                        .Where(u => u.RouterJobId == job.JobId)
                        .ToListAsync();

                    // 3️⃣ Determine which images to delete (user removed them)
                    var toDelete = existingImages
                        .Where(x => !keptIds.Contains(x.UploadImageId))
                        .ToList();

                    foreach (var img in toDelete)
                    {
                        var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                            img.ImagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                        if (System.IO.File.Exists(physicalPath))
                            System.IO.File.Delete(physicalPath);

                        _context.Remove(img);
                    }
                    await _context.SaveChangesAsync();

                    // 4️⃣ Save NEW images
                    if (ImageFiles != null && ImageFiles.Count > 0)
                    {
                        var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "jobimages");
                        if (!Directory.Exists(uploadFolder))
                            Directory.CreateDirectory(uploadFolder);

                        foreach (var file in ImageFiles)
                        {
                            if (file.Length > 0)
                            {
                                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                                var filePath = Path.Combine(uploadFolder, uniqueFileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                _context.Add(new UploadImage
                                {
                                    RouterJobId = job.JobId,
                                    ImagePath = "/jobimages/" + uniqueFileName
                                });
                            }
                        }

                        await _context.SaveChangesAsync();
                    }

                    // Generate new random JobNumber
                    job.JobNumber = GenerateUniqueJobNumber();

                    await _context.SaveChangesAsync();

                    // Prepare the view model for QR generation
                    var jobVm = new RouterJobVm
                    {
                        JobId = job.JobId,
                        JobNumber = job.JobNumber,
                        CustomerName = job.CustomerName,
                        JobDetails = job.JobDetails,
                        PdfFilePath = ""
                    };

                    // Generate QR + PDF
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
                                await _context.SaveChangesAsync();
                            }
                        }
                    }

                    TempData["success"] = "Router Job updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Error updating Router Job: " + ex.Message;
                }
            }
            else
            {
                TempData["error"] = "Please correct the errors and try again.";
            }
            ViewBag.ClassificationListWithMinutes = _context.classifications
            .Select(c => new ClassificationDropdownVm
            {
                ClassificationId = c.ClassificationId,
                Name = c.Name,
                Minutes = c.Minutes
            })
            .ToList();

            ViewBag.RunTypeList = new SelectList(_context.RunTypes.ToList(), "RunTypeId", "Name", vm.RunTypeId);
            ViewBag.ASFList = new SelectList(_context.ASFs.ToList(), "ASFId", "Name", vm.ASFId);
            ViewBag.MaterailList = new SelectList(_context.Materails.ToList(), "MaterailId", "Name", vm.MaterailId);
            ViewBag.JobProcessList = new SelectList(_context.JobProcesses.ToList(), "ProcessId", "Name", vm.ProcessId);

            return View(vm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routerJob = await _context.RouterJobs
                .Include(r => r.CreatedByUser)
                .Include(r => r.UpdatedByUser)
                .FirstOrDefaultAsync(m => m.JobId == id);
            if (routerJob == null)
            {
                return NotFound();
            }

            return View(routerJob);
        }

        // POST: RouterJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var routerJob = await _context.RouterJobs
                .Include(r => r.UploadImages)
                .FirstOrDefaultAsync(r => r.JobId == id);

                if (routerJob != null)
                {
                    // Delete images from disk and DB
                    if (routerJob.UploadImages != null)
                    {
                        foreach (var img in routerJob.UploadImages)
                        {
                            var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.ImagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                            if (System.IO.File.Exists(physicalPath))
                                System.IO.File.Delete(physicalPath);

                            _context.Remove(img);
                        }
                    }

                    _context.RouterJobs.Remove(routerJob);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Router Job deleted successfully.";

                }
                else
                {
                    TempData["error"] = "Router Job not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error deleting Router Job: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }


        //Get API for StageChange
        [HttpGet]
        public async Task<IActionResult> ChangeStage(int id)
        {
            var job = await _context.RouterJobs
                .Include(j => j.JobProcess)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null || job.ProcessId == null)
                return NotFound();

            // Get all steps for this job's process
            var steps = await _context.ProcessSteps
                .Where(ps => ps.ProcessId == job.ProcessId)
                .OrderBy(ps => ps.StepOrder)
                .ToListAsync();

            ViewBag.ProcessSteps = new SelectList(steps, "ProcessStepId", "StepName");
                //ViewBag.CustomerName = job.CustomerName;


            // Find current stage (if any)
            var currentStage = await _context.JobProcessStages
                .Where(s => s.JobId == id)
                .OrderByDescending(s => s.CreatedOn)
                .Select(s => s.ProcessStepId)
                .FirstOrDefaultAsync();

            return View(new ChangeStageVm
            {
                JobId = id,
                CurrentProcessStepId = currentStage,
                CustomerName = job.CustomerName 

            });
        }

        //Post API for ChangeStage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStage(ChangeStageVm vm)
        {
            if (vm.JobId == 0 || vm.NewProcessStepId == null)
            {
                TempData["error"] = "Invalid data.";
                return RedirectToAction(nameof(Index));
            }

            // Find the latest stage for this job
            var latestStage = await _context.JobProcessStages
                .Where(s => s.JobId == vm.JobId)
                .OrderByDescending(s => s.CreatedOn)
                .FirstOrDefaultAsync();

            if (latestStage != null)
            {
                latestStage.ProcessStepId = vm.NewProcessStepId.Value;
                latestStage.UpdatedOn = DateTime.UtcNow;
                _context.Update(latestStage);
            }
            else
            {
                // If no stage exists, create one
                var newStage = new JobProcessStage
                {
                    JobId = vm.JobId,
                    ProcessStepId = vm.NewProcessStepId.Value,
                    StageStatus = Enums.ProcessStageStatus.Completed,
                    UpdatedOn = DateTime.UtcNow,
                    CompletedOn = DateTime.UtcNow
                };
                _context.JobProcessStages.Add(newStage);
            }

            // Update RouterJob's UpdatedOn
            var job = await _context.RouterJobs.FindAsync(vm.JobId);
            if (job != null)
            {
                job.UpdatedOn = DateTime.UtcNow;
                _context.Update(job);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Stage updated successfully.";
            return RedirectToAction(nameof(Index));
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

        private class QrCodeResult
        {
            public string Message { get; set; }
            public string SavedPath { get; set; }
            public string ImageBase64 { get; set; }
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

        //Functions For Download_GeneratePdf
        private async Task<RouterJob?> GetJobWithDetailsAsync(int id)
        {
            return await _context.RouterJobs
                .Include(j => j.Classification)
                .Include(j => j.RunType)
                .Include(j => j.ASF)
                .Include(j => j.Materail)
                .Include(j => j.JobProcess)
                .Include(j => j.UploadImages)
                .FirstOrDefaultAsync(j => j.JobId == id);
        }

        private async Task CompleteSecondProcessStepIfNeededAsync(RouterJob job)
        {
            if (job.ProcessId.HasValue)
            {
                var secondStep = await _context.ProcessSteps
                    .Where(ps => ps.ProcessId == job.ProcessId)
                    .OrderBy(ps => ps.StepOrder)
                    .Skip(1)
                    .FirstOrDefaultAsync();

                if (secondStep != null)
                {
                    var alreadyCompleted = await _context.JobProcessStages
                        .AnyAsync(jps => jps.JobId == job.JobId && jps.ProcessStepId == secondStep.ProcessStepId && jps.StageStatus == Enums.ProcessStageStatus.Completed);

                    if (!alreadyCompleted)
                    {
                        var processStage = new JobProcessStage
                        {
                            JobId = job.JobId,
                            ProcessStepId = secondStep.ProcessStepId,
                            StageStatus = Enums.ProcessStageStatus.Completed,
                            CompletedOn = DateTime.UtcNow,
                            CreatedOn = DateTime.UtcNow
                        };
                        _context.JobProcessStages.Add(processStage);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        private async Task<List<string>> GetJobImagesBase64Async(RouterJob job)
        {
            var imageBase64List = new List<string>();
            if (job.UploadImages != null)
            {
                foreach (var img in job.UploadImages)
                {
                    var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.ImagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(physicalPath))
                    {
                        var bytes = await System.IO.File.ReadAllBytesAsync(physicalPath);
                        var base64 = "data:image/png;base64," + Convert.ToBase64String(bytes);
                        imageBase64List.Add(base64);
                    }
                }
            }
            return imageBase64List;
        }

        private async Task<string?> GetQrCodeBase64Async(RouterJob job)
        {
            if (!string.IsNullOrEmpty(job.PdfFilePath) && System.IO.File.Exists(job.PdfFilePath))
            {
                var bytes = await System.IO.File.ReadAllBytesAsync(job.PdfFilePath);
                return "data:image/png;base64," + Convert.ToBase64String(bytes);
            }
            return null;
        }

        //NameSuggestionFunction 
        [HttpGet]
        public async Task<IActionResult> GetCustomerSuggestions(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Json(new List<string>());
            }

            // We query the RouterJobs table directly as it contains the CustomerName field
            var suggestions = await _context.RouterJobs
                .Where(j => j.CustomerName != null && j.CustomerName.StartsWith(term))
                .Select(j => j.CustomerName)
                .Distinct() // Ensures "Mehtab" only appears once even if he has 50 jobs
                .OrderBy(name => name)
                .Take(10) // Optimization: limit to top 10 results
                .ToListAsync();

            return Json(suggestions);
        }

    }
}
