using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AircraftApp.EntityFrameworkCore
{
    public class AircraftAppDbContextFactory : IDesignTimeDbContextFactory<AircraftAppDbContext>
    {
        public AircraftAppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AircraftApp.Web.Host"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connStr = configuration.GetConnectionString("Default");
            var builder = new DbContextOptionsBuilder();
            builder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
            return new AircraftAppDbContext(builder.Options);
        }
    }
}
