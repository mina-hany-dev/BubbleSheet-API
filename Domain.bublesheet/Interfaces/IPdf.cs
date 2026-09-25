using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IPdf
    {
        Task AddPdfAsync(PdfFile pdfFile);
        Task<PdfFile?> GetPdfFile(int PdfId);
        Task DeletePdf(int PdfId);
        Task<List<int>> GetPdfsIdByLessonId(int Id);
    }
}
