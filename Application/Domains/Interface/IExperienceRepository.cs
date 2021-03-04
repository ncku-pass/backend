using Application.Domains;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Domains.Interface
{
    public interface IExperienceRepository
    {
        Task<ExperienceDomain> GetExperienceAsync(int experienceId);
    }
}
