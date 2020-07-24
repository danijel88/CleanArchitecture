using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Core
{
    public interface IReadService<TEntity>
    {
        Task<IList<TEntity>> GetAll();
        Task<TEntity> Get(Guid id);
    }
}