using BubleSheet.Services.Interfaces;
using BubleSheet.Services.Models;
using BunnyCDN.Net.Storage;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace BubleSheet.Services.Implementation
{
    public class BunnyStorageService : IBunnyStorageService
    {
        private readonly BunnyCDNStorage _storage;
        private readonly BunnyOptions _options;

        public BunnyStorageService(IOptions<BunnyOptions> options)
        {
            _options = options.Value;

            _storage = new BunnyCDNStorage(
                _options.ZoneName,
                _options.AccessKey,
                _options.Region);
        }

        public async Task<FileUploadResult> UploadImageAsync(
            IFormFile file,
            string folder)
        {
            ValidateImage(file);

            return await UploadAsync(file, folder);
        }

        public async Task<FileUploadResult> UploadPdfAsync(
            IFormFile file,
            string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required.");

            if (Path.GetExtension(file.FileName)
                .Equals(".pdf", StringComparison.OrdinalIgnoreCase) == false)
            {
                throw new ArgumentException("Invalid pdf.");
            }

            return await UploadAsync(file, folder);
        }

        private async Task<FileUploadResult> UploadAsync(
            IFormFile file,
            string folder)
        {
            var extension = Path.GetExtension(file.FileName);

            var fileName = $"{Guid.NewGuid()}{extension}";

            var relativePath =
                $"{folder.Trim('/')}/{fileName}";

            var bunnyPath =
                $"/{_options.ZoneName}/{relativePath}";

            using var stream = file.OpenReadStream();

            await _storage.UploadAsync(stream, bunnyPath);

            return new FileUploadResult
            {
                FileName = fileName,

                // مهم: ده اللي نستخدمه في الحذف
                RelativePath = relativePath,

                // ده رابط الـ CDN الأساسي
                Url = $"{_options.BaseUrl.TrimEnd('/')}/{relativePath}"
            };
        }

        //public async Task DeleteAsync(string relativePath)
        //{
        //    if (string.IsNullOrWhiteSpace(relativePath))
        //        return;

        //    relativePath = relativePath.TrimStart('/');

        //    await _storage.DeleteObjectAsync(
        //        $"/{_options.ZoneName}/{relativePath}");
        //}
        public async Task DeleteAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            var uri = new Uri(url);

            var relativePath = uri.AbsolutePath.TrimStart('/');

            await _storage.DeleteObjectAsync(
                $"/{_options.ZoneName}/{relativePath}");
        }
        public async Task<bool> ExistsAsync(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return false;

            relativePath = relativePath.TrimStart('/');

            var folder = Path.GetDirectoryName(relativePath)?
                .Replace("\\", "/");

            var fileName = Path.GetFileName(relativePath);

            if (string.IsNullOrEmpty(folder))
                folder = "";

            var files = await _storage.GetStorageObjectsAsync(
                $"/{_options.ZoneName}/{folder}/");

            return files.Any(x => x.ObjectName == fileName);
        }

        public string GenerateSecureUrl(
     string relativePath,
     int expirationMinutes = 30)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentException("Path is required.");

            if (string.IsNullOrWhiteSpace(_options.UrlTokenAuthenticationKey))
                throw new InvalidOperationException(
                    "Bunny URL Token Authentication Key is not configured.");

            // لو اتبعت URL كامل، نستخرج منه الـ path
            if (Uri.TryCreate(relativePath, UriKind.Absolute, out var uri))
            {
                relativePath = uri.AbsolutePath.TrimStart('/');
            }
            else
            {
                relativePath = relativePath.TrimStart('/');
            }

            var path = "/" + relativePath;

            var expires = DateTimeOffset.UtcNow
                .AddMinutes(expirationMinutes)
                .ToUnixTimeSeconds();

            var hash = MD5.HashData(
                Encoding.UTF8.GetBytes(
                    _options.UrlTokenAuthenticationKey +
                    path +
                    expires));

            var token = Convert.ToBase64String(hash)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            return $"{_options.BaseUrl.TrimEnd('/')}" +
                   $"{path}" +
                   $"?token={token}&expires={expires}";
        }

        private static void ValidateImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException(
                    "Image is required.");

            string[] allowed =
            [
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            ];

            var ext = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            if (!allowed.Contains(ext))
                throw new ArgumentException(
                    "Invalid image.");
        }
        public async Task<double> GetFileSizeAsync(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentException("Path is required.");

            if (Uri.TryCreate(relativePath, UriKind.Absolute, out var uri))
            {
                relativePath = uri.AbsolutePath.TrimStart('/');
            }
            else
            {
                relativePath = relativePath.TrimStart('/');
            }

            var folder = Path.GetDirectoryName(relativePath)?
                .Replace("\\", "/");

            var fileName = Path.GetFileName(relativePath);

            if (string.IsNullOrEmpty(folder))
                folder = "";

            var files = await _storage.GetStorageObjectsAsync(
                $"/{_options.ZoneName}/{folder}/");

            var file = files.FirstOrDefault(x => x.ObjectName == fileName);

            if (file == null)
                throw new FileNotFoundException(
                    "File not found on Bunny Storage.",
                    relativePath);

            // Bytes → MB
            var sizeInMb = (double)file.Length / (1024 * 1024);

            return Math.Round(sizeInMb, 2);
        }
    }
}