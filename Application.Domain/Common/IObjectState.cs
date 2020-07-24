using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Domain.Common.Enums;

namespace CleanArchitecture.Domain.Common
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}