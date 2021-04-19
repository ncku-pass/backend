using Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Experience> Experience { get; }
        IBaseRepository<Experience_Tag> Experience_Tag { get; }
        IBaseRepository<Tag> Tag { get; }
        IBaseRepository<User> User { get; }
        IBaseRepository<Resume> Resume { get; }
        IBaseRepository<Topic> Topic { get; }
        IBaseRepository<Topic_Experience> Topic_Experience { get; }

        /// <summary>
        /// Saves the change.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangeAsync();
    }
}