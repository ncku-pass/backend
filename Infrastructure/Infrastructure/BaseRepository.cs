using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Infrastructure
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            this._context = context;
        }
        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _context.Set<TEntity>().Add(entity);
        }
        public void Remove(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            this._context.Entry(entity).State = EntityState.Deleted;
        }
        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            this._context.Entry(entity).State = EntityState.Modified;
        }
        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return this._context.Set<TEntity>().Any(predicate);
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this._context.Set<TEntity>().AnyAsync(predicate);
        }
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return this._context.Set<TEntity>().Where(predicate ?? (x => true));
        }
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return this._context.Set<TEntity>().Where(predicate);
        }
        public ValueTask<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null)
            => new ValueTask<IQueryable<TEntity>>(
                this._context.Set<TEntity>().AsNoTracking().Where(predicate ?? (x => true)));
        public ValueTask<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
            => new ValueTask<int>(this._context.Set<TEntity>().AsNoTracking().Where(predicate ?? (x => true)).CountAsync());
        public ValueTask<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null)
            => new ValueTask<TEntity>(this._context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(predicate ?? (x => true)));
        public ValueTask<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null)
            => new ValueTask<TEntity>(this._context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate ?? (x => true)));
        public async Task<ICollection<TEntity>> ToListAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
    }
}