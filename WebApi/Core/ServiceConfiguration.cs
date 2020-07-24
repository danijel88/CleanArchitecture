namespace WebApi.Core
{
    public class ServiceConfiguration
    {
        public static string DefaultEnvironment { get; } = "Development";
        public static string ConnectionName { get; } = "ServiceDb";
        public static string ServiceSchema { get; } = "CleanArchitecture";
    }
}