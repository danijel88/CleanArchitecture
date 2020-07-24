using System.Linq;
using Application.Infrastructure;
using Application.Infrastructure.Core;
using CleanArchitecture.Application.Core.Repository.Impl;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApi.Core.Infrastructure
{
    public class DataServiceContext : DataContext
    {
        public DataServiceContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DataServiceContext() : base(DbContextOptionsFactory.CreateOptions(ServiceConfiguration.DefaultEnvironment, ServiceConfiguration.ConnectionName, ServiceConfiguration.ServiceSchema))
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(ServiceConfiguration.ServiceSchema);

            foreach (IMutableEntityType mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                string[] nameParts = mutableEntityType.Name.Split('.');
                string name = nameParts.Last();
                modelBuilder.Entity(mutableEntityType.Name).ToTable(name);
            }

            modelBuilder.Entity<Todo>().HasKey(k => k.Id).IsClustered(false);
        }
    }
}