using Application.Domains;
using Application.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using AutoMapper;
using Infrastructure.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExperienceService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void AddExperience(ExperienceDomain experience)
        {
            //this._unitOfWork.Experience.Add(experience);
        }

        public void DeleteExperience(ExperienceDomain experience)
        {
            //this._unitOfWork.Experience.Remove(experience);
        }

        public Task<bool> ExperienceExistsAsync(int experienceId)
        {
            return this._unitOfWork.Experience.AnyAsync(e => e.Id == experienceId);
        }

        public async Task<ExperienceDomain> GetExperienceAsync(int experienceId)
        {
            var experienceModel = await _unitOfWork.Experience.FirstOrDefaultAsync(n => n.Id == experienceId);

            var tagModel = await (from tag in _unitOfWork.Tag.GetAll()
                                  join combine in _unitOfWork.Tag_Experience.Where(t => t.ExperienceId == experienceModel.Id)
                                      on tag.Id equals combine.TagId
                                  select new TagDomain()
                                  {
                                      Id = tag.Id,
                                      Name = tag.Name
                                  }).ToListAsync();

            var experienceDomain = _mapper.Map<ExperienceDomain>(experienceModel);
            experienceDomain.Tags = _mapper.Map<ICollection<TagDomain>>(tagModel);

            return experienceDomain;
        }


        public Task<IEnumerable<ExperienceDomain>> GetExperiencesAsync()
        {
            throw new NotImplementedException();
        }


    }
}
