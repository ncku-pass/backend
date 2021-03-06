using Application.Domains;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IExperienceService
    {
        Task<ExperienceDomain> GetExperienceAsync(int experienceId);
        Task<IEnumerable<ExperienceDomain>> GetExperiencesAsync();
        Task<bool> ExperienceExistsAsync(int experienceId);
        void AddExperience(ExperienceDomain experience);
        void DeleteExperience(ExperienceDomain experience);
    }
}
