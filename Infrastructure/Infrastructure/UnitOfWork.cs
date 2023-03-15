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
        public IBaseRepository<Experience_Tag> Experience_Tag { get; private set; }
        public IBaseRepository<Experience_Image> Experience_Image { get; private set; }
        public IBaseRepository<Tag> Tag { get; private set; }
        public IBaseRepository<User> User { get; private set; }
        public IBaseRepository<Resume> Resume { get; private set; }
        public IBaseRepository<Card> Card { get; private set; }
        public IBaseRepository<Card_Experience> Card_Experience { get; private set; }
        public IBaseRepository<Department> Department { get; private set; }
        public IBaseRepository<Image> Image { get; private set; }

        public UnitOfWork(
            AppDbContext context,
            IBaseRepository<Experience> experienceRepository,
            IBaseRepository<Experience_Tag> experience_Tag,
            IBaseRepository<Experience_Image> experience_Image,
            IBaseRepository<Tag> tagRepository,
            IBaseRepository<User> userRepository,
            IBaseRepository<Resume> resume,
            IBaseRepository<Card> card,
            IBaseRepository<Card_Experience> card_Experience,
            IBaseRepository<Department> department,
            IBaseRepository<Image> Image
            )
        {
            this._context = context;
            this.Experience = experienceRepository;
            this.Experience_Tag = experience_Tag;
            this.Experience_Image = experience_Image;
            this.Tag = tagRepository;
            this.User = userRepository;
            this.Resume = resume;
            this.Card = card;
            this.Card_Experience = card_Experience;
            this.Department = department;
            this.Image = Image;
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