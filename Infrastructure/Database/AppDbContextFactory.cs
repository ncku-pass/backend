using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private readonly IConfiguration _configuration;

        public AppDbContextFactory(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public AppDbContextFactory()
        {

        }

        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql("server=localhost; database=Test_Portfolio; uid=root; pwd=123456")
                            .UseSnakeCaseNamingConvention();
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}