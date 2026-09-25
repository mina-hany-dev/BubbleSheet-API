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
    public class ExamRepo : IExam
    {
        private readonly bubblesheetDbContext _context;
        public ExamRepo(bubblesheetDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<int> AddAsyncExam(Exam exam)
        {
            await _context.Exams.AddAsync(exam);
            await Task.CompletedTask;
            return exam.ExamID;
        }
        public async Task DeleteAsyncExam(int id)
        {
            var exam = await _context.Exams
        .FirstOrDefaultAsync(e => e.ExamID == id);

            if (exam == null)
                return;

            _context.Exams.Remove(exam);
            await Task.CompletedTask;
        }
        public async Task<List<Exam>> GetAllAsyncExmas()
        {
            return await _context.Exams.ToListAsync();
        }
        public async Task<Exam> GetExamById(int id)
        {
            return await _context.Exams
                .Include(x => x.ExamQuestions)
                .FirstOrDefaultAsync(x => x.ExamID == id);
        }
        public async Task<int> GetCountOfExamsForStudent(int studentId)
        {
            return await _context.Exams
                .CountAsync(e => e.StudentId == studentId);
        }
        public async Task<Exam> UpdateExam(Exam exam)
        {
            var existingExam = await _context.Exams
                .FirstOrDefaultAsync(x => x.ExamID == exam.ExamID);

            if (existingExam == null)
                return null;

            existingExam.Update(
                exam.ExamName,
                exam.Duration,
                exam.Description
            );

            await _context.SaveChangesAsync();

            return existingExam;
        }
        public async Task<List<int>> GetAllExamIdsByLessonId(int lessonId)
        {
            return await _context.Exams
                .Where(e => e.LessonID == lessonId)
                .Select(e => e.ExamID)
                .ToListAsync();
        }
        public async Task<int?> GetLessonIdByExamId(int examId)
        {
            return await _context.Exams
                .Where(x => x.ExamID == examId)
                .Select(x => x.LessonID)
                .FirstOrDefaultAsync();
        }
    }
}
