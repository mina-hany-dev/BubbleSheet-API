using bubblesheet.Infrastracture.Dtos;

namespace BubleSheet.Services.Interfaces
{
    public interface IYearService
    {
        Task<YearsResponseDto> AddAsync(AddYearDto addYearDto);
        Task<YearsResponseDto> EditYear(EditYearDto dto);
        Task DeleteYear(int yearId);
        Task DeleteImgYear(int yearId);
    }
}
