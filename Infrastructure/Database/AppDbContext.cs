using Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Tag_Experience> Tag_Experiences { get; set; }
    }
}
