using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Domains.Interface
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        bool Contains(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        ValueTask<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null);
        ValueTask<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
        ValueTask<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null);
        ValueTask<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<ICollection<TEntity>> ToListAsync();
    }
}
