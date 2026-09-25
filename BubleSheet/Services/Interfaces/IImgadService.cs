using bubblesheet.Infrastracture.Dtos;
using Domain.bublesheet.Entities.enums;

namespace BubleSheet.Services.Interfaces
{
    public interface IImgadService
    {
        Task<List<ImgAdDTO>> GetAllImgAds();
        Task<ImgAdDTO> AddAdAsync(AddImgAdDTO addImgAdDTO);
        Task DeleteAdAsync(int id);
        Task<ImgAdDTO> GetImgAdByLevel(Levels level);
    }
}
