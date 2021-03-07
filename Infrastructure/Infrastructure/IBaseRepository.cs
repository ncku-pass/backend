using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Infrastructure
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        bool Contains(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<List<TEntity>> ToListAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        /// <summary>
        /// 取得單筆
        /// </summary>
        /// <param name="predicate">查詢條件</param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
