// ---------------------------------------------------------------------------
// Enum definitions — auto-generated from ER model
// Maps integer values to display labels for enum fields
// ---------------------------------------------------------------------------

// MaintenanceRequest.priority
export const MaintenanceRequestPriorityMap: Record<string, string> = {
  '0': 'Low',
  '1': 'Medium',
  '2': 'High',
  '3': 'Critical'
};
export const MaintenanceRequestPriorityOptions = [
  { label: 'Low', value: '0' },
  { label: 'Medium', value: '1' },
  { label: 'High', value: '2' },
  { label: 'Critical', value: '3' }
];

// MaintenanceRequest.status
export const MaintenanceRequestStatusMap: Record<string, string> = {
  '0': 'Draft',
  '1': 'PendingLineMechanicApproval',
  '2': 'PendingQualityInspection',
  '3': 'Approved',
  '4': 'Revision',
  '5': 'Completed',
  '6': 'Cancelled'
};
export const MaintenanceRequestStatusOptions = [
  { label: 'Draft', value: '0' },
  { label: 'PendingLineMechanicApproval', value: '1' },
  { label: 'PendingQualityInspection', value: '2' },
  { label: 'Approved', value: '3' },
  { label: 'Revision', value: '4' },
  { label: 'Completed', value: '5' },
  { label: 'Cancelled', value: '6' }
];

// PurchaseRequest.urgencyLevel
export const PurchaseRequestUrgencyLevelMap: Record<string, string> = {
  '0': 'Normal',
  '1': 'Urgent',
  '2': 'Critical'
};
export const PurchaseRequestUrgencyLevelOptions = [
  { label: 'Normal', value: '0' },
  { label: 'Urgent', value: '1' },
  { label: 'Critical', value: '2' }
];

// PurchaseRequest.status
export const PurchaseRequestStatusMap: Record<string, string> = {
  '0': 'Draft',
  '1': 'PendingApproval',
  '2': 'Approved',
  '3': 'Rejected',
  '4': 'Ordered',
  '5': 'Received',
  '6': 'Cancelled'
};
export const PurchaseRequestStatusOptions = [
  { label: 'Draft', value: '0' },
  { label: 'PendingApproval', value: '1' },
  { label: 'Approved', value: '2' },
  { label: 'Rejected', value: '3' },
  { label: 'Ordered', value: '4' },
  { label: 'Received', value: '5' },
  { label: 'Cancelled', value: '6' }
];
