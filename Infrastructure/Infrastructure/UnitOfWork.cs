using Infrastructure.Database;
using Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        public AppDbContext _context { get; private set; }

        public IBaseRepository<Experience> Experience { get; private set; }
        public IBaseRepository<Tag> Tag { get; private set; }
        public IBaseRepository<Tag_Experience> Tag_Experience { get; private set; }

        public UnitOfWork(
            AppDbContext context,
            IBaseRepository<Experience> experienceRepository,
            IBaseRepository<Tag> tagRepository,
            IBaseRepository<Tag_Experience> tag_ExperienceRepository
            )
        {
            this._context = context;
            this.Experience = experienceRepository;
            this.Tag = tagRepository;
            this.Tag_Experience = tag_ExperienceRepository;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// SaveChange
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangeAsync()
        {
            return await this._context.SaveChangesAsync() >= 0;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
                    this._context = null;
                }
            }
            this.disposed = true;
        }
    }
}