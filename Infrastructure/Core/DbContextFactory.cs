using System;
using System.Linq;
using CleanArchitecture.Application.Core.Repository.Impl;

namespace Application.Infrastructure.Core
{
    public class DbContextFactory
    {
        public static TReturn CreateDbContext<TContext, TReturn>(string[] args, string connectionStringName,
            string schema)
            where TContext : DataContext
        {
            string hostingEnvironmentName = args.Length == 0 ? string.Empty : args.First();

            return (TReturn) Activator.CreateInstance(typeof(TContext),
                DbContextOptionsFactory.CreateOptions(hostingEnvironmentName, connectionStringName, schema));
        }
    }
}