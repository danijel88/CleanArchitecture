using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Core.Repository
{
    public interface IRepositoryAsync<TEntity> : IRepository<TEntity> where TEntity : class, IObjectState, new()
    {
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TEntity> FindAsync(Guid id, List<Expression<Func<TEntity, object>>> includes);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);

        Task<IList<TEntity>> SelectListAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null);

        Task<IList<TEntity>> SelectListAsync(IQueryable<TEntity> query, int? page = null, int? pageSize = null);

        Task<TEntity> FirstOrDefaultAsync(IQueryable<TEntity> query);

        Task<IList<TResult>> SelectAsync<TResult>(IQueryable<TEntity> query,
            Expression<Func<TEntity, TResult>> selector);
    }
}