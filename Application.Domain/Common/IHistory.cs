using System;

namespace CleanArchitecture.Domain.Common
{
    public interface IHistory
    {
        Guid CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        Guid? ModifiedBy { get; set; }
        DateTime? ModifiedAt { get; set; }
    }
}