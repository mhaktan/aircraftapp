import React, { useState } from 'react';
import { useMutation } from '@tanstack/react-query';
import { TkButton, TkInput, TkSelect } from '@takeoff-ui/react';
import { dataProvider } from '../../dataProvider';
import { overlayStyle, modalStyle } from '../../styles';
import { LookupSelect } from '../../shared/LookupSelect';
import { useFlows } from '../../flows/FlowProvider';

type MaintenanceRequestRecord = {
  id: string | number;
  requestNumber: string;
  description: string;
  priority: string;
  requestedBy: string;
  lineMechanicId?: string;
  qualityInspectorId?: number;
  estimatedDuration?: number;
  actualDuration?: number;
  workPerformed?: string;
  revisionNote?: string;
  status: string;
  aircraftId: string;
  maintenanceTypeId: string;
};

interface MaintenanceRequestCreateProps {
  open: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const MaintenanceRequestCreate: React.FC<MaintenanceRequestCreateProps> = ({ open, onClose, onSuccess }) => {
  const [form, setForm] = useState<Partial<MaintenanceRequestRecord>>({});
  const setField = (name: string, value: unknown) => setForm((p) => ({ ...p, [name]: value }));
  const { triggerFlows } = useFlows();

  const mutation = useMutation({
    mutationFn: (values: Partial<MaintenanceRequestRecord>) => dataProvider.create('MaintenanceRequest', values),
    onSuccess: (_data, values) => { triggerFlows('create', 'MaintenanceRequest', values as Record<string, unknown>); onSuccess(); onClose(); setForm({}); },
    onError: (err: Error) => { window.dispatchEvent(new CustomEvent('app-toast', { detail: { type: 'error', message: err.message } })); },
  });

  if (!open) return null;

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ padding: '20px 28px 0', flexShrink: 0, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h2 style={{ margin: 0, fontSize: 18, fontWeight: 700 }}>Create MaintenanceRequest</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: 20, cursor: 'pointer', color: '#666', padding: '4px 8px', borderRadius: 4 }} onMouseOver={(e) => (e.currentTarget.style.color = '#333')} onMouseOut={(e) => (e.currentTarget.style.color = '#666')}>✕</button>
        </div>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(form); }} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
          <div style={{ flex: 1, overflowY: 'auto', padding: '20px 28px' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div>
                  <TkInput mode="text" label="Request Number *" value={String(form.requestNumber ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('requestNumber', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Description *" value={String(form.description ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('description', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Priority *" value={String(form.priority ?? '')} onChange={(v) => setField('priority', v ? Number(v) : null)} searchable={false} options={[{ label: 'Low', value: '0' }, { label: 'Medium', value: '1' }, { label: 'High', value: '2' }, { label: 'Critical', value: '3' }]} />
                </div>
                <div>
                  <TkInput mode="text" label="Requested By *" value={String(form.requestedBy ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('requestedBy', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Line Mechanic Id" resource="User" value={String(form.lineMechanicId ?? '')} onChange={(v) => setField('lineMechanicId', v)} displayField="userName" defaultFilter={{ 'roleIds': '1' }} />
                </div>
                <div>
                  <TkInput mode="number" label="Quality Inspector Id" value={String(form.qualityInspectorId ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('qualityInspectorId', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Estimated Duration" value={String(form.estimatedDuration ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('estimatedDuration', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Actual Duration" value={String(form.actualDuration ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('actualDuration', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Work Performed" value={String(form.workPerformed ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('workPerformed', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Revision Note" value={String(form.revisionNote ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('revisionNote', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Status *" value={String(form.status ?? '')} onChange={(v) => setField('status', v ? Number(v) : null)} searchable={false} options={[{ label: 'Draft', value: '0' }, { label: 'PendingLineMechanicApproval', value: '1' }, { label: 'PendingQualityInspection', value: '2' }, { label: 'Approved', value: '3' }, { label: 'Revision', value: '4' }, { label: 'Completed', value: '5' }, { label: 'Cancelled', value: '6' }]} />
                </div>
                <div>
                  <LookupSelect label="Uçak *" resource="Aircraft" value={String(form.aircraftId ?? '')} onChange={(v) => setField('aircraftId', v)} displayField="registrationNumber" />
                </div>
                <div>
                  <LookupSelect label="Bakım Türü *" resource="MaintenanceType" value={String(form.maintenanceTypeId ?? '')} onChange={(v) => setField('maintenanceTypeId', v)} />
                </div>
            </div>
          </div>
          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8, padding: '16px 28px', borderTop: '1px solid #e8e8e8', flexShrink: 0, background: '#fff' }}>
            <TkButton label="Cancel" variant="secondary" onTkClick={onClose} />
            <TkButton label={mutation.isPending ? 'Saving…' : 'Create'} variant="primary" mode="submit" disabled={mutation.isPending} />
          </div>
        </form>
      </div>
    </div>
  );
};
