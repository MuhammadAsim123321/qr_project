//using Azure;
//using Azure.Storage.Blobs;
//using Azure.Storage.Blobs.Models;
//using Microsoft.Extensions.Logging;
//using System.Collections.Concurrent;

//namespace Identity_Login.Services
//{
//    public class BlobMigrationService
//    {
//        private readonly BlobContainerClient _containerClient;
//        private readonly ILogger<BlobMigrationService> _logger;
//        private readonly IConfiguration _configuration;

//        private readonly SemaphoreSlim _parallelSemaphore;

//        private long _totalFiles;
//        private long _processedFiles;
//        private long _uploadedFiles;
//        private long _skippedFiles;
//        private long _failedFiles;
//        private long _totalBytes;
//        private long _uploadedBytes;

//        public BlobMigrationService(
//            IConfiguration configuration,
//            ILogger<BlobMigrationService> logger)
//        {
//            _configuration = configuration;
//            _logger = logger;

//            var connectionString =
//                configuration["AzureBlobStorage:ConnectionString"];

//            var containerName =
//                configuration["AzureBlobStorage:ContainerName"];

//            if (string.IsNullOrWhiteSpace(connectionString))
//                throw new InvalidOperationException(
//                    "AzureBlobStorage:ConnectionString is not configured.");

//            if (string.IsNullOrWhiteSpace(containerName))
//                throw new InvalidOperationException(
//                    "AzureBlobStorage:ContainerName is not configured.");

//            var blobServiceClient =
//                new BlobServiceClient(connectionString);

//            _containerClient =
//                blobServiceClient.GetBlobContainerClient(containerName);

//            // Keep this relatively low on App Service.
//            // Start with 4. Increase to 6-8 only if performance is good.
//            var parallelism =
//                configuration.GetValue<int?>(
//                    "BlobMigration:Parallelism") ?? 4;

//            if (parallelism < 1)
//                parallelism = 1;

//            if (parallelism > 16)
//                parallelism = 16;

//            _parallelSemaphore =
//                new SemaphoreSlim(parallelism, parallelism);
//        }

//        public async Task<MigrationResult> MigrateAsync(
//            CancellationToken cancellationToken)
//        {
//            ResetCounters();

//            var startTime = DateTime.UtcNow;

//            _logger.LogInformation(
//                "==================================================");

//            _logger.LogInformation(
//                "Blob migration started at {StartTime}",
//                startTime);

//            _logger.LogInformation(
//                "Container: {Container}",
//                _containerClient.Name);

//            await _containerClient.CreateIfNotExistsAsync(
//                cancellationToken: cancellationToken);

//            var sourceFolders = new[]
//            {
//                new MigrationFolder(
//                    Path.Combine(
//                        Directory.GetCurrentDirectory(),
//                        "wwwroot",
//                        "jobimages"),
//                    "uploadedImages"),

//                new MigrationFolder(
//                    Path.Combine(
//                        Directory.GetCurrentDirectory(),
//                        "wwwroot",
//                        "qrs"),
//                    "qrImages")
//            };

//            foreach (var folder in sourceFolders)
//            {
//                cancellationToken.ThrowIfCancellationRequested();

//                await MigrateFolderAsync(
//                    folder.SourcePath,
//                    folder.BlobPrefix,
//                    cancellationToken);
//            }

//            var duration =
//                DateTime.UtcNow - startTime;

//            var result = new MigrationResult
//            {
//                TotalFiles = _totalFiles,
//                ProcessedFiles = _processedFiles,
//                UploadedFiles = _uploadedFiles,
//                SkippedFiles = _skippedFiles,
//                FailedFiles = _failedFiles,
//                TotalBytes = _totalBytes,
//                UploadedBytes = _uploadedBytes,
//                Duration = duration
//            };

//            _logger.LogInformation(
//                "==================================================");

//            _logger.LogInformation(
//                "Blob migration completed.");

//            _logger.LogInformation(
//                "Total files: {TotalFiles}",
//                result.TotalFiles);

//            _logger.LogInformation(
//                "Uploaded: {UploadedFiles}",
//                result.UploadedFiles);

//            _logger.LogInformation(
//                "Skipped: {SkippedFiles}",
//                result.SkippedFiles);

//            _logger.LogInformation(
//                "Failed: {FailedFiles}",
//                result.FailedFiles);

