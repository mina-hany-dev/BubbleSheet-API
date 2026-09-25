using Domain.bublesheet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IAdvertiser
    {
        Task AddAdvertiser(Advertisers advertiser);
        Task RemoveAdvertiser(Advertisers advertiser);
        Task<List<Advertisers>> GetAllAdvertiser();
        Task<Advertisers?> GetAdById(int Id);
    }
}
