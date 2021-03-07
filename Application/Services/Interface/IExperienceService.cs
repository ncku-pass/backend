using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IExperienceService
    {
        Task<ExperienceDto> GetExperienceByIdAsync(int experienceId);
        Task<IEnumerable<ExperienceDto>> GetExperiencesAsync();
        Task<bool> ExperienceExistsAsync(int experienceId);
        void AddExperience(ExperienceDtoCreate experience);
        void DeleteExperienceAsync(int experienceId);
    }
}
