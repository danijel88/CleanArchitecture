using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Core
{
    public interface IExtendedReadService<TEntity>
    {
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        Task<long?> GetCurrentRowVersion(Guid id);
    }
}