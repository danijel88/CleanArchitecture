using Application.Infrastructure;
using Application.Infrastructure.Core;
using Microsoft.EntityFrameworkCore.Design;

namespace WebApi.Core.Infrastructure
{
    public class DataServiceContextFactory : IDesignTimeDbContextFactory<DataServiceContext>
    {
        public DataServiceContext CreateDbContext(string[] args)
        {
            string environment = ServiceConfiguration.DefaultEnvironment;
            if (args.Length > 0)
            {
                environment = args[0];
            }

            return DbContextFactory.CreateDbContext<DataServiceContext, DataServiceContext>(new[] { environment },
                ServiceConfiguration.ConnectionName, ServiceConfiguration.ServiceSchema);
        }
    }
}