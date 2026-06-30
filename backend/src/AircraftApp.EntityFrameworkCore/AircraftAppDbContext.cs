using Microsoft.EntityFrameworkCore;
using Abp.EntityFrameworkCore;
using AircraftApp.Entities;

namespace AircraftApp.EntityFrameworkCore
{
    public class AircraftAppDbContext : AbpDbContext
    {
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<MaintenanceType> MaintenanceTypes { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<MaintenanceLog> MaintenanceLogs { get; set; }
        public DbSet<SparePart> SpareParts { get; set; }
        public DbSet<MaintenancePartUsage> MaintenancePartUsages { get; set; }
        public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ApprovalRecord> ApprovalRecords { get; set; }
        public DbSet<StatusChangeLog> StatusChangeLogs { get; set; }


        public AircraftAppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aircraft 1:N MaintenanceRequest
            modelBuilder.Entity<MaintenanceRequest>()
                .HasOne(x => x.Aircraft)
                .WithMany(x => x.MaintenanceRequests)
                .HasForeignKey(x => x.AircraftId)
                .OnDelete(DeleteBehavior.Restrict);

            // MaintenanceType 1:N MaintenanceRequest
            modelBuilder.Entity<MaintenanceRequest>()
                .HasOne(x => x.MaintenanceType)
                .WithMany(x => x.MaintenanceRequests)
                .HasForeignKey(x => x.MaintenanceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // MaintenanceRequest 1:N MaintenanceLog
            modelBuilder.Entity<MaintenanceLog>()
                .HasOne(x => x.MaintenanceRequest)
                .WithMany(x => x.MaintenanceLogs)
                .HasForeignKey(x => x.MaintenanceRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // MaintenanceLog 1:N MaintenancePartUsage
            modelBuilder.Entity<MaintenancePartUsage>()
                .HasOne(x => x.MaintenanceLog)
                .WithMany(x => x.MaintenancePartUsages)
                .HasForeignKey(x => x.MaintenanceLogId)
                .OnDelete(DeleteBehavior.Cascade);

            // SparePart 1:N MaintenancePartUsage
            modelBuilder.Entity<MaintenancePartUsage>()
                .HasOne(x => x.SparePart)
                .WithMany(x => x.MaintenancePartUsages)
                .HasForeignKey(x => x.SparePartId)
                .OnDelete(DeleteBehavior.Restrict);

            // SparePart 1:N PurchaseRequest
            modelBuilder.Entity<PurchaseRequest>()
                .HasOne(x => x.SparePart)
                .WithMany(x => x.PurchaseRequests)
                .HasForeignKey(x => x.SparePartId)
                .OnDelete(DeleteBehavior.Restrict);


            // RBAC: AppUser N:N AppRole via UserRole junction
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRole>()
                .HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique();

            // RolePermission: AppRole 1:N RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RolePermission>()
                .HasIndex(rp => new { rp.RoleId, rp.PermissionName })
                .IsUnique();

            // AppRole.Name unique
            modelBuilder.Entity<AppRole>()
                .HasIndex(r => r.Name)
                .IsUnique();

        }
    }
}
