using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchitecture.Application.Core.Repository.Impl
{
    public class UnitOfWork : IUnitOfWorkAsync
    {
        private readonly IDataContextAsync _context;
        private readonly ICurrentUser _currentUser;
        private bool _disposed;
        private int _trancount;
        private IDbContextTransaction _transaction;

        public UnitOfWork(IDataContextAsync context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            DbContext context = (DbContext) _context;
            if (_trancount == 0)
            {
                _transaction = context.Database.BeginTransaction();
                _trancount = 1;
            }
            else
            {
                if (isolationLevel != _transaction.GetDbTransaction().IsolationLevel)
                    throw new ArgumentException("Incompatible isolation level", nameof(isolationLevel));

                _trancount++;
            }
        }

        public bool Commit()
        {
            if (_trancount == 0) return false;

            if (_trancount == 1)
            {
                _transaction.Commit();
                _trancount = 0;
            }
            else if (_trancount > 0)
            {
                _trancount--;
            }

            return true;
        }

        public void Rollback()
        {
            if (_trancount > 0)
            {
                _transaction.Rollback();
                _trancount--;
                _context.SyncObjectsStatePostCommit();
            }
        }

        public int SaveChanges()
        {
            BeforeSave();
            return _context.SaveChanges();
        }


        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            BeforeSave();
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task<int> SaveChangesAsync()
        {
            BeforeSave();
            return _context.SaveChangesAsync();
        }

        ~UnitOfWork()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        private void BeforeSave()
        {
            UpdateHistory();
            UpdateVersion();
        }

        private void UpdateHistory()
        {
            DbContext context = (DbContext) _context;
            foreach (EntityEntry entry in context.ChangeTracker.Entries().Where(e => e.Entity is IHistory))
            {
                IHistory entity = (IHistory) entry.Entity;
                if (entry.State == EntityState.Added)
                    SetCreationHistory(entity);
                else if (entry.State == EntityState.Modified) SetChangeHistory(entity);
            }
        }

        /// <summary>
        ///     Updates the RowVersion.
        /// </summary>
        private void UpdateVersion()
        {
            DbContext context = (DbContext) _context;
            foreach (EntityEntry entry in context.ChangeTracker.Entries().Where(e => e.Entity is IEntity))
            {
                IEntity entity = (IEntity) entry.Entity;
                if (entry.State == EntityState.Added)
                    entity.RowVersion = 1;
                else if (entry.State == EntityState.Modified)
                    entity.RowVersion = entity.RowVersion.HasValue ? entity.RowVersion + 1 : 1;
            }
        }

        /// <summary>
        ///     Sets the creation history.
        /// </summary>
        /// <param name="history">The history.</param>
        private void SetCreationHistory(IHistory history)
        {
            history.CreatedAt = DateTime.Now;

            if (_currentUser != null && _currentUser.IsAuthenticated)
                history.CreatedBy = _currentUser.UserId;
            else
                history.CreatedBy = Guid.Empty;

            SetChangeHistory(history);
        }

        /// <summary>
        ///     Sets the change history.
        /// </summary>
        /// <param name="history">The history.</param>
        private void SetChangeHistory(IHistory history)
        {
            history.ModifiedAt = DateTime.Now;

            if (_currentUser != null && _currentUser.IsAuthenticated)
                history.ModifiedBy = _currentUser.UserId;
            else
                history.ModifiedBy = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)

                    try
                    {
                    }
                    catch (ObjectDisposedException)
                    {
                        // do nothing, the objectContext has already been disposed
                    }

                _disposed = true;
            }
        }
    }
}