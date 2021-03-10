using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSnakeCaseNamingConvention();

        // TODO:自動更新Create、Update Time
        // https://www.entityframeworktutorial.net/faq/set-created-and-modified-date-in-efcore.aspx
        //public override int SaveChanges()
        //{
        //    var entries = ChangeTracker
        //        .Entries()
        //        .Where(e => e.Entity is BaseEntity && (
        //                e.State == EntityState.Added
        //                || e.State == EntityState.Modified));

        //    foreach (var entityEntry in entries)
        //    {
        //        ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

        //        if (entityEntry.State == EntityState.Added)
        //        {
        //            ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
        //        }
        //    }

        //    return base.SaveChanges();
        //}

        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Tag_Experience> Tag_Experiences { get; set; }
    }
}