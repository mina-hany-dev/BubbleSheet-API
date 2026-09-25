using bubblesheet.Infrastracture.Dtos;

namespace BubleSheet.Services.Interfaces
{
    public interface IpdfService
    {
        Task DeletePdf(int id);
        Task DeletePdfsinLesson(int LessonId);
        Task<PDFDTO> AddPdfAsync(AddPdfDto DTO);
        Task<string> GetPdfUrlAsync(int id);
    }
}
