namespace CleanArchitecture.Domain.Common
{
    public interface IEntity : IIdentifiable
    {
        long? RowVersion { get; set; }
    }
}