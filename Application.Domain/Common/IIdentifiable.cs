using System;

namespace CleanArchitecture.Domain.Common
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}