//            _logger.LogInformation(
//                "Total bytes: {TotalBytes:N0}",
//                result.TotalBytes);

//            _logger.LogInformation(
//                "Uploaded bytes: {UploadedBytes:N0}",
//                result.UploadedBytes);

//            _logger.LogInformation(
//                "Duration: {Duration}",
//                result.Duration);

//            _logger.LogInformation(
//                "==================================================");

//            return result;
//        }

//        private async Task MigrateFolderAsync(
//            string sourcePath,
//            string blobPrefix,
//            CancellationToken cancellationToken)
//        {
//            if (!Directory.Exists(sourcePath))
//            {
//                _logger.LogWarning(
//                    "Source directory does not exist: {SourcePath}",
//                    sourcePath);

//                return;
//            }

//            _logger.LogInformation(
//                "Scanning folder: {SourcePath}",
//                sourcePath);

//            var files = Directory.EnumerateFiles(
//                sourcePath,
//                "*",
//                SearchOption.AllDirectories);

//            var tasks = new List<Task>();

//            foreach (var filePath in files)
//            {
//                cancellationToken.ThrowIfCancellationRequested();

//                var fileInfo = new FileInfo(filePath);

//                Interlocked.Increment(ref _totalFiles);

//                Interlocked.Add(
//                    ref _totalBytes,
//                    fileInfo.Length);

//                await _parallelSemaphore.WaitAsync(
//                    cancellationToken);

//                var task = ProcessFileAsync(
//                    filePath,
//                    sourcePath,
//                    blobPrefix,
//                    cancellationToken);

//                tasks.Add(task);

//                _ = task.ContinueWith(
//                    _ => _parallelSemaphore.Release(),
//                    CancellationToken.None,
//                    TaskContinuationOptions.ExecuteSynchronously,
//                    TaskScheduler.Default);

//                // Prevent creation of thousands of Task objects.
//                if (tasks.Count >= 100)
//                {
//                    await Task.WhenAll(tasks);
//                    tasks.Clear();
//                }
//            }

//            if (tasks.Count > 0)
//            {
//                await Task.WhenAll(tasks);
//            }
//        }

//        private async Task ProcessFileAsync(
//            string filePath,
//            string sourceRoot,
//            string blobPrefix,
//            CancellationToken cancellationToken)
//        {
//            try
//            {
//                var relativePath =
//                    Path.GetRelativePath(
//                        sourceRoot,
//                        filePath);

//                // Convert Windows path separators to Blob separators.
//                relativePath =
//                    relativePath.Replace(
//                        '\\',
//                        '/');

//                var blobName =
//                    $"{blobPrefix}/{relativePath}";

//                var blobClient =
//                    _containerClient.GetBlobClient(blobName);

//                // Resume capability:
//                // if the blob already exists, don't upload it again.
//                if (await blobClient.ExistsAsync(
//                        cancellationToken))
//                {
//                    Interlocked.Increment(
//                        ref _skippedFiles);

//                    var skippedNumber =
//                        Interlocked.Increment(
//                            ref _processedFiles);

//                    if (skippedNumber % 100 == 0)
//                    {
//                        LogProgress();
//                    }

//                    return;
//                }

//                var contentType =
//                    GetContentType(filePath);

//                const int maxAttempts = 5;

//                for (var attempt = 1;
//                     attempt <= maxAttempts;
//                     attempt++)
//                {
//                    try
//                    {
//                        await UploadFileAsync(
//                            blobClient,
//                            filePath,
//                            contentType,
//                            cancellationToken);

//                        Interlocked.Increment(
//                            ref _uploadedFiles);

//                        var fileInfo =
//                            new FileInfo(filePath);

//                        Interlocked.Add(
//                            ref _uploadedBytes,
//                            fileInfo.Length);

//                        Interlocked.Increment(
//                            ref _processedFiles);

//                        LogProgress();

//                        return;
//                    }
//                    catch (Exception ex)
//                        when (attempt < maxAttempts &&
//                              !cancellationToken.IsCancellationRequested)
//                    {
//                        var delaySeconds =
//                            Math.Pow(2, attempt);

//                        _logger.LogWarning(
//                            ex,
//                            "Upload failed. Retry {Attempt}/{MaxAttempts} in {Delay}s: {File}",
//                            attempt,
//                            maxAttempts,
//                            delaySeconds,
//                            filePath);

