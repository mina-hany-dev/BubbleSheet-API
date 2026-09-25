using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
     public interface IStudentAttempts
    {
        Task<bool> HasStudentTakenFreeAttemptExam(int studentId, int examId);
        Task<List<int?>> GetAllStudentAttemptExamIds(int studentId);
        Task AddAsync(StudentAttempt studentAttempt);
        Task<StudentAttempt?> GetById(int AttempetId);
        Task<List<int?>> GetAllStudentAttemptQuestionBankIds(int studentId);
        Task RemoveStudentAttempt(int AttempetId); 
        Task RemoveRangeStudentAttempt(List<int> AttempetIDs);
        Task<StudentAttempt?> GetAttemptHasNotSumbittedByIdForExam(
                    int studentId,
                    int examId);
        Task<StudentAttempt?> GetAttemptHasNotSumbittedByIdForQuestionBank(
           int studentId,
           int questionBankId);
        Task <List<StudentAttempt>> GetStudentAttempetsByStudentId(int studentId);
        Task<int?> GetAttemptIdByIdAndType(
     int studentId,
     int id,
     submitType submitType);
        Task<int> GetNumberOfAttemptByStudentIdAndType(int StudentId, submitType submitType);

        Task<List<int>> GetAllAttemptIDs(int Id,submitType submitType, bool all = false);

        Task<bool> IfStudentSolveQbank(int StudentId, int QBank);

        //Task<int> GetLastExamAndQuestion(int studentId);
        //Task<int?> GetStudentAttemptID (int studentId,int examId);
    }
}
