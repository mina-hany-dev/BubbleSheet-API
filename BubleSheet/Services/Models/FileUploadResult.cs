namespace BubleSheet.Services.Models
{
    public class FileUploadResult
    {
        public string FileName { get; set; } = null!;
        public string RelativePath { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
