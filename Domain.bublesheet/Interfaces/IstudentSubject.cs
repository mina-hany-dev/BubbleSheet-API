using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IstudentSubject
    {
        Task<List<Subject>> GetAllSubjectsByStudentId(int studentId);
        Task AddAsync (StudentSubjects studentSubjects);
        Task<bool> IsStudentSubscribedToSubject(int studentId,int subjectId);
    }
}
