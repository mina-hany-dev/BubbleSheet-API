using bubblesheet.Infrastracture.Data;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Repos
{
    public class PDFRepo(IUnitOfWork unitOfWork, bubblesheetDbContext context) : IPdf
    {
        private readonly bubblesheetDbContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task AddPdfAsync(PdfFile pdfFile)
        {
            await _context.Pdfs.AddAsync(pdfFile);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<PdfFile?> GetPdfFile(int pdfId)
        {
            return await _context.Pdfs.FirstOrDefaultAsync(p => p.PdfId == pdfId);
        }
        public async Task DeletePdf(int PdfId)
        {
            var pdf = await _context.Pdfs
                .FirstOrDefaultAsync(x => x.PdfId == PdfId);

            if (pdf == null)
                return;

            _context.Pdfs.Remove(pdf);

            await _context.SaveChangesAsync();
        }
        public async Task<List<int>> GetPdfsIdByLessonId(int Id)
        {
            return await _context.Pdfs
                .Where(x => x.LessonID == Id)
                .Select(x => x.PdfId)
                .ToListAsync();
        }
    }
}