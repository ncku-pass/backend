using Application.Dto;
using Application.Dto.Messages;
using Application.Dto.Responses;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IExperienceService
    {
        Task<ExperienceResponse> GetExperienceByIdAsync(int experienceId);
        Task<IEnumerable<ExperienceResponse>> GetExperiencesAsync();
        Task<bool> ExperienceExistsAsync(int experienceId);
        Task<ExperienceResponse> AddExperienceAsync(ExperienceCreateMessage experience);
        Task<bool> DeleteExperienceAsync(int experienceId);
    }
}
