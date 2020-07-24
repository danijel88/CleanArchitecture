using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Core.Repository
{
    public interface IDataContext
    {
        int SaveChanges();

        void SyncObjectsStatePostCommit();

        void SyncObjectState<TEntity>(TEntity entity)
            where TEntity : class, IObjectState;
    }
}