//                        await Task.Delay(
//                            TimeSpan.FromSeconds(
//                                delaySeconds),
//                            cancellationToken);
//                    }
//                }

//                throw new InvalidOperationException(
//                    $"Failed after {maxAttempts} attempts: {filePath}");
//            }
//            catch (OperationCanceledException)
//            {
//                throw;
//            }
//            catch (Exception ex)
//            {
//                Interlocked.Increment(
//                    ref _failedFiles);

//                Interlocked.Increment(
//                    ref _processedFiles);

//                _logger.LogError(
//                    ex,
//                    "FAILED: {FilePath}",
//                    filePath);

//                LogProgress();
//            }
//        }

//        private async Task UploadFileAsync(
//            BlobClient blobClient,
//            string filePath,
//            string contentType,
//            CancellationToken cancellationToken)
//        {
//            // FileStream streams directly from App Service disk.
//            // The entire file is NOT loaded into memory.
//            await using var stream =
//                new FileStream(
//                    filePath,
//                    FileMode.Open,
//                    FileAccess.Read,
//                    FileShare.Read,
//                    bufferSize: 1024 * 1024,
//                    options: FileOptions.SequentialScan);

//            var uploadOptions =
//                new BlobUploadOptions
//                {
//                    HttpHeaders =
//                        new BlobHttpHeaders
//                        {
//                            ContentType = contentType
//                        }
//                };

//            await blobClient.UploadAsync(
//                stream,
//                uploadOptions,
//                cancellationToken);
//        }

//        private void LogProgress()
//        {
//            var processed =
//                Interlocked.Read(
//                    ref _processedFiles);

//            if (processed == 0)
//                return;

//            // Don't write a log entry for every file.
//            // This is important when migrating tens of thousands of files.
//            if (processed % 100 != 0 &&
//                processed != Interlocked.Read(
//                    ref _totalFiles))
//            {
//                return;
//            }

//            var total =
//                Interlocked.Read(
//                    ref _totalFiles);

//            var uploaded =
//                Interlocked.Read(
//                    ref _uploadedFiles);

//            var skipped =
//                Interlocked.Read(
//                    ref _skippedFiles);

//            var failed =
//                Interlocked.Read(
//                    ref _failedFiles);

//            var percent =
//                total == 0
//                    ? 0
//                    : (double)processed / total * 100;

//            _logger.LogInformation(
//                "Migration progress: {Processed}/{Total} ({Percent:F2}%) | Uploaded: {Uploaded} | Skipped: {Skipped} | Failed: {Failed}",
//                processed,
//                total,
//                percent,
//                uploaded,
//                skipped,
//                failed);
//        }

//        private static string GetContentType(
//            string filePath)
//        {
//            var extension =
//                Path.GetExtension(filePath)
//                    .ToLowerInvariant();

//            return extension switch
//            {
//                ".jpg" or ".jpeg" => "image/jpeg",
//                ".png" => "image/png",
//                ".gif" => "image/gif",
//                ".webp" => "image/webp",
//                ".bmp" => "image/bmp",
//                ".svg" => "image/svg+xml",
//                ".ico" => "image/x-icon",
//                ".tif" or ".tiff" => "image/tiff",
//                _ => "application/octet-stream"
//            };
//        }

//        private void ResetCounters()
//        {
//            Interlocked.Exchange(
//                ref _totalFiles,
//                0);

//            Interlocked.Exchange(
//                ref _processedFiles,
//                0);

//            Interlocked.Exchange(
//                ref _uploadedFiles,
//                0);

//            Interlocked.Exchange(
//                ref _skippedFiles,
//                0);

//            Interlocked.Exchange(
//                ref _failedFiles,
//                0);

//            Interlocked.Exchange(
//                ref _totalBytes,
//                0);

//            Interlocked.Exchange(
//                ref _uploadedBytes,
//                0);
//        }

//        private record MigrationFolder(
//            string SourcePath,
//            string BlobPrefix);
//    }

//    public class MigrationResult
//    {
//        public long TotalFiles { get; set; }

//        public long ProcessedFiles { get; set; }

//        public long UploadedFiles { get; set; }

//        public long SkippedFiles { get; set; }

//        public long FailedFiles { get; set; }

//        public long TotalBytes { get; set; }

//        public long UploadedBytes { get; set; }

//        public TimeSpan Duration { get; set; }
//    }
//}
