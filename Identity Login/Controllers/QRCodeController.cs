using Identity_Login.Data;
using Identity_Login.Models.ViewModels;
//using MessagingToolkit.QRCode.Codec;
//using MessagingToolkit.QRCode.Codec.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Text;

namespace Identity_Login.Controllers
{
    public class QRCodeController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public QRCodeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> Generate([FromBody] RouterJobVm jobDetails)
        {
            if (jobDetails == null)
                return BadRequest("Invalid object data");

            // Convert object -> JSON
            string jsonData = JsonConvert.SerializeObject(jobDetails);

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(jsonData, QRCodeGenerator.ECCLevel.Q))
            {
                var qrCode = new BitmapByteQRCode(qrCodeData);
                byte[] qrCodeImage = qrCode.GetGraphic(20);

                // Save QR code file on server
                string? savedPath = await SaveQrFileAsync(qrCodeImage, jobDetails.JobNumber);

                // Update job details
                jobDetails.PdfFilePath = savedPath ?? string.Empty;

                return Ok(new
                {
                    Message = "QR code generated successfully",
                    SavedPath = savedPath,
                    ImageBase64 = Convert.ToBase64String(qrCodeImage)
                });
            }
        }

        private async Task<string?> SaveQrFileAsync(byte[] qrCodeImage, string jobNumber)
        {
            try
            {
                string directoryPath = Path.Combine(_environment.WebRootPath, "qrs");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string uniqueFileName = $"{jobNumber}_{Guid.NewGuid()}.png";
                string filePath = Path.Combine(directoryPath, uniqueFileName);

                await System.IO.File.WriteAllBytesAsync(filePath, qrCodeImage);

                return filePath;
            }
            catch
            {
                return null;
            }
        }


        [HttpGet]
        public async Task<IActionResult> GenerateDummy()
        {
            // Create dummy job details
            var dummyJob = new RouterJobVm
            {
                JobId = 1,
                JobNumber = "JOB-A1-B2-C3",
                CustomerName = "Muhammad Asim",
                QrCodeData = "Any other QR Data",
                PdfFilePath = ""
            };

            // Call Generate with dummy data
            return await Generate(dummyJob);
        }

        //[HttpGet]
        //public IActionResult ReadQrFromFile(string fileName)
        //{
        //    string directoryPath = Path.Combine(_environment.WebRootPath, "qrs");
        //    string filePath = Path.Combine(directoryPath, fileName);

        //    var jobDetails = ReadQrFileWithMessagingToolkit(filePath);

        //    if (jobDetails == null)
        //        return NotFound("QR code could not be read or file missing");

        //    return Ok(jobDetails);
        //}

        //public RouterJobVm? ReadQrFileWithMessagingToolkit(string filePath)
        //{
        //    if (!System.IO.File.Exists(filePath))
        //        return null;

        //    try
        //    {
        //        // Image.FromFile locks file until disposed; using ensures disposal
        //        using var bitmap = (Bitmap)Image.FromFile(filePath);

        //        var decoder = new QRCodeDecoder();
        //        var qrBitmapImage = new QRCodeBitmapImage(bitmap);

        //        // This returns the decoded text inside QR (usually the JSON you stored)
        //        var decodedText = decoder.Decode(qrBitmapImage);

        //        if (string.IsNullOrWhiteSpace(decodedText))
        //            return null;

        //        return JsonConvert.DeserializeObject<RouterJobVm>(decodedText);
        //    }
        //    catch (Exception ex)
        //    {
        //        // log ex if you have a logger
        //        Console.WriteLine($"QR decode failed: {ex.Message}");
        //        return null;
        //    }
        //}
    
    }
}
