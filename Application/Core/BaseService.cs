using System.Threading.Tasks;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Core
{
    public abstract class BaseService<TEntity> : IWriteService<TEntity> where TEntity : class, IEntity, new()
    {
        private readonly IBaseServiceProvider<TEntity> _baseServiceProvider;

        protected BaseService(IBaseServiceProvider<TEntity> baseServiceProvider)
        {
            _baseServiceProvider = baseServiceProvider;
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            return await _baseServiceProvider.Create(entity);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            return await _baseServiceProvider.Update(entity);
        }

        public async Task<bool> Delete(TEntity entity)
        {
            return await _baseServiceProvider.Delete(entity);
        }
    }
}