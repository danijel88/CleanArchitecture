using CleanArchitecture.Application.Core;
using CleanArchitecture.Application.Core.Repository;
using CleanArchitecture.Application.Core.Repository.Impl;
using CleanArchitecture.Application.Core.Services;
using CleanArchitecture.Application.Core.Services.Impl;
using CleanArchitecture.Domain.Entities;
using Lamar;
using WebApi.Core.DependencyInjection.Policies;
using WebApi.Models;

namespace WebApi.Core.Infrastructure
{
    public class ServiceContextRegistry : ServiceRegistry
    {
        public ServiceContextRegistry()
        {
            For<IDataContextAsync>().Use<DataServiceContext>();

            For(typeof(IRepositoryAsync<>)).Use(typeof(Repository<>));
            For(typeof(IRepository<>)).Use(typeof(Repository<>));
            For<IUnitOfWorkAsync>().Use<UnitOfWork>();
            For(typeof(IEntityService<>)).Use(typeof(EntityService<>));
            // Base-Services
            For(typeof(IBaseServiceProvider<>)).Use(typeof(BaseServiceProvider<>));
            For<ICurrentUser>().Use<CurrentUser>();

            Policies.Add<DefaultLifetime>();
        }
    }
}