using Domain.bublesheet.Entities;
using Domain.bublesheet.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.bublesheet.Interfaces
{
    public interface IImgAd
    {
        Task<List<ImgAd>> GetAllAds();
        Task<ImgAd?> GetAdByLevel(Levels level);
        Task DeleteAd(int Id);
        Task AddAsync(ImgAd imgAd);
        Task UpdateAsync(ImgAd imgAd);
        Task<ImgAd> GetAdById(int Id);
    }
}
