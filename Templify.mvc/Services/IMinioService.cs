namespace Templify.mvc.Services
{
    public interface IMinioService
    {
        Task<bool> InitializeBucketAsync();
        Task<bool> UploadLogAsync(string logContent, string fileName);
        Task<bool> UploadLogFileAsync(string filePath, string fileName);
        Task<string> DownloadLogAsync(string fileName);
        Task<bool> DeleteLogAsync(string fileName);
        Task<List<string>> ListLogsAsync();
    }
}
