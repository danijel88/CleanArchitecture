using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Core.Repository
{
    public interface IRepository<TEntity> where TEntity : class, IObjectState, new()
    {
        void Delete(object id);
        void Delete(TEntity entity);
        TEntity Find(params object[] keyValues);
        void Insert(TEntity entity);
        void DeleteRange(ICollection<TEntity> entities);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        IQueryable<TEntity> Queryable();
        void Update(TEntity entity);
    }
}