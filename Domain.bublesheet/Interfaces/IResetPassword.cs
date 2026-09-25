using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IResetPassword
    {
        Task<string>AddAsync(ResetPassword resetPassword);
        Task RemoveAsync(int userId);
        Task UpdateAsync(ResetPassword resetPassword);
        Task<ResetPassword?> GetAsync(int userId);
    }
}
