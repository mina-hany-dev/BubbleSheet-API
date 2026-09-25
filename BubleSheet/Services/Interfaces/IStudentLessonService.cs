namespace BubleSheet.Services.Interfaces
{
    public interface IStudentLessonService
    {
        Task PutLessonsinStudentLessons(int subjectId);
        Task SetInProgress(int LessonId);
        Task SetCompelete(int LessonId);
        Task DoUpdateLessonStatus(int lessonId);
    }
}
