using System;

namespace CleanArchitecture.Application.Core
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; set; }
        Guid UserId { get; set; }
    }
}