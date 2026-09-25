using Domain.bublesheet.Entities;
using bubblesheet.Infrastracture.Dtos;

namespace BubleSheet.Services.Interfaces
{
    public interface IAdvertiserService
    {
        Task<List<AdvertisersDto>> GetAllAdvertiser();
        Task AddAsyncAdvertiser(AddAdvertiserDto advertisersDto);
        Task DeleteAdvertiser(int id);
    }
}
