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

namespace Infrastructure.Infrastructure.Repositories
{
    public class ExperienceRepository<TEntity> : BaseRepository<TEntity> where TEntity : class
    {
        public ExperienceRepository(AppDbContext context) : base(context)
        {
        }
    }
}
