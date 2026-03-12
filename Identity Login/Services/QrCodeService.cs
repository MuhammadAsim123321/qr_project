using Identity_Login.Models.ViewModels;
using QRCoder;

namespace Identity_Login.Services
{

    public class QrCodeService
    {
        private readonly IWebHostEnvironment _environment;

        public QrCodeService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string?> GenerateAndSaveQrAsync(RouterJobVm jobVm)
        {
            // Convert object to JSON
            var jsonData = System.Text.Json.JsonSerializer.Serialize(jobVm);

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(jsonData, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);

            // Save QR code file on server
            string directoryPath = Path.Combine(_environment.WebRootPath, "qrs");
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string uniqueFileName = $"{jobVm.JobNumber}_{Guid.NewGuid()}.png";
            string filePath = Path.Combine(directoryPath, uniqueFileName);

            await System.IO.File.WriteAllBytesAsync(filePath, qrCodeImage);

            return filePath;
        }
    }
}
