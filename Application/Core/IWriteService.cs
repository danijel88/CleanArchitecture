using System.Threading.Tasks;

namespace CleanArchitecture.Application.Core
{
    public interface IWriteService<TEntity>
    {
        Task<TEntity> Create(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<bool> Delete(TEntity entity);
    }
}