using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Templify.mvc.Services;
using Templify.mvc.Attributes;

namespace Templify.mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [RequireRole("Admin")]
    public class LogsController : Controller
    {
        private readonly IMinioService _minioService;
        private readonly IMinioLoggingService _minioLoggingService;
        private readonly ILogger<LogsController> _logger;

        public LogsController(
            IMinioService minioService,
            IMinioLoggingService minioLoggingService,
            ILogger<LogsController> logger)
        {
            _minioService = minioService;
            _minioLoggingService = minioLoggingService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var logs = await _minioService.ListLogsAsync();
                ViewBag.TotalLogs = logs.Count;
                ViewBag.Logs = logs.OrderByDescending(l => l).Take(50).ToList();
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve logs from MinIO");
                TempData["ErrorMessage"] = "Failed to retrieve logs from MinIO";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SyncLogs()
        {
            try
            {
                var result = await _minioLoggingService.SyncLogsToMinioAsync();
                if (result)
                {
                    TempData["SuccessMessage"] = "Logs synchronized successfully with MinIO";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to synchronize logs with MinIO";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during log synchronization");
                TempData["ErrorMessage"] = "Error during log synchronization";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ArchiveLogs()
        {
            try
            {
                var result = await _minioLoggingService.ArchiveLogsAsync();
                if (result)
                {
                    TempData["SuccessMessage"] = "Logs archived successfully to MinIO";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to archive logs to MinIO";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during log archiving");
                TempData["ErrorMessage"] = "Error during log archiving";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DownloadLog(string fileName)
        {
            try
            {
                var logContent = await _minioService.DownloadLogAsync(fileName);
                if (string.IsNullOrEmpty(logContent))
                {
                    TempData["ErrorMessage"] = "Failed to download log from MinIO";
                    return RedirectToAction(nameof(Index));
                }

                var bytes = System.Text.Encoding.UTF8.GetBytes(logContent);
                return File(bytes, "application/json", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download log: {FileName}", fileName);
                TempData["ErrorMessage"] = "Failed to download log from MinIO";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLog(string fileName)
        {
            try
            {
                var result = await _minioService.DeleteLogAsync(fileName);
                if (result)
                {
                    TempData["SuccessMessage"] = $"Log '{fileName}' deleted successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to delete log '{fileName}'";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete log: {FileName}", fileName);
                TempData["ErrorMessage"] = $"Failed to delete log '{fileName}'";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ViewLog(string fileName)
        {
            try
            {
                var logContent = await _minioService.DownloadLogAsync(fileName);
                if (string.IsNullOrEmpty(logContent))
                {
                    TempData["ErrorMessage"] = "Failed to download log from MinIO";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.FileName = fileName;
                ViewBag.LogContent = logContent;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to view log: {FileName}", fileName);
                TempData["ErrorMessage"] = "Failed to view log from MinIO";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

