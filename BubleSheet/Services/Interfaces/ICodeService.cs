using bubblesheet.Infrastracture.Dtos;
using Domain.bublesheet.Entities;

namespace BubleSheet.Services.Interfaces
{
    public interface ICodeService
    {
        Task<List<CodeDto>> GenerateCodesAsync(int count, decimal balance);
        Task DeleteCode(string CodeText);
        Task ChargeCard(CodeChargeDto dto);
        Task<List<CodeDto>> GetAllCodesDtos();
    }
}
