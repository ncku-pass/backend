using Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Experience> Experience { get; }
        IBaseRepository<Tag> Tag { get; }
        IBaseRepository<Tag_Experience> Tag_Experience { get; }
        IBaseRepository<User> User { get; }

        /// <summary>
        /// Saves the change.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangeAsync();
    }
}