using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Options;
using Templify.mvc.Settings;

namespace Templify.mvc.Services
{
    public class S3MinioService : IMinioService
    {
        private readonly IAmazonS3 _s3;
        private readonly MinioSettings _settings;
        private readonly ILogger<S3MinioService> _logger;

        public S3MinioService(IOptions<MinioSettings> settings, ILogger<S3MinioService> logger)
        {
            _settings = settings.Value;
            _logger = logger;

            var config = new AmazonS3Config
            {
                ServiceURL = ($"http{(_settings.UseSSL ? "s" : string.Empty)}://{_settings.Endpoint}"),
                ForcePathStyle = true,
                UseHttp = !_settings.UseSSL,
                SignatureVersion = "4"
            };

            _s3 = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, config);
        }

        public async Task<bool> InitializeBucketAsync()
        {
            try
            {
                var exists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3, _settings.BucketName);
                if (!exists)
                {
                    await _s3.PutBucketAsync(new PutBucketRequest { BucketName = _settings.BucketName });
                    _logger.LogInformation("Bucket {Bucket} created", _settings.BucketName);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to init bucket {Bucket}", _settings.BucketName);
                return false;
            }
        }

        public async Task<bool> UploadLogAsync(string logContent, string fileName)
        {
            try
            {
                using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(logContent));
                var put = new PutObjectRequest
                {
                    BucketName = _settings.BucketName,
                    Key = fileName,
                    InputStream = stream,
                    ContentType = "application/octet-stream"
                };
                await _s3.PutObjectAsync(put);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload log failed {File}", fileName);
                return false;
            }
        }

        public async Task<bool> UploadLogFileAsync(string filePath, string fileName)
        {
            try
            {
                if (!File.Exists(filePath)) return false;
                var put = new PutObjectRequest
                {
                    BucketName = _settings.BucketName,
                    Key = fileName,
                    FilePath = filePath,
                    ContentType = "application/octet-stream"
                };
                await _s3.PutObjectAsync(put);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload file failed {File}", fileName);
                return false;
            }
        }

        public async Task<string> DownloadLogAsync(string fileName)
        {
            try
            {
                var resp = await _s3.GetObjectAsync(_settings.BucketName, fileName);
                using var reader = new StreamReader(resp.ResponseStream);
                return await reader.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Download failed {File}", fileName);
                return string.Empty;
            }
        }

        public async Task<bool> DeleteLogAsync(string fileName)
        {
            try
            {
                await _s3.DeleteObjectAsync(_settings.BucketName, fileName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete failed {File}", fileName);
                return false;
            }
        }

        public async Task<List<string>> ListLogsAsync()
        {
            var list = new List<string>();
            try
            {
                string? token = null;
                do
                {
                    var resp = await _s3.ListObjectsV2Async(new ListObjectsV2Request
                    {
                        BucketName = _settings.BucketName,
                        ContinuationToken = token
                    });
                    list.AddRange(resp.S3Objects.Select(o => o.Key));
                    token = resp.IsTruncated ? resp.NextContinuationToken : null;
                }
                while (token != null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "List logs failed");
            }
            return list;
        }
    }
}
