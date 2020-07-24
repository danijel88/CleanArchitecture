using System;

namespace CleanArchitecture.Domain.Common
{
    public class Entity : IEntity
    {
        public Guid Id { get; set; }
        public long? RowVersion { get; set; }
    }
}