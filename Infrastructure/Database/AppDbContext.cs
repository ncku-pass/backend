using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    public class AppDbContext : IdentityDbContext<IdentityUser> //DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSnakeCaseNamingConvention();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseModel && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseModel)entityEntry.Entity).UpdateTime = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreateTime = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync();
        }

        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Tag> Tags { get; set; }
        //public DbSet<Tag_Experience> Tag_Experiences { get; set; }
        public DbSet<Experience_Tag> Experience_Tags { get; set; }
        
        public DbSet<User> User { get; set; }
    }
}