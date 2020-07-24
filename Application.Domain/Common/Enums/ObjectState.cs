namespace CleanArchitecture.Domain.Common.Enums
{
    public enum ObjectState : byte
    {
        Added,
        Modified,
        Deleted,
        Unchanged,
        Detached
    }
}