namespace AircraftApp.Authorization
{
    public static class PermissionNames
    {
        public const string Pages = "Pages";

        // Aircraft
        public const string Aircraft_Read = "Aircraft.Read";
        public const string Aircraft_Create = "Aircraft.Create";
        public const string Aircraft_Update = "Aircraft.Update";
        public const string Aircraft_Delete = "Aircraft.Delete";

        // MaintenanceType
        public const string MaintenanceType_Read = "MaintenanceType.Read";
        public const string MaintenanceType_Create = "MaintenanceType.Create";
        public const string MaintenanceType_Update = "MaintenanceType.Update";
        public const string MaintenanceType_Delete = "MaintenanceType.Delete";

        // MaintenanceRequest
        public const string MaintenanceRequest_Read = "MaintenanceRequest.Read";
        public const string MaintenanceRequest_Create = "MaintenanceRequest.Create";
        public const string MaintenanceRequest_Update = "MaintenanceRequest.Update";
        public const string MaintenanceRequest_Delete = "MaintenanceRequest.Delete";
        public const string MaintenanceRequest_ChangeStatus = "MaintenanceRequest.ChangeStatus";

        // MaintenanceLog
        public const string MaintenanceLog_Read = "MaintenanceLog.Read";
        public const string MaintenanceLog_Create = "MaintenanceLog.Create";
        public const string MaintenanceLog_Update = "MaintenanceLog.Update";
        public const string MaintenanceLog_Delete = "MaintenanceLog.Delete";

        // SparePart
        public const string SparePart_Read = "SparePart.Read";
        public const string SparePart_Create = "SparePart.Create";
        public const string SparePart_Update = "SparePart.Update";
        public const string SparePart_Delete = "SparePart.Delete";

        // MaintenancePartUsage
        public const string MaintenancePartUsage_Read = "MaintenancePartUsage.Read";
        public const string MaintenancePartUsage_Create = "MaintenancePartUsage.Create";
        public const string MaintenancePartUsage_Update = "MaintenancePartUsage.Update";
        public const string MaintenancePartUsage_Delete = "MaintenancePartUsage.Delete";

        // PurchaseRequest
        public const string PurchaseRequest_Read = "PurchaseRequest.Read";
        public const string PurchaseRequest_Create = "PurchaseRequest.Create";
        public const string PurchaseRequest_Update = "PurchaseRequest.Update";
        public const string PurchaseRequest_Delete = "PurchaseRequest.Delete";
        public const string PurchaseRequest_ChangeStatus = "PurchaseRequest.ChangeStatus";

        // RBAC management
        public const string AppUser_Read = "AppUser.Read";
        public const string AppRole_Read = "AppRole.Read";
        public const string AppUser_Create = "AppUser.Create";
        public const string AppRole_Create = "AppRole.Create";
        public const string AppUser_Update = "AppUser.Update";
        public const string AppRole_Update = "AppRole.Update";
        public const string AppUser_Delete = "AppUser.Delete";
        public const string AppRole_Delete = "AppRole.Delete";
        public const string AppRole_AssignPermissions = "AppRole.AssignPermissions";

    }
}
