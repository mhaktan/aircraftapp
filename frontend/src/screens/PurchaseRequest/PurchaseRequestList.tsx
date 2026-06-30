import React, { useState, useCallback } from 'react';
import { TkButton } from '@takeoff-ui/react';
import { useListQuery } from '../../shared/useListQuery';
import { useDeleteMutation } from '../../shared/useDeleteMutation';
import { DeleteConfirmDialog } from '../../shared/DeleteConfirmDialog';
import { ListPageLayout } from '../../shared/ListPageLayout';
import type { TableColumn } from '../../shared/ListPageLayout';
import { actionColumn } from '../../shared/ActionButtons';
import { PurchaseRequestCreate } from './PurchaseRequestCreate';
import { PurchaseRequestEdit } from './PurchaseRequestEdit';
import { useFlows } from '../../flows/FlowProvider';

// ---------------------------------------------------------------------------
// Types
// ---------------------------------------------------------------------------

type PurchaseRequestRecord = {
  id: string | number;
  requestNumber: string;
  requestedQuantity: number;
  urgencyLevel: string;
  justification: string;
  estimatedUnitPrice?: number;
  requestedBy: string;
  status: string;
  sparePartId: string;
  [key: string]: unknown;
};

// ---------------------------------------------------------------------------
// Column definition — edit this array to add/remove/reorder columns
// ---------------------------------------------------------------------------
//
// Override examples:
//   • Hide a column:     remove its entry from COLUMNS
//   • Add a custom col:  { field: 'fullName', header: 'Full Name', html: (row) => `${row.firstName} ${row.lastName}` }
//   • Enable filtering:  add searchable: true  or  filterType: 'text' | 'checkbox' | 'radio' | 'datepicker'
//   • Custom cell render: html: (row) => `<span style="color:green">${row.status}</span>`
//
// Shared components (src/shared/) can be edited to change behavior globally:
//   • ListPageLayout  — table wrapper, pagination, header layout
//   • ActionButtons   — edit/delete button styles, labels, and behavior
//   • useListQuery    — data fetching, sorting, filtering logic
//   • useDeleteMutation / DeleteConfirmDialog — delete flow
//
// Action buttons override: edit src/shared/ActionButtons.ts DEFAULT_CONFIG
// or pass custom config: actionColumn('id', { hasEdit: true, config: { edit: { label: 'View', style: '...' } } })
//

const COLUMNS: TableColumn[] = [
  { field: 'id', header: 'ID', sortable: true },
  { field: 'requestNumber', header: 'Request Number', sortable: true, searchable: true, filterType: 'text' },
  { field: 'requestedQuantity', header: 'Requested Quantity', sortable: true },
  { field: 'urgencyLevel', header: 'Urgency Level', sortable: true, filterType: 'radio', filterOptions: [{ label: 'Normal', value: '0' }, { label: 'Urgent', value: '1' }, { label: 'Critical', value: '2' }], html: (row: Record<string, unknown>) => { const m: Record<string, string> = {'0': 'Normal', '1': 'Urgent', '2': 'Critical'}; return m[String(row.urgencyLevel ?? '')] ?? String(row.urgencyLevel ?? '\u2014'); } },
  { field: 'justification', header: 'Justification', sortable: true, searchable: true, filterType: 'text' },
  { field: 'estimatedUnitPrice', header: 'Estimated Unit Price', sortable: true },
  { field: 'requestedBy', header: 'Requested By', sortable: true, searchable: true, filterType: 'text' },
  { field: 'status', header: 'Status', sortable: true, filterType: 'radio', filterOptions: [{ label: 'Draft', value: '0' }, { label: 'PendingApproval', value: '1' }, { label: 'Approved', value: '2' }, { label: 'Rejected', value: '3' }, { label: 'Ordered', value: '4' }, { label: 'Received', value: '5' }, { label: 'Cancelled', value: '6' }], html: (row: Record<string, unknown>) => { const m: Record<string, string> = {'0': 'Draft', '1': 'PendingApproval', '2': 'Approved', '3': 'Rejected', '4': 'Ordered', '5': 'Received', '6': 'Cancelled'}; return m[String(row.status ?? '')] ?? String(row.status ?? '\u2014'); } },
  { field: 'sparePartId', header: 'Yedek Parça', sortable: true },
];

// ---------------------------------------------------------------------------
// PurchaseRequestList
// ---------------------------------------------------------------------------

export const PurchaseRequestList: React.FC = () => {
  const list = useListQuery<PurchaseRequestRecord>({ resource: 'PurchaseRequest' });
  const [showCreate, setShowCreate] = useState(false);
  const [editRecord, setEditRecord] = useState<PurchaseRequestRecord | null>(null);
  const [selectedRows, setSelectedRows] = useState<PurchaseRequestRecord[]>([]);
  const { triggerFlows } = useFlows();
  const del = useDeleteMutation('PurchaseRequest', (ids) => { ids.forEach(id => triggerFlows('delete', 'PurchaseRequest', { id })); });


  const columns = [...COLUMNS, ...actionColumn('id', { hasEdit: true, hasDelete: true })];

  const handleCrudAction = useCallback((action: string, id: string) => {
    const row = list.records.find((r) => String(r.id) === id);
    if (!row) return;
    if (action === 'edit') setEditRecord(row);
    if (action === 'delete') del.requestSingleDelete(row.id);
  }, [list.records]);

  return (
    <>
      <ListPageLayout
        title="PurchaseRequest"
        subtitle={Object.keys(list.displayParams).length > 0 ? (
          <div style={{ fontSize: 13, color: '#666', marginTop: 4 }}>
            {Object.entries(list.displayParams).map(([k, v]) => (
              <span key={k} style={{ marginRight: 12 }}>{k}: <strong>{v}</strong></span>
            ))}
          </div>
        ) : undefined}
        records={list.records}
        columns={columns}
        dataKey="id"
        total={list.total}
        loading={list.isLoading}
        page={list.page}
        perPage={list.perPage}
        onPageChange={list.setPage}
        onPerPageChange={list.setPerPage}
        onTableRequest={list.handleTableRequest}
        selectionMode="checkbox"
        selectedRows={selectedRows}
        onSelectionChange={(rows) => setSelectedRows(rows as PurchaseRequestRecord[])}
        onCrudAction={handleCrudAction}
        headerActions={<>
          {selectedRows.length > 0 && (
            <TkButton label={`Delete (${selectedRows.length})`} variant="danger" onTkClick={() => del.requestDelete(selectedRows.map(r => r.id), `${selectedRows.length} record(s)`)} />
          )}

          <TkButton label="+ Create PurchaseRequest" variant="primary" onTkClick={() => setShowCreate(true)} />
        </>}
      />

      {list.isError && (
        <div style={{ padding: '10px 14px', background: '#fff3f3', border: '1px solid #f5c6c6', borderRadius: 6, color: '#c62828', fontSize: 13, marginBottom: 12 }}>
          Failed to load data: {(list.error as Error).message}
        </div>
      )}

      <DeleteConfirmDialog
        visible={!!del.deleteTarget}
        label={del.deleteTarget?.label ?? ''}
        isPending={del.isPending}
        onConfirm={del.confirmDelete}
        onCancel={() => del.setDeleteTarget(null)}
      />
      <PurchaseRequestCreate open={showCreate} onClose={() => setShowCreate(false)} onSuccess={list.invalidate} />
      <PurchaseRequestEdit record={editRecord} onClose={() => setEditRecord(null)} onSuccess={list.invalidate} />

    </>
  );
};

