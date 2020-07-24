using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.Application.Core.Repository.Impl
{
    public abstract class DataContext : DbContext, IDataContextAsync
    {
        public Guid InstanceId { get; }

        protected DataContext()
        {
            InstanceId = Guid.NewGuid();
        }

        protected DataContext(DbContextOptions options): base(options)
        {
        }

        public override int SaveChanges()
        {
            SyncObjectsStatePreCommit();
            ChangeTracker contextAdapter = ChangeTracker;
            contextAdapter.DetectChanges();
            int changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        private void SyncObjectsStatePreCommit()
        {
            foreach (EntityEntry dbEntityEntry in ChangeTracker.Entries())
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (EntityEntry entityEntry in ChangeTracker.Entries())
            {
                ((IObjectState) entityEntry.Entity).ObjectState = StateHelper.ConvertState(entityEntry.State);
            }
        }

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState
        {
            Entry(entity).State = StateHelper.ConvertState(entity.ObjectState);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await SaveChangesAsync(CancellationToken.None);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SyncObjectsStatePreCommit();

            ChangeTracker contextAdapter = ChangeTracker;

            contextAdapter.DetectChanges();

            int changesAsync = await base.SaveChangesAsync(cancellationToken);

            SyncObjectsStatePostCommit();
            return changesAsync;
        }
    }
}