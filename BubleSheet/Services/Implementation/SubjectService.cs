using bubblesheet.Infrastracture.Dtos;
using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using MiniShop.Application.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class SubjectService(ILessonService lessonServices ,IStudentLessonService studentLessonService,IUnitOfWork unitOfWork , IAccountService accountService, IStudentLesson studentLesson ,IJwtService jwtService, ISubject subject , ILesson lesson, IstudentSubject studentSubject,ILessonService lessonService) : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAccountService _accountService = accountService;
        private readonly IJwtService _jwtService = jwtService;
        private readonly ISubject _subject = subject;
        private readonly IstudentSubject _studentSubject = studentSubject;
        private readonly ILesson _lesson = lesson;
        private readonly IStudentLesson _studentLesson = studentLesson;
        private readonly IStudentLessonService _studentLessonService = studentLessonService;
        private readonly ILessonService _LessonService = lessonServices;
        public async Task BuySubject(BuySubjectDto buySubjectDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                int studentId = _jwtService.GetCurrentStudentId();

                var subject = await _subject.GetSubjectById(buySubjectDto.SubjectId);

                if (subject == null)
                    throw new Exception("Subject not found.");

                var studentSubjects = await _studentSubject
                    .GetAllSubjectsByStudentId(studentId);

                if (studentSubjects.Any(x => x.SubjectId == subject.SubjectId))
                    throw new Exception("Student already enrolled.");

                await _accountService.DeductBalance(
                    studentId,
                    subject.Price,
                    TransactionType.CoursePurchase,
                    subject.Name);

                var enrollment = new StudentSubjects(
                    studentId,
                    subject.SubjectId);

                await _studentSubject.AddAsync(enrollment);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task<List<SubjectsDto>> GetSubjectsByYearId(int yearId)
        {
            var subjects = await _subject.GetAllSubjectsByYearId(yearId);

            var subjectIds = subjects
                .Select(s => s.SubjectId)
                .ToList();

            var lessonCounts = await _lesson
                .GetLessonCountsBySubjectIds(subjectIds);

            return subjects.Select(subject => new SubjectsDto
            {
                SubjectId = subject.SubjectId,
                Name = subject.Name,
                Description = subject.Description,
                Level = subject.academicYear.ACLevel,
                LessonCount = lessonCounts.ContainsKey(subject.SubjectId)
                    ? lessonCounts[subject.SubjectId]
                    : 0

            }).ToList();
        }
        public async Task<List<DataofSubjectDto>> GetSubjectsLessonDataByYearId(int yearId) 
        {
            int studentId = _jwtService.GetCurrentStudentId();

            var subjects = await _subject.GetAllSubjectsByYearId(yearId);

            var subjectIds = subjects
                .Select(s => s.SubjectId)
                .ToList();

            var lessonCounts = await _lesson
                .GetLessonCountsBySubjectIds(subjectIds);

            var completedCounts = await _studentLesson
                .GetCompletedLessonsCountBySubjectIds(studentId, subjectIds);

            return subjects.Select(subject =>
            {
                int total = lessonCounts.GetValueOrDefault(subject.SubjectId, 0);
                int completed = completedCounts.GetValueOrDefault(subject.SubjectId, 0);

                return new DataofSubjectDto
                {
                    SubjectId = subject.SubjectId,
                    Name = subject.Name,
                    Description = subject.Description,
                    Level = subject.academicYear.ACLevel,

                    LessonCount = total,
                    CountOfLessonsCompelete = completed,
                    CountOfLessonsNotCompelete = total - completed,

                    percentage = total == 0
                        ? 0
                        : completed * 100 / total
                };
            }).ToList();
        }
        public async Task<SubjectDetailsDto> GetSubjectDetails(int subjectId)
        {
            int studentId = _jwtService.GetCurrentStudentId();

            var subject = await _subject.GetSubjectById(subjectId);

            if (subject == null)
                throw new Exception("Subject not found.");

            await _studentLessonService.PutLessonsinStudentLessons(subjectId);

            int lessonCount = await _lesson.GetLessonCountBySubjectId(subjectId);

            int completedLessons = await _studentLesson
                .GetCompletedLessonsCountBySubjectId(studentId, subjectId);

            var lessons = await _lesson.GetLessonsBySubjectId(subjectId);

            return new SubjectDetailsDto
            {
                SubjectId = subject.SubjectId,
                Name = subject.Name,
                Description = subject.Description,
                Level = subject.academicYear.ACLevel,

                LessonCount = lessonCount,
                CompletedLessons = completedLessons,
                NotCompletedLessons = lessonCount - completedLessons,

                Percentage = lessonCount == 0
                    ? 0
                    : completedLessons * 100 / lessonCount,

                Lessons = lessons.Select(l => new LessonDto
                {
                    LessonID = l.LessonID,
                    LessonName = l.LessonName,
                    Description = l.Description,
                    Index = l.Index,

                    LessonStatue = l.StudentLessons
                .Where(sl => sl.StudentId == studentId)
                .Select(sl => sl.LessonStatue)
                .FirstOrDefault()

                }).ToList()
            };
        }
        public async Task<SubjectDto> AddSubjectAsync(AddSubjectDto subjectDto)
        {
            Subject newSubject = new Subject(subjectDto.Name,
                subjectDto.Price,
                subjectDto.YearId,
                subjectDto.Description
            );

            await _subject.AddSync(newSubject);

            return new SubjectDto
            {
                SubjectId = newSubject.SubjectId,
                Name = newSubject.Name,
                Description = newSubject.Description,
                Price = newSubject.Price
            };
        }
        public async Task<SubjectDto> EditSubjectAsync(EditSubjectDTO editSubjectDTO)
        {
            var subject = await _subject.GetSubjectById(editSubjectDTO.SubjectId);

            subject.Update(
                editSubjectDTO.Name,
                editSubjectDTO.Price,
                editSubjectDTO.Description
            );

            await _unitOfWork.SaveChangesAsync();

            return new SubjectDto
            {
                SubjectId = subject.SubjectId,
                Name = subject.Name,
                Price = subject.Price,
                Description = subject.Description
            };
        }
        public async Task DeleteSubject(int subjectId)
        {
            var lessons = await _LessonService.GetLessonBySubjectIdAsync(subjectId);

            var ids = lessons.Select(l => l.LessonID).ToList();

            if (ids.Any())
            {
                await _LessonService.DeleteLessons(ids);
            }

            await _subject.DeleteSubject(subjectId);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteSubjects(List<int> IDs)
        {
            foreach (var subjectId in IDs)
            {
                await DeleteSubject(subjectId);
            }
        }
    }
}
