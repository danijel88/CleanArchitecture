using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Infrastructure.Core
{
    public static class DbContextOptionsFactory
    {
        /// <summary>
        ///     Creates the DBContextOptions
        /// </summary>
        /// <param name="hostingEnvironmentName">The environment for the options</param>
        /// <param name="connectionsStringName">Name of the connection within the settings file</param>
        /// <param name="schema">The schema where __ServiceMigrationsHistory will be created. Use one per service</param>
        /// <returns></returns>
        public static DbContextOptions CreateOptions(string hostingEnvironmentName, string connectionsStringName,
            string schema = "dbo")
        {
            if (string.IsNullOrEmpty(hostingEnvironmentName))
            {
                //throw new ArgumentNullException(nameof(hostingEnvironmentName));
                hostingEnvironmentName = "Development";
            }

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{hostingEnvironmentName}.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            if (configuration == null || string.IsNullOrEmpty(configuration.GetConnectionString(connectionsStringName)))
            {
                return null;
            }

            string connectionString = configuration.GetConnectionString(connectionsStringName);
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            // https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/history-table
            builder.UseSqlServer(connectionString,
                x => x.MigrationsHistoryTable("__ServiceMigrationsHistory", schema));
            return builder.Options;
        }
    }
}