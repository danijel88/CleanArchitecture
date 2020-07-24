using CleanArchitecture.Application.Core.Repository;
using CleanArchitecture.Application.Core.Services;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Core
{
    public interface IBaseServiceProvider<TEntity> : IReadService<TEntity>, IWriteService<TEntity>,
        IExtendedReadService<TEntity> where TEntity : class,  IEntity, new()
    {
        IUnitOfWorkAsync UnitOfWork { get; }
        ICurrentUser CurrentUser { get; }
        IEntityService<TEntity> EntityService { get; }
    }
}