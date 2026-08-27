using System.Diagnostics;
using Identity_Login.Models.ViewModels;
using QRCoder;

namespace Identity_Login.Services
{

    public class QrCodeService
    {
        private readonly BlobStorageService _blobStorageService;

        public QrCodeService(BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public async Task<string?> GenerateAndSaveQrAsync(RouterJobVm jobVm)
        {
            try
            {
                if (jobVm == null)
                    throw new ArgumentNullException(nameof(jobVm));

                // ✅ Convert object to JSON
                var jsonData = System.Text.Json.JsonSerializer.Serialize(jobVm);
                Debug.WriteLine($"📝 Generating QR for Job: {jobVm.JobNumber}");

                // ✅ Generate QR code
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(jsonData, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new BitmapByteQRCode(qrCodeData);
                byte[] qrCodeImage = qrCode.GetGraphic(20);

                Debug.WriteLine($"✅ QR Image generated: {qrCodeImage.Length} bytes");

                // ✅ Upload to Azure Blob Storage
                string uniqueFileName = $"{jobVm.JobNumber}_{Guid.NewGuid()}.png";
                var blobUrl = await _blobStorageService.UploadImageBytesAsync(qrCodeImage, uniqueFileName, "image/png");

                Debug.WriteLine($"✅ QR uploaded to blob: {blobUrl}");
                return blobUrl;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error in GenerateAndSaveQrAsync: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Re-throw so caller can handle it
            }
        }
    }
}