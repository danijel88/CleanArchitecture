using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Common.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Core.Repository.Impl
{
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState, new()
    {
        private readonly IDataContextAsync _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(IDataContextAsync context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            if (context is DbContext dbContext) _dbSet = dbContext.Set<TEntity>();
        }

        public void Delete(object id)
        {
            TEntity entity = _dbSet.Find(id);
            Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);

            _context.SyncObjectState(entity);
        }

        public TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public void Insert(TEntity entity)
        {
            entity.ObjectState = ObjectState.Added;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public void DeleteRange(ICollection<TEntity> entities)
        {
            foreach (TEntity entity in entities) Delete(entity);
        }

        public IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return _dbSet.FromSqlRaw(query, parameters);
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }

        public void Update(TEntity entity)
        {
            entity.ObjectState = ObjectState.Modified;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await DeleteAsync(keyValues);
        }

        public async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            TEntity entity = await _dbSet.FindAsync(keyValues);
            if (entity == null)
                return false;
            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
            return true;
        }

        public async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public async Task<TEntity> FindAsync(Guid id, List<Expression<Func<TEntity, object>>> includes)
        {
            if (!typeof(IIdentifiable).IsAssignableFrom(typeof(IIdentifiable)))
                throw new Exception("Die Funktion steht nur für Entitäten zur Verfügung, die IEntity implementieren");

            ParameterExpression parameter = Expression.Parameter(typeof(IIdentifiable), "e");

            Expression<Func<TEntity, bool>> filter = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(Expression.Property(parameter, "Id"), Expression.Constant(id))
                , parameter);

            TEntity foundEntity = (await SelectListAsync(filter, includes: includes)).FirstOrDefault();

            return foundEntity;
        }

        public async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues, CancellationToken.None);
        }

        public async Task<IList<TEntity>> SelectListAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null, int? page = null,
            int? pageSize = null)
        {
            return await Select(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

        public async Task<IList<TEntity>> SelectListAsync(IQueryable<TEntity> query, int? page = null,
            int? pageSize = null)
        {
            if (page != null && pageSize != null)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return await query.AsExpandable().ToListAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(IQueryable<TEntity> query)
        {
            return await query.AsExpandable().FirstOrDefaultAsync();
        }

        public async Task<IList<TResult>> SelectAsync<TResult>(IQueryable<TEntity> query,
            Expression<Func<TEntity, TResult>> selector)
        {
            IQueryable<TResult> processQuery = query.AsExpandable().Select(selector);

            return await processQuery.ToListAsync();
        }

        internal IQueryable<TEntity> Select(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (orderBy != null) query = orderBy(query);

            if (filter != null) query = query.AsExpandable().Where(filter);

            if (page != null && pageSize != null)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return query;
        }
    }
}