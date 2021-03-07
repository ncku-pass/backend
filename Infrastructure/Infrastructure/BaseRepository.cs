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
        public IQueryable<TEntity> GetAll()
        {
            return this._context.Set<TEntity>();
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return this._context.Set<TEntity>().Where(predicate);
        }
        public Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return (Task<IQueryable<TEntity>>)this._context.Set<TEntity>().AsNoTracking().Where(predicate ?? (x => true));
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this._context.Set<TEntity>().AsNoTracking().Where(predicate ?? (x => true)).CountAsync();
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
            => this._context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(predicate ?? (x => true));

        public async Task<List<TEntity>> ToListAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        public async Task AddAsync(TEntity entity)
        {
            await this._context
                .Set<TEntity>()
                .AddAsync(entity);

            await this._context.SaveChangesAsync();
        }
        public async Task UpdateAsync(TEntity entity)
        {
            if (this._context.Set<TEntity>().Local.Contains(entity))
                this._context.Set<TEntity>().Local.Remove(entity);

            _context.Set<TEntity>().Update(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this._context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
    }
}