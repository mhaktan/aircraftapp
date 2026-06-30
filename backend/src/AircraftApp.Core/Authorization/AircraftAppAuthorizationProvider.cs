using Abp.Authorization;
using Abp.Localization;

namespace AircraftApp.Authorization
{
    public class AircraftAppAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull("Pages") ?? context.CreatePermission("Pages", L("Pages"));

            // Aircraft
            pages.CreateChildPermission(PermissionNames.Aircraft_Read, L("Aircraft.Read"));
            pages.CreateChildPermission(PermissionNames.Aircraft_Create, L("Aircraft.Create"));
            pages.CreateChildPermission(PermissionNames.Aircraft_Update, L("Aircraft.Update"));
            pages.CreateChildPermission(PermissionNames.Aircraft_Delete, L("Aircraft.Delete"));

            // MaintenanceType
            pages.CreateChildPermission(PermissionNames.MaintenanceType_Read, L("MaintenanceType.Read"));
            pages.CreateChildPermission(PermissionNames.MaintenanceType_Create, L("MaintenanceType.Create"));
            pages.CreateChildPermission(PermissionNames.MaintenanceType_Update, L("MaintenanceType.Update"));
            pages.CreateChildPermission(PermissionNames.MaintenanceType_Delete, L("MaintenanceType.Delete"));

            // MaintenanceRequest
            pages.CreateChildPermission(PermissionNames.MaintenanceRequest_Read, L("MaintenanceRequest.Read"));
            pages.CreateChildPermission(PermissionNames.MaintenanceRequest_Create, L("MaintenanceRequest.Create"));
            pages.CreateChildPermission(PermissionNames.MaintenanceRequest_Update, L("MaintenanceRequest.Update"));
            pages.CreateChildPermission(PermissionNames.MaintenanceRequest_Delete, L("MaintenanceRequest.Delete"));
            pages.CreateChildPermission(PermissionNames.MaintenanceRequest_ChangeStatus, L("MaintenanceRequest.ChangeStatus"));

            // MaintenanceLog
            pages.CreateChildPermission(PermissionNames.MaintenanceLog_Read, L("MaintenanceLog.Read"));
            pages.CreateChildPermission(PermissionNames.MaintenanceLog_Create, L("MaintenanceLog.Create"));
            pages.CreateChildPermission(PermissionNames.MaintenanceLog_Update, L("MaintenanceLog.Update"));
            pages.CreateChildPermission(PermissionNames.MaintenanceLog_Delete, L("MaintenanceLog.Delete"));

            // SparePart
            pages.CreateChildPermission(PermissionNames.SparePart_Read, L("SparePart.Read"));
            pages.CreateChildPermission(PermissionNames.SparePart_Create, L("SparePart.Create"));
            pages.CreateChildPermission(PermissionNames.SparePart_Update, L("SparePart.Update"));
            pages.CreateChildPermission(PermissionNames.SparePart_Delete, L("SparePart.Delete"));

            // MaintenancePartUsage
            pages.CreateChildPermission(PermissionNames.MaintenancePartUsage_Read, L("MaintenancePartUsage.Read"));
            pages.CreateChildPermission(PermissionNames.MaintenancePartUsage_Create, L("MaintenancePartUsage.Create"));
            pages.CreateChildPermission(PermissionNames.MaintenancePartUsage_Update, L("MaintenancePartUsage.Update"));
            pages.CreateChildPermission(PermissionNames.MaintenancePartUsage_Delete, L("MaintenancePartUsage.Delete"));

            // PurchaseRequest
            pages.CreateChildPermission(PermissionNames.PurchaseRequest_Read, L("PurchaseRequest.Read"));
            pages.CreateChildPermission(PermissionNames.PurchaseRequest_Create, L("PurchaseRequest.Create"));
            pages.CreateChildPermission(PermissionNames.PurchaseRequest_Update, L("PurchaseRequest.Update"));
            pages.CreateChildPermission(PermissionNames.PurchaseRequest_Delete, L("PurchaseRequest.Delete"));
            pages.CreateChildPermission(PermissionNames.PurchaseRequest_ChangeStatus, L("PurchaseRequest.ChangeStatus"));

            // RBAC
            pages.CreateChildPermission(PermissionNames.AppUser_Read, L("AppUser.Read"));
            pages.CreateChildPermission(PermissionNames.AppRole_Read, L("AppRole.Read"));
            pages.CreateChildPermission(PermissionNames.AppUser_Create, L("AppUser.Create"));
            pages.CreateChildPermission(PermissionNames.AppRole_Create, L("AppRole.Create"));
            pages.CreateChildPermission(PermissionNames.AppUser_Update, L("AppUser.Update"));
            pages.CreateChildPermission(PermissionNames.AppRole_Update, L("AppRole.Update"));
            pages.CreateChildPermission(PermissionNames.AppUser_Delete, L("AppUser.Delete"));
            pages.CreateChildPermission(PermissionNames.AppRole_Delete, L("AppRole.Delete"));
            pages.CreateChildPermission(PermissionNames.AppRole_AssignPermissions, L("AppRole.AssignPermissions"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AircraftAppConsts.LocalizationSourceName);
        }
    }
}
