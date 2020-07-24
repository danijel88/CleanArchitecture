using System;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities
{
    public class Todo : EntityHistory
    {
        public Todo()
        {
            RowVersion = 1;
            IsActive = true;
            IsDeleted = false;
        }

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}