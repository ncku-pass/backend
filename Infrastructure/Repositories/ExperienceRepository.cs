using Application.Domains;
using Application.Domains.Interface;
using AutoMapper;
using Infrastructure.Database;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ExperienceRepository : IExperienceRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ExperienceRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ExperienceDomain> GetExperienceAsync(int experienceId)
        {
            var experienceModel = await _context.Experiences.FirstOrDefaultAsync(n => n.Id == experienceId);
            var tagModel = await (from tag in _context.Tags
                                  join combine in _context.Tag_Experiences.Where(t => t.ExperienceId == experienceModel.Id)
                                      on tag.Id equals combine.TagId
                                  select new Tag()
                                  {
                                      Id = tag.Id,
                                      Name = tag.Name
                                  }).ToListAsync();

            var experienceDomain = _mapper.Map<ExperienceDomain>(experienceModel);
            experienceDomain.Tags = _mapper.Map<ICollection<TagDomain>>(tagModel);

            return experienceDomain;
        }
    }
}
