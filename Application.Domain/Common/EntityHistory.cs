using System;

namespace CleanArchitecture.Domain.Common
{
    public class EntityHistory : IEntityHistory
    {
        public Guid Id { get; set; }
        public long? RowVersion { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}