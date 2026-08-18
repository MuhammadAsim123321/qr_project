using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace Identity_Login.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        // ✅ SIMPLIFIED & FIXED: Use connection string directly
        public BlobStorageService(IConfiguration configuration, HttpClientHandler? httpClientHandler = null)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];

            // Validate configuration
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("AzureBlobStorage:ConnectionString is not configured in appsettings.json");

            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentException("AzureBlobStorage:ContainerName is not configured in appsettings.json");

            // ✅ OPTIMAL: Create BlobServiceClient with connection string + HttpClientHandler
            var blobClientOptions = new BlobClientOptions();

            if (httpClientHandler != null)
            {
                // Use custom handler for connection pooling and SSL handling
                blobClientOptions.Transport = new Azure.Core.Pipeline.HttpClientTransport(
                    new HttpClient(httpClientHandler, disposeHandler: false));
            }

            // Create BlobServiceClient using connection string (simplest and most reliable)
            var blobServiceClient = new BlobServiceClient(connectionString, blobClientOptions);

            // Get container client
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Create container if it doesn't exist (synchronously in constructor is OK for init)
            try
            {
                _containerClient.CreateIfNotExistsAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize blob storage container", ex);
            }
        }

        // ✅ Backward compatibility: Simple constructor without HttpClientHandler
        public BlobStorageService(IConfiguration configuration)
            : this(configuration, httpClientHandler: null)
        {
        }

        // Used for uploaded images coming from a form (IFormFile)
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = _containerClient.GetBlobClient(fileName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType },
                cancellationToken: CancellationToken.None);

            return blobClient.Uri.ToString();
        }

        // Used for the QR code, which is generated in memory as a byte array (not an IFormFile)
        public async Task<string> UploadImageBytesAsync(byte[] fileBytes, string fileNameHint, string contentType)
        {
            if (fileBytes == null || fileBytes.Length == 0)
                throw new ArgumentException("File bytes cannot be null or empty");

            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileNameHint)}_{Guid.NewGuid()}{Path.GetExtension(fileNameHint)}";
            var blobClient = _containerClient.GetBlobClient(uniqueFileName);

            using var stream = new MemoryStream(fileBytes);
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType },
                cancellationToken: CancellationToken.None);

            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteImageAsync(string blobUrl)
        {
            // Guard: legacy local wwwroot paths aren't blob URLs
            if (string.IsNullOrEmpty(blobUrl) || !blobUrl.StartsWith("http"))
                return false;

            try
            {
                var fileName = GetFileNameFromUrl(blobUrl);
                var blobClient = _containerClient.GetBlobClient(fileName);
                return await blobClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting blob: {ex.Message}");
                return false;
            }
        }

        public async Task<byte[]?> DownloadImageBytesAsync(string blobUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(blobUrl))
                    return null;

                var fileName = GetFileNameFromUrl(blobUrl);
                var blobClient = _containerClient.GetBlobClient(fileName);

                if (!await blobClient.ExistsAsync())
                    return null;

                var response = await blobClient.DownloadContentAsync();
                return response.Value.Content.ToArray();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error downloading blob: {ex.Message}");
                return null;
            }
        }
        // this is GetFileNameFromUrl that extracts the file name from the blob URL
        private static string GetFileNameFromUrl(string blobUrl)
        {
            var uri = new Uri(blobUrl);
            return Path.GetFileName(uri.LocalPath);
        }
    }
}