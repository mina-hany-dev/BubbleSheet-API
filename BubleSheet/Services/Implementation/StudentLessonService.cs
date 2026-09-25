using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MiniShop.Application.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class StudentLessonService(IExam exam,IStudentAttemptService studentAttemptservice,IUnitOfWork unitOfWork,IJwtService jwtService,ILesson lesson,IStudentLesson studentLesson) : IStudentLessonService
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly ILesson _lesson = lesson;
        private readonly IStudentLesson _studentLesson = studentLesson;
        private readonly IUnitOfWork _unitofwork = unitOfWork;
        private readonly IStudentAttemptService _studentAttempetservice = studentAttemptservice;
        private readonly IExam _exam = exam;
        public async Task PutLessonsinStudentLessons(int subjectId)
        {
            var studentId = _jwtService.GetCurrentStudentId();

            var lessons = await _lesson.GetLessonsBySubjectId(subjectId);

            if (lessons == null || !lessons.Any())
                return;

            var lessonIds = lessons
                .Select(x => x.LessonID)
                .ToList();

            var existingLessonIds = await _studentLesson
                .GetExistLessonsIds(studentId, lessonIds);

            var studentLessons = lessonIds
                .Except(existingLessonIds)
                .Select(lessonId => new StudentLesson(studentId,lessonId))
                .ToList();

            if (studentLessons.Count == 0)
                return;

            await _studentLesson.AddRangeAsync(studentLessons);
            await _unitofwork.SaveChangesAsync();
        }
        public async Task SetInProgress(int LessonId)
        {
            var studentId = _jwtService.GetCurrentStudentId();
            var LessonStudent = await _studentLesson.GetStudentLessonByStudentIdAndLessonId(studentId, LessonId);
            if (LessonStudent == null) return;
            LessonStudent.StartLesson();
            await _unitofwork.SaveChangesAsync();
        }
        public async Task SetCompelete(int LessonId)
        {
            var studentId = _jwtService.GetCurrentStudentId();
            var LessonStudent = await _studentLesson.GetStudentLessonByStudentIdAndLessonId(studentId, LessonId);
            if (LessonStudent == null) return;
            LessonStudent.CompleteLesson();
            await _unitofwork.SaveChangesAsync();
        }
        public async Task DoUpdateLessonStatus(int lessonId)
        {
            var studentId = _jwtService.GetCurrentStudentId();

            var isCompleted = await _studentAttempetservice
                .IsStudentCompeleteAllLesson(studentId, lessonId);

            if (isCompleted)
                await SetCompelete(lessonId);
            else
                await SetInProgress(lessonId);
        }
    }
}
