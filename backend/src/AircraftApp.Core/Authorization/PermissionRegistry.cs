using System.Collections.Generic;
using Abp.Dependency;

namespace AircraftApp.Authorization
{
    /// <summary>Single permission descriptor — name, group (entity), description.</summary>
    public class PermissionInfo
    {
        public string Name { get; }
        public string Group { get; }
        public string Description { get; }
        public bool IsRbac { get; }

        public PermissionInfo(string name, string group, string description, bool isRbac)
        {
            Name = name; Group = group; Description = description; IsRbac = isRbac;
        }
    }

    public interface IPermissionRegistry
    {
        IReadOnlyList<PermissionInfo> All { get; }
    }

    public class PermissionRegistry : IPermissionRegistry, ISingletonDependency
    {
        public IReadOnlyList<PermissionInfo> All { get; } = new List<PermissionInfo>
        {
            new PermissionInfo("Aircraft.Read", "Aircraft", "Read Aircraft", false),
            new PermissionInfo("Aircraft.Create", "Aircraft", "Create Aircraft", false),
            new PermissionInfo("Aircraft.Update", "Aircraft", "Update Aircraft", false),
            new PermissionInfo("Aircraft.Delete", "Aircraft", "Delete Aircraft", false),
            new PermissionInfo("MaintenanceType.Read", "MaintenanceType", "Read MaintenanceType", false),
            new PermissionInfo("MaintenanceType.Create", "MaintenanceType", "Create MaintenanceType", false),
            new PermissionInfo("MaintenanceType.Update", "MaintenanceType", "Update MaintenanceType", false),
            new PermissionInfo("MaintenanceType.Delete", "MaintenanceType", "Delete MaintenanceType", false),
            new PermissionInfo("MaintenanceRequest.Read", "MaintenanceRequest", "Read MaintenanceRequest", false),
            new PermissionInfo("MaintenanceRequest.Create", "MaintenanceRequest", "Create MaintenanceRequest", false),
            new PermissionInfo("MaintenanceRequest.Update", "MaintenanceRequest", "Update MaintenanceRequest", false),
            new PermissionInfo("MaintenanceRequest.Delete", "MaintenanceRequest", "Delete MaintenanceRequest", false),
            new PermissionInfo("MaintenanceRequest.ChangeStatus", "MaintenanceRequest", "Change MaintenanceRequest status", false),
            new PermissionInfo("MaintenanceLog.Read", "MaintenanceLog", "Read MaintenanceLog", false),
            new PermissionInfo("MaintenanceLog.Create", "MaintenanceLog", "Create MaintenanceLog", false),
            new PermissionInfo("MaintenanceLog.Update", "MaintenanceLog", "Update MaintenanceLog", false),
            new PermissionInfo("MaintenanceLog.Delete", "MaintenanceLog", "Delete MaintenanceLog", false),
            new PermissionInfo("SparePart.Read", "SparePart", "Read SparePart", false),
            new PermissionInfo("SparePart.Create", "SparePart", "Create SparePart", false),
            new PermissionInfo("SparePart.Update", "SparePart", "Update SparePart", false),
            new PermissionInfo("SparePart.Delete", "SparePart", "Delete SparePart", false),
            new PermissionInfo("MaintenancePartUsage.Read", "MaintenancePartUsage", "Read MaintenancePartUsage", false),
            new PermissionInfo("MaintenancePartUsage.Create", "MaintenancePartUsage", "Create MaintenancePartUsage", false),
            new PermissionInfo("MaintenancePartUsage.Update", "MaintenancePartUsage", "Update MaintenancePartUsage", false),
            new PermissionInfo("MaintenancePartUsage.Delete", "MaintenancePartUsage", "Delete MaintenancePartUsage", false),
            new PermissionInfo("PurchaseRequest.Read", "PurchaseRequest", "Read PurchaseRequest", false),
            new PermissionInfo("PurchaseRequest.Create", "PurchaseRequest", "Create PurchaseRequest", false),
            new PermissionInfo("PurchaseRequest.Update", "PurchaseRequest", "Update PurchaseRequest", false),
            new PermissionInfo("PurchaseRequest.Delete", "PurchaseRequest", "Delete PurchaseRequest", false),
            new PermissionInfo("PurchaseRequest.ChangeStatus", "PurchaseRequest", "Change PurchaseRequest status", false),
            new PermissionInfo("AppUser.Read", "AppUser", "Read users", true),
            new PermissionInfo("AppRole.Read", "AppRole", "Read roles", true),
            new PermissionInfo("AppUser.Create", "AppUser", "Create users", true),
            new PermissionInfo("AppRole.Create", "AppRole", "Create roles", true),
            new PermissionInfo("AppUser.Update", "AppUser", "Update users", true),
            new PermissionInfo("AppRole.Update", "AppRole", "Update roles", true),
            new PermissionInfo("AppUser.Delete", "AppUser", "Delete users", true),
            new PermissionInfo("AppRole.Delete", "AppRole", "Delete roles", true),
            new PermissionInfo("AppRole.AssignPermissions", "AppRole", "Assign permissions to roles", true),
        };
    }
}
