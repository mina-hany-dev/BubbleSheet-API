using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface ISubject
    {
        //Task AddSubject();
        Task<Subject?> GetSubjectById (int id);
        Task<List<Subject?>> GetAllSubjectsByYearId(int Id);
        Task<Subject> AddSync(Subject subject);
        Task<Subject> UpdateSubject(Subject subject);
        Task DeleteSubject(int Id);
    }
}
