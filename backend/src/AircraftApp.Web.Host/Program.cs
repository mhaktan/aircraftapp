using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AircraftApp.EntityFrameworkCore;
using AircraftApp.EntityFrameworkCore.Seed;
using AircraftApp.Entities;

namespace AircraftApp.Web.Host
{
    /// <summary>
    /// Background service that runs migration + seed once at startup
    /// without blocking the HTTP pipeline.
    /// </summary>
    public class MigrationHostedService : IHostedService
    {
        private readonly IConfiguration _config;

        public MigrationHostedService(IConfiguration config)
        {
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var connStr = _config.GetConnectionString("Default") ?? "";
            if (string.IsNullOrEmpty(connStr)) return;

            // Run in background to avoid blocking host startup (prevents EF tooling timeout)
            _ = Task.Run(async () =>
            {
                // Small delay to ensure host is fully started before DB operations
                await Task.Delay(1000, cancellationToken);
                try
                {
                    var optionsBuilder = new DbContextOptionsBuilder();
                    optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));

                    using (var db = new AircraftAppDbContext(optionsBuilder.Options))
                    {
                        db.Database.EnsureCreated();
                        Console.WriteLine("[Migration] Database is up to date.");

                        // Seed sample data — wrapped in its own try so a failure here doesn't block RBAC seed below.
                        try
                        {
                    if (!db.Aircrafts.Any())
                    {
                        db.Aircrafts.AddRange(
                    new Aircraft { Id = 1, RegistrationNumber = "ABC-001", AircraftType = "Sample Item 1", Manufacturer = "Sample Item 1", Model = "Sample Item 1", SerialNumber = "ABC-001", IsActive = true },
                    new Aircraft { Id = 2, RegistrationNumber = "XYZ-002", AircraftType = "Sample Item 2", Manufacturer = "Sample Item 2", Model = "Sample Item 2", SerialNumber = "XYZ-002", IsActive = false }
                        );
                    }
                    if (!db.MaintenanceTypes.Any())
                    {
                        db.MaintenanceTypes.AddRange(
                    new MaintenanceType { Id = 3, Name = "Alice Johnson", Description = "Lorem ipsum dolor sit amet", IsActive = true },
                    new MaintenanceType { Id = 4, Name = "Bob Smith", Description = "Consectetur adipiscing elit", IsActive = false }
                        );
                    }
                    if (!db.MaintenanceRequests.Any())
                    {
                        db.MaintenanceRequests.AddRange(
                    new MaintenanceRequest { Id = 5, RequestNumber = "ABC-001", Description = "Lorem ipsum dolor sit amet", Priority = (Priority)0, RequestedBy = "Sample Item 1", LineMechanicId = 1000L, QualityInspectorId = 1000L, EstimatedDuration = 42, ActualDuration = 42, WorkPerformed = "Sample Item 1", RevisionNote = "Lorem ipsum dolor sit amet", Status = (Status)0, AircraftId = 1, MaintenanceTypeId = 3 },
                    new MaintenanceRequest { Id = 6, RequestNumber = "XYZ-002", Description = "Consectetur adipiscing elit", Priority = (Priority)1, RequestedBy = "Sample Item 2", LineMechanicId = 2000L, QualityInspectorId = 2000L, EstimatedDuration = 17, ActualDuration = 17, WorkPerformed = "Sample Item 2", RevisionNote = "Consectetur adipiscing elit", Status = (Status)1, AircraftId = 2, MaintenanceTypeId = 4 }
                        );
                    }
                    if (!db.MaintenanceLogs.Any())
                    {
                        db.MaintenanceLogs.AddRange(
                    new MaintenanceLog { Id = 7, PerformedBy = "Sample Item 1", WorkDescription = "Lorem ipsum dolor sit amet", TimeSpent = 42, LogDate = new DateTime(2024, 3, 15), MaintenanceRequestId = 5 },
                    new MaintenanceLog { Id = 8, PerformedBy = "Sample Item 2", WorkDescription = "Consectetur adipiscing elit", TimeSpent = 17, LogDate = new DateTime(2024, 6, 20), MaintenanceRequestId = 6 }
                        );
                    }
                    if (!db.SpareParts.Any())
                    {
                        db.SpareParts.AddRange(
                    new SparePart { Id = 9, PartNumber = "ABC-001", PartName = "Alice Johnson", Description = "Lorem ipsum dolor sit amet", UnitOfMeasure = "Sample Item 1", StockQuantity = 42, MinStockLevel = 42, UnitPrice = 99.99m, IsActive = true },
                    new SparePart { Id = 10, PartNumber = "XYZ-002", PartName = "Bob Smith", Description = "Consectetur adipiscing elit", UnitOfMeasure = "Sample Item 2", StockQuantity = 17, MinStockLevel = 17, UnitPrice = 149.50m, IsActive = false }
                        );
                    }
                    if (!db.MaintenancePartUsages.Any())
                    {
                        db.MaintenancePartUsages.AddRange(
                    new MaintenancePartUsage { Id = 11, QuantityUsed = 42, Notes = "Lorem ipsum dolor sit amet", MaintenanceLogId = 7, SparePartId = 9 },
                    new MaintenancePartUsage { Id = 12, QuantityUsed = 17, Notes = "Consectetur adipiscing elit", MaintenanceLogId = 8, SparePartId = 10 }
                        );
                    }
                    if (!db.PurchaseRequests.Any())
                    {
                        db.PurchaseRequests.AddRange(
                    new PurchaseRequest { Id = 13, RequestNumber = "ABC-001", RequestedQuantity = 42, UrgencyLevel = (UrgencyLevel)0, Justification = "Sample Item 1", EstimatedUnitPrice = 99.99m, RequestedBy = "Sample Item 1", Status = (Status)0, SparePartId = 9 },
                    new PurchaseRequest { Id = 14, RequestNumber = "XYZ-002", RequestedQuantity = 17, UrgencyLevel = (UrgencyLevel)1, Justification = "Sample Item 2", EstimatedUnitPrice = 149.50m, RequestedBy = "Sample Item 2", Status = (Status)1, SparePartId = 10 }
                        );
                    }
                            db.SaveChanges();
                            Console.WriteLine("[Seed] Sample data created.");
                        }
                        catch (Exception sampleEx)
                        {
                            Console.WriteLine($"[Seed] Sample data skipped: {sampleEx.GetType().Name}: {sampleEx.Message}");
                            // Carry on — RBAC seed must still run so admin/123qwe is usable.
                        }
                    }
                    // RBAC seed (Admin/User roles + permissions + admin user) runs through ABP DI
                    // so PermissionRegistry can be injected. SeedHelper is idempotent.
                    SeedHelper.SeedHostDb(Abp.Dependency.IocManager.Instance);
                    Console.WriteLine("[Seed] RBAC seed complete (Admin role + admin user).");
                }
                catch (Exception ex)
                {
                    // Full diagnostic — surface the real cause so silent seed failures are debuggable.
                    Console.WriteLine($"[Migration] FAILED: {ex.GetType().Name}: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"[Migration] InnerException: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
                    Console.WriteLine("[Migration] StackTrace:");
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("[Migration] App continues without migration — admin user will not exist.");
                }
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public class Program
    {
        // Runtime entry: WebHost is required because ABP Startup returns IServiceProvider.
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .Run();
        }

        // Design-time entry for EF Core tools (dotnet ef migrations).
        // Without this, EF tools wait 5 minutes for IHost build (resolver default timeout)
        // and then SIGTERM any running dotnet process — killing live dev servers.
        // We expose a minimal IHost that EF tools resolve in milliseconds; the actual
        // DbContext is built by IDesignTimeDbContextFactory in the EntityFrameworkCore project.
        public static IHostBuilder CreateHostBuilder(string[] args)
            => Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);
    }
}
