using Application.Domains;
using Application.Domains.Interface;
using Application.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly IExperienceRepository _experienceRepository;

        public ExperienceService(IExperienceRepository experienceRepository)
        {
            _experienceRepository = experienceRepository;
        }

        public async Task<ExperienceDomain> GetExperienceAsync(int experienceId)
        {
            var experienceDomain = await _experienceRepository.GetExperienceAsync(experienceId);
            return experienceDomain;
        }
    }
}
