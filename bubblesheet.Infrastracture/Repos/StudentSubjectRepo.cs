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
    public class StudentSubjectRepo(bubblesheetDbContext context) : IstudentSubject
    {
        private readonly bubblesheetDbContext _context = context;   
        public async Task<List<Subject>> GetAllSubjectsByStudentId(int studentId)
        {
            return await _context.StudentSubjects
                .Where(x => x.StudentId == studentId)
                .Select(x => x.Subject)
                .ToListAsync();
        }
        public async Task AddAsync(StudentSubjects studentSubjects)
        {
            await _context.StudentSubjects.AddAsync(studentSubjects);
            await Task.CompletedTask;
        }
        public async Task<bool> IsStudentSubscribedToSubject(
    int studentId,
    int subjectId)
        {
            return await _context.StudentSubjects
                .AnyAsync(x =>
                    x.StudentId == studentId &&
                    x.SubjectId == subjectId);
        }
    }
}
