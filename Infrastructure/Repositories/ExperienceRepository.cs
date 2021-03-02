using Application.Interface.Services;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class ExperienceRepository : IExperienceRepository
    {
        private readonly AppDbContext _context;
        public ExperienceRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
