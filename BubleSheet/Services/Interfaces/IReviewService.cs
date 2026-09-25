using bubblesheet.Infrastracture.Dtos;
using Domain.bublesheet.Entities.enums;

namespace BubleSheet.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDTO> ReviewQuestions(int Id,submitType Type);
    }
}
