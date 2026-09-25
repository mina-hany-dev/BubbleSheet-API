using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShop.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Student student);
        string GenerateRefreshToken();
        int GetCurrentStudentId();

    }
}
