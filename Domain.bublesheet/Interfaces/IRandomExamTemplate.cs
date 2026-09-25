using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IRandomExamTemplate
    {
        Task<RandomExamTemplate?> GetByLessonIdAsync(int lessonId);
        Task AddAsync(RandomExamTemplate template);
        Task<RandomExamTemplate?> UpdateAsync(RandomExamTemplate template);
        Task DeleteRandExam(int Id);
    }
}
