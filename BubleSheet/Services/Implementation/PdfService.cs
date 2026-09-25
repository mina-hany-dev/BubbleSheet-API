using bubblesheet.Infrastracture.Dtos;
using bubblesheet.Infrastracture.Repos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class PdfService(IPdf pdf,IBunnyStorageService bunnyStorageService) :IpdfService
    {
        private readonly IPdf _pdf = pdf;
        private readonly IBunnyStorageService _bunnyStorageService = bunnyStorageService;
        public async Task<PDFDTO> AddPdfAsync(AddPdfDto DTO)
        {
            var pdf = await _bunnyStorageService.UploadPdfAsync(
                DTO.File,
                "PDFs");

            double size = Math.Round(
                (double)DTO.File.Length / (1024 * 1024),
                2);

            PdfFile newPdf = new PdfFile(
                pdf.Url,
                DTO.Name,
                DTO.Description,
                size,
                DTO.LessonId);

            await _pdf.AddPdfAsync(newPdf);


            return new PDFDTO
            {
                PdfId = newPdf.PdfId,
                PdfName = newPdf.PdfName,
                Description = newPdf.Description,
                Space = newPdf.Space
            };
        }
        public async Task<string> GetPdfUrlAsync(int id)
        {
            var pdf = await _pdf.GetPdfFile(id);

            if (pdf == null)
                throw new KeyNotFoundException("PDF not found.");

            return _bunnyStorageService.GenerateSecureUrl(
                pdf.PdfLink,
                1);
        }
        public async Task DeletePdf(int id)
        {
            var pdf = await _pdf.GetPdfFile(id);

            if (pdf == null)
                throw new KeyNotFoundException("PDF not found.");

            if (!string.IsNullOrEmpty(pdf.PdfLink))
            {
                await _bunnyStorageService.DeleteAsync(pdf.PdfLink);
            }

            await _pdf.DeletePdf(id);
        }
        public async Task DeletePdfsinLesson(int LessonId)
        {
            var pdfIds = await _pdf.GetPdfsIdByLessonId(LessonId);

            foreach (var pdfId in pdfIds)
            {
                await DeletePdf(pdfId);
            }
        }
    }
}
