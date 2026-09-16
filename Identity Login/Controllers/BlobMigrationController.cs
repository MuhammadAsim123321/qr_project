//using Identity_Login.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace Identity_Login.Controllers
//{
//    [Authorize]
//    public class BlobMigrationController : Controller
//    {
//        private readonly BlobMigrationService _migrationService;
//        private readonly ILogger<BlobMigrationController> _logger;

//        // Prevent two migrations from running simultaneously.
//        private static readonly SemaphoreSlim MigrationLock =
//            new SemaphoreSlim(1, 1);

//        private static Task<MigrationResult>? _migrationTask;

//        private static DateTime? _startedAt;

//        public BlobMigrationController(
//            BlobMigrationService migrationService,
//            ILogger<BlobMigrationController> logger)
//        {
//            _migrationService = migrationService;
//            _logger = logger;
//        }

//        [HttpGet]
//        public IActionResult Index()
//        {
//            return Content(
//                "Blob migration endpoint is available.",
//                "text/plain");
//        }

//        [HttpGet]
//        public async Task<IActionResult> Start(
//            CancellationToken cancellationToken)
//        {
//            if (!await MigrationLock.WaitAsync(
//                    0,
//                    cancellationToken))
//            {
//                return Conflict(
//                    new
//                    {
//                        message =
//                            "A blob migration is already running."
//                    });
//            }

//            try
//            {
//                if (_migrationTask != null &&
//                    !_migrationTask.IsCompleted)
//                {
//                    return Conflict(
//                        new
//                        {
//                            message =
//                                "A blob migration is already running."
//                        });
//                }

//                _startedAt = DateTime.UtcNow;

//                _logger.LogInformation(
//                    "Starting blob migration.");

//                // Important:
//                // Do not await the migration here.
//                // The migration must continue after the HTTP
//                // response is returned.
//                _migrationTask =
//                    Task.Run(
//                        () => _migrationService.MigrateAsync(
//                            CancellationToken.None));

//                return Accepted(
//                    new
//                    {
//                        message =
//                            "Blob migration started.",
//                        startedAt = _startedAt
//                    });
//            }
//            finally
//            {
//                MigrationLock.Release();
//            }
//        }

//        [HttpGet]
//        public IActionResult Status()
//        {
//            if (_migrationTask == null)
//            {
//                return Ok(
//                    new
//                    {
//                        running = false,
//                        message =
//                            "Migration has not been started."
//                    });
//            }

//            if (!_migrationTask.IsCompleted)
//            {
//                return Ok(
//                    new
//                    {
//                        running = true,
//                        startedAt = _startedAt,
//                        message =
//                            "Migration is still running. Check application logs for progress."
//                    });
//            }

//            if (_migrationTask.IsFaulted)
//            {
//                return StatusCode(
//                    500,
//                    new
//                    {
//                        running = false,
//                        failed = true,
//                        startedAt = _startedAt,
//                        error =
//                            _migrationTask.Exception?.GetBaseException()
//                                .Message
//                    });
//            }

//            return Ok(
//                new
//                {
//                    running = false,
//                    completed = true,
//                    startedAt = _startedAt,
//                    result = _migrationTask.Result
//                });
//        }
//    }
//}
