using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CleanArchitecture.Application.Core.Repository;
using CleanArchitecture.Application.Core.Services;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Core
{
    public class BaseServiceProvider<TEntity> : IBaseServiceProvider<TEntity>
        where TEntity : class, IEntity, new()
    {
        public BaseServiceProvider(IEntityService<TEntity> entityService, IUnitOfWorkAsync unitOfWork,
            ICurrentUser currentUser)
        {
            EntityService = entityService;
            UnitOfWork = unitOfWork;
            CurrentUser = currentUser;
        }

        public IUnitOfWorkAsync UnitOfWork { get; }

        public ICurrentUser CurrentUser { get; }

        public IEntityService<TEntity> EntityService { get; set; }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            return await EntityService.AnyAsync(predicate);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            bool inserted = EntityService.Insert(entity);
            if (!inserted) throw new Exception("Failed inserting entity");

            bool savingResponse = await Save("Creating");
            if (savingResponse) return await Get(entity.Id);
            throw new Exception("Failed saving entity");
        }

        public async Task<bool> Delete(TEntity entity)
        {
            await EntityService.DeleteAsync(entity);
            return await Save("Deleting");
        }

        public async Task<TEntity> Get(Guid id)
        {
            TEntity entity = await EntityService.FindAsync(id);
            return entity;
        }

        public async Task<IList<TEntity>> GetAll()
        {
            IList<TEntity> result = await EntityService.SelectListAsync();
            return result;
        }

        public Task<long?> GetCurrentRowVersion(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            EntityService.Update(entity);
            bool savingResponse = await Save("Updating");
            if (savingResponse)
            {
                return await Get(entity.Id);
            }

            return null;
        }

        private async Task<bool> Save(string actionName)
        {
            try
            {
                int saveCount = await UnitOfWork.SaveChangesAsync();
                if (saveCount == 0) return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}