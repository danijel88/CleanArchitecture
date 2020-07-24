using System;

namespace CleanArchitecture.Application.Core
{
    public interface IRowVersionedRequest
    {
        Guid Id { get; }
        long RowVersion { get;  }
    }
}