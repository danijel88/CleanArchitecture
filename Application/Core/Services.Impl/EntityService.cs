using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Core.Repository;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Core.Services.Impl
{
    public class EntityService<TEntity> : IEntityService<TEntity> where TEntity : class, IObjectState, new()
    {
        private readonly IRepositoryAsync<TEntity> _repository;

        public EntityService(IRepositoryAsync<TEntity> repository)
        {
            _repository = repository;
        }

        public bool Delete(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            TEntity entity = _repository.Find(id);

            if (entity != null)
            {
                Delete(entity);
                return true;
            }

            return false;
        }

        public bool Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity is ISoftDelete)
            {
                ((ISoftDelete) entity).IsDeleted = true;
                _repository.Update(entity);
            }
            else
            {
                _repository.Delete(entity);
            }

            return true;
        }

        public TEntity Find(params object[] keyValues)
        {
            if (keyValues == null)
                throw new ArgumentNullException(nameof(keyValues));

            TEntity foundEntity = _repository.Find(keyValues);

            if (foundEntity is ISoftDelete && ((ISoftDelete) foundEntity).IsDeleted) foundEntity = null;

            return foundEntity;
        }

        public bool Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _repository.Insert(entity);
            return true;
        }

        public bool DeleteRange(ICollection<TEntity> entities)
        {
            if (entities.Count <= 0)
                throw new ArgumentNullException(nameof(entities));

            _repository.DeleteRange(entities);

            return true;
        }

        public IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException(nameof(query));

            return _repository.SelectQuery(query, parameters);
        }

        public IQueryable<TEntity> Queryable()
        {
            return _repository.Queryable();
        }

        public bool Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _repository.Update(entity);
            ;
            return true;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repository.AnyAsync(predicate);
        }

        public async Task<bool> DeleteAsync(params object[] keyValues)
        {
            if (keyValues == null)
                throw new ArgumentNullException(nameof(keyValues));

            await _repository.DeleteAsync(keyValues);

            return true;
        }

        public async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            if (keyValues == null)
                throw new ArgumentNullException(nameof(keyValues));

            await _repository.DeleteAsync(cancellationToken, keyValues);
            return true;
        }

        public async Task<TEntity> FindAsync(params object[] keyValues)
        {
            if (keyValues == null)
                throw new ArgumentNullException(nameof(keyValues));
            TEntity foundEntity = await _repository.FindAsync(keyValues);

            if (foundEntity is ISoftDelete && ((ISoftDelete) foundEntity).IsDeleted) foundEntity = null;

            return foundEntity;
        }

        public async Task<TEntity> FindAsync(Guid id, List<Expression<Func<TEntity, object>>> includes)
        {
            TEntity foundEntity = await _repository.FindAsync(id, includes);

            if (foundEntity is ISoftDelete deletableEntity && deletableEntity.IsDeleted)
            {
                foundEntity = null;
            }

            return foundEntity;
        }

        public async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            if (keyValues == null)
                throw new ArgumentNullException(nameof(keyValues));
            TEntity foundEntity = await _repository.FindAsync(keyValues, cancellationToken);

            if (foundEntity is ISoftDelete && ((ISoftDelete) foundEntity).IsDeleted) foundEntity = null;

            return foundEntity;
        }

        public async Task<IList<TEntity>> SelectListAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null, int? page = null,
            int? pageSize = null)
        {
            return await _repository.SelectListAsync(filter, orderBy, includes, page, pageSize);
        }

        public async Task<IList<TEntity>> SelectListAsync(IQueryable<TEntity> query, int? page = null,
            int? pageSize = null)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _repository.SelectListAsync(query, page, pageSize);
        }

        public async Task<TEntity> FirstOrDefaultAsync(IQueryable<TEntity> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _repository.FirstOrDefaultAsync(query);
        }

        public async Task<IList<TResult>> SelectAsync<TResult>(IQueryable<TEntity> query,
            Expression<Func<TEntity, TResult>> selector)
        {
            return await _repository.SelectAsync(query, selector);
        }
    }
}