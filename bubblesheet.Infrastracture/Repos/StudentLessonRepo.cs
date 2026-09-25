using bubblesheet.Infrastracture.Data;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblesheet.Infrastracture.Repos
{
    public class StudentLessonRepo(bubblesheetDbContext context , IUnitOfWork unitOfWork) : IStudentLesson
    {
        private readonly bubblesheetDbContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Dictionary<int, int>> GetCompletedLessonsCountBySubjectIds(
    int studentId,
    List<int> subjectIds)
        {
            return await _context.StudentLessons
                .Where(sl =>
                    sl.StudentId == studentId &&
                    sl.LessonStatue == LessonStatue.End &&
                    subjectIds.Contains(sl.Lesson.SubjectId))
                .GroupBy(sl => sl.Lesson.SubjectId)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Count());
        }
        public async Task<int> GetCompletedLessonsCountBySubjectId(
    int studentId,
    int subjectId)
        {
            return await _context.StudentLessons
                .CountAsync(sl =>
                    sl.StudentId == studentId &&
                    sl.LessonStatue == LessonStatue.End &&
                    sl.Lesson.SubjectId == subjectId);
        }
        public async Task<List<StudentLesson>> GetAllStudentLessons()
        {
            return await _context.StudentLessons.ToListAsync();
        }
        public async Task PutLessonsinStudentLessons(int studentId, List<int> lessonsIds)
        {
            var studentLessons = lessonsIds
                .Select(id => new StudentLesson(studentId, id))
                .ToList();

            await _context.StudentLessons.AddRangeAsync(studentLessons);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<List<int>> GetExistLessonsIds(int studentId, List<int> lessonIds)
        {
            return await _context.StudentLessons
                .Where(x => x.StudentId == studentId &&
                            lessonIds.Contains(x.LessonId))
                .Select(x => x.LessonId)
                .ToListAsync();
        }
        public async Task AddRangeAsync(List<StudentLesson> studentLessons)
        {
            await _context.StudentLessons.AddRangeAsync(studentLessons);
        }
        public async Task<StudentLesson?> GetStudentLessonByStudentIdAndLessonId(
    int studentId,
    int lessonId)
        {
            return await _context.StudentLessons
                .FirstOrDefaultAsync(sl =>
                    sl.LessonId == lessonId &&
                    sl.StudentId == studentId);
        }
        public async Task DeleteAllStudentLessonsByessonId(int lessonId)
        {
            var studentLessons = await _context.StudentLessons
                .Where(x => x.LessonId == lessonId)
                .ToListAsync();

            _context.StudentLessons.RemoveRange(studentLessons);

            await _context.SaveChangesAsync();
        }
    }
}
