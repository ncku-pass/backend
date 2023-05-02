using Application.Dto.Messages;
using Application.Dto.Responses;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IBackstageService
    {
        Task<BackstageCategoriesAnalyzeResponse> CategoriesAnalyze(BackstageCategoriesAnalyzeMessage message);
        Task<BackStageAbilityAnalyzeResponse> AbilityAnalyze(BackstageCategoriesAnalyzeMessage message);
    }
}