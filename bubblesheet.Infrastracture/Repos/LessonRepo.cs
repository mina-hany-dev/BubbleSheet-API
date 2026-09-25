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
    public class LessonRepo(bubblesheetDbContext context) :ILesson
    {
         private readonly bubblesheetDbContext _context = context;
        public async Task<List<Lesson>> GetLessonsBySubjectId(int SubjectId)
        {
            return await _context.Lessons
                .Where(l => l.SubjectId == SubjectId)
                .Include(l => l.StudentLessons)
                .Include(l => l.Subject)
                    .ThenInclude(s => s.academicYear)
                .ToListAsync();
        }

        public async Task<int> GetLessonCountBySubjectId(int subjectId)
        {
            return await _context.Lessons
                .CountAsync(x => x.SubjectId == subjectId);
        }
        public async Task<Dictionary<int, int>> GetLessonCountsBySubjectIds(List<int> subjectIds)
        {
            return await _context.Lessons
                .Where(l => subjectIds.Contains(l.SubjectId))
                .GroupBy(l => l.SubjectId)
                .ToDictionaryAsync(
                    x => x.Key,
                    x => x.Count()
                );
        }
        public async Task<Lesson?> GetLessonDetailsById(int lessonId)
        {
            return await _context.Lessons
                .Include(l => l.Subject)
                .Include(l => l.Exams)
                    .ThenInclude(e => e.ExamQuestions)
                .Include(l => l.pdfFiles)
                .Include(l => l.QuestionBanks)
                .Include(l => l.StudentLessons)
                .FirstOrDefaultAsync(l => l.LessonID == lessonId);
        }
        public async Task DeleteLesson(int Id)
        {
            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(x => x.LessonID == Id);

            if (lesson == null)
                return;

            _context.Lessons.Remove(lesson);

            await _context.SaveChangesAsync();
        }

        public async Task<Lesson> UpdateLesson(Lesson lesson)
        {
            var existingLesson = await _context.Lessons
                .FirstOrDefaultAsync(x => x.LessonID == lesson.LessonID);

            if (existingLesson == null)
                return null;

            existingLesson.Update(
                lesson.LessonName,
                lesson.Description
            );

            await _context.SaveChangesAsync();

            return existingLesson;
        }
        public async Task<Lesson> AddAsync(Lesson lesson)
        {
            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();

            return lesson;
        }
        public async Task<int> LastIndex(int subjectId)
        {
            return await _context.Lessons
                .Where(l => l.SubjectId == subjectId)
                .OrderByDescending(l => l.Index)
                .Select(l => l.Index)
                .FirstOrDefaultAsync();
        }

    }
}
