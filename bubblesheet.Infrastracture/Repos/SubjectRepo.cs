using bubblesheet.Infrastracture.Data;
using bubblesheet.Infrastracture.Dtos;
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
    public class SubjectRepo(bubblesheetDbContext context,IUnitOfWork unitOfWork) : ISubject
    {
        private readonly bubblesheetDbContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Subject?> GetSubjectById(int id)
        {
            return await _context.subjects
         .Include(s => s.academicYear)
         .FirstOrDefaultAsync(s => s.SubjectId == id);
        }
        public async Task<List<Subject>> GetAllSubjectsByYearId(int id)
        {
            return await _context.subjects
                .Where(s => s.YearId == id)
                .Include(s => s.academicYear)
                .Include(s=>s.Lessons)
                .ToListAsync();
        }
        public async Task<Subject>AddSync(Subject subject)
        {
            await _context.subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
            return subject;
        }
        public async Task DeleteSubject(int Id)
        {
            var subject = await _context.subjects
                .FirstOrDefaultAsync(s => s.SubjectId == Id);

            if (subject == null)
                return;

            _context.subjects.Remove(subject);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Subject> UpdateSubject(Subject subject)
        {
            var existingSubject = await _context.subjects
                .FirstOrDefaultAsync(s => s.SubjectId == subject.SubjectId);

            if (existingSubject == null)
                return null;

            existingSubject.Update(
                   subject.Name,
                   subject.Price,
                   subject.Description
               );

            await _context.SaveChangesAsync();

            return existingSubject;
        }
    }
}
