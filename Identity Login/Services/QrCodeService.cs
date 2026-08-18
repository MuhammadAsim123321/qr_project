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
            // Convert object to JSON
            var jsonData = System.Text.Json.JsonSerializer.Serialize(jobVm);

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(jsonData, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);

            string uniqueFileName = $"{jobVm.JobNumber}_{Guid.NewGuid()}.png";

            // Upload to Azure Blob Storage instead of saving to wwwroot
            var blobUrl = await _blobStorageService.UploadImageBytesAsync(qrCodeImage, uniqueFileName, "image/png");

            return blobUrl; // full https URL, e.g. https://quickanodizingstorage.blob.core.windows.net/images/xxxx.png
        }
    }
}