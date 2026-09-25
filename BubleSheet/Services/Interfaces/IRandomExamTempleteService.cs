using bubblesheet.Infrastracture.Dtos;
using Domain.bublesheet.Entities;

namespace BubleSheet.Services.Interfaces
{
    public interface IRandomExamTempleteService
    {
        Task<RandomExamTemplate?> updateAsync(UpdateRandomExamTemplate template);
    }
}
