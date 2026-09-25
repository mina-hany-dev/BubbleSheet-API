using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Interfaces;

namespace BubleSheet.Services.Implementation
{
    public class StudentSubjectService(IstudentSubject studentSubject)
    : IStudentSubjectService
    {
        private readonly IstudentSubject _studentSubject = studentSubject;

        public async Task<bool> IsStudentSubscribedToSubject(
            int studentId,
            int subjectId)
        {
            return await _studentSubject
                .IsStudentSubscribedToSubject(studentId, subjectId);
        }
    }
}
