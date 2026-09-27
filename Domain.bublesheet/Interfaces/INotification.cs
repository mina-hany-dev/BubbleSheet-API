using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface INotification
    {
        Task<Notification> AddAsync(Notification notification);

        Task<Notification?> GetByIdAsync(int id);

        Task<List<Notification>> GetByStudentIdAsync(int studentId);

        Task<List<Notification>> GetUnreadByStudentIdAsync(int studentId);
    }
}
