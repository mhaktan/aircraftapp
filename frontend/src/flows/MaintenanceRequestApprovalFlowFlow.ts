// Auto-generated flow: MaintenanceRequest Approval Flow
// Auto-generated approval flow for MaintenanceRequest. Customize email templates and add conditions as needed.
// Resource: MaintenanceRequest
// Enabled: true
//
// Nodes:
  // trigger: On MaintenanceRequest Submit
  // condition: Status = PendingLineMechanicApproval?
  // approval: MaintenanceRequest Approval
  // action: Send Approval Email
  // trigger: On MaintenanceRequest Approved
  // action: Send Completion Email
//
// Edges:
  // On MaintenanceRequest Submit → Status = PendingLineMechanicApproval?
  // Status = PendingLineMechanicApproval? → MaintenanceRequest Approval (true)
  // MaintenanceRequest Approval → Send Approval Email
  // On MaintenanceRequest Approved → Send Completion Email
//
// This file is for documentation purposes.
// Flow execution is handled by FlowEngine.ts using flowDefinitions.json.

export const FLOW_MAINTENANCEREQUEST_APPROVAL_FLOW_ID = 'flow-MaintenanceRequest-approval';
