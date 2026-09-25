using BubleSheet.Services.Models;

namespace BubleSheet.Services.Interfaces
{
    public interface IBunnyStorageService
    {
        Task<FileUploadResult> UploadImageAsync(IFormFile file, string folder);

        Task<FileUploadResult> UploadPdfAsync(IFormFile file, string folder);

        Task DeleteAsync(string relativePath);

        Task<bool> ExistsAsync(string relativePath);

        string GenerateSecureUrl(string relativePath,int expirationMinutes = 30);
        Task<double> GetFileSizeAsync(string relativePath);
    }
}
