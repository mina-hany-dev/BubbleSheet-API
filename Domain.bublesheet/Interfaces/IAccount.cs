using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IAccount
    {
        Task AddAsync(Student student);
        Task<Student?> GetStudentByIDAsync(int id);
        Task UpdateAsync(Student student);
        Task<Student?> GetStudentByRefreshTokenAsync(string refreshToken);
        Task<Student?> GetStudentByEmailAsync(string email);
        Task DeleteStudent (Student student);
        Task<int> CountOfStudent();
        Task<List<Student>> GetAllStudentsById(List<int> IDs);
    }
}
