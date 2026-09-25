namespace BubleSheet.Services.Interfaces
{
    public interface IStudentSubjectService
    {
        Task<bool> IsStudentSubscribedToSubject(int studentId, int subjectId);
    }
}
