using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Domains.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<ExperienceDomain> Experience { get; }
        IBaseRepository<TagDomain> Tag { get; }
        IBaseRepository<Tag_ExperienceDomain> Tag_Experience { get; }

        /// <summary>
        /// Saves the change.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangeAsync();
    }
}
