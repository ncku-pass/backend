using Application.Dto.Messages;
using Application.Dto.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IExperienceService
    {
        Task<ExperienceResponse> GetExperienceByIdAsync(int experienceId);

        Task<IEnumerable<ExperienceResponse>> GetExperiencesAsync();

        Task<IEnumerable<ExperienceResponse>> GetByUserIdAsync(int userId);

        Task<bool> ExperienceExistsAsync(int experienceId);

        Task<ICollection<int>> ExperiencesExistsAsync(int[] expIds);

        Task<ExperienceResponse> AddExperienceAsync(ExperienceCreateMessage experience);
        Task ManipulateExp_TagRelation(int experienceId, int[] tagIds);

        Task<bool> DeleteExperienceAsync(int experienceId);

        Task<ExperienceResponse> UpdateExperienceAsync(ExperienceUpdateMessage experienceUpdateMessage);
    }
}