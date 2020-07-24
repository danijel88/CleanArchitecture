using System;
using System.Data;

namespace CleanArchitecture.Application.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        bool Commit();
        void Rollback();
        int SaveChanges();
    }
}