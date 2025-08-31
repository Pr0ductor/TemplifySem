using Microsoft.Extensions.Options;
using Templify.mvc.Settings;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Templify.mvc.Services
{
    public class MinioHealthCheckService : IHealthCheck
    {
        private readonly IMinioService _minioService;
        private readonly MinioSettings _settings;

        public MinioHealthCheckService(IMinioService minioService, IOptions<MinioSettings> settings)
        {
            _minioService = minioService;
            _settings = settings.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Проверяем подключение к MinIO
                var bucketExists = await _minioService.InitializeBucketAsync();
                
                if (bucketExists)
                {
                    return HealthCheckResult.Healthy(
                        description: $"MinIO connection is healthy. Bucket '{_settings.BucketName}' is accessible.",
                        data: new Dictionary<string, object>
                        {
                            { "Endpoint", _settings.Endpoint },
                            { "Bucket", _settings.BucketName },
                            { "Status", "Connected" }
                        });
                }
                else
                {
                    return HealthCheckResult.Unhealthy(
                        description: "MinIO bucket initialization failed.",
                        data: new Dictionary<string, object>
                        {
                            { "Endpoint", _settings.Endpoint },
                            { "Bucket", _settings.BucketName },
                            { "Status", "Failed" }
                        });
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                    description: "MinIO health check failed.",
                    exception: ex,
                    data: new Dictionary<string, object>
                    {
                        { "Endpoint", _settings.Endpoint },
                        { "Bucket", _settings.BucketName },
                        { "Status", "Error" },
                        { "ErrorMessage", ex.Message }
                    });
            }
        }
    }
}

