import React, { useState } from 'react';
import { useMutation } from '@tanstack/react-query';
import { TkButton, TkInput } from '@takeoff-ui/react';
import { dataProvider } from '../../dataProvider';
import { overlayStyle, modalStyle } from '../../styles';
import { useFlows } from '../../flows/FlowProvider';

type SparePartRecord = {
  id: string | number;
  partNumber: string;
  partName: string;
  description?: string;
  unitOfMeasure: string;
  stockQuantity: number;
  minStockLevel: number;
  unitPrice?: number;
  isActive: boolean;
};

interface SparePartCreateProps {
  open: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const SparePartCreate: React.FC<SparePartCreateProps> = ({ open, onClose, onSuccess }) => {
  const [form, setForm] = useState<Partial<SparePartRecord>>({});
  const setField = (name: string, value: unknown) => setForm((p) => ({ ...p, [name]: value }));
  const { triggerFlows } = useFlows();

  const mutation = useMutation({
    mutationFn: (values: Partial<SparePartRecord>) => dataProvider.create('SparePart', values),
    onSuccess: (_data, values) => { triggerFlows('create', 'SparePart', values as Record<string, unknown>); onSuccess(); onClose(); setForm({}); },
    onError: (err: Error) => { window.dispatchEvent(new CustomEvent('app-toast', { detail: { type: 'error', message: err.message } })); },
  });

  if (!open) return null;

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ padding: '20px 28px 0', flexShrink: 0, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h2 style={{ margin: 0, fontSize: 18, fontWeight: 700 }}>Create SparePart</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: 20, cursor: 'pointer', color: '#666', padding: '4px 8px', borderRadius: 4 }} onMouseOver={(e) => (e.currentTarget.style.color = '#333')} onMouseOut={(e) => (e.currentTarget.style.color = '#666')}>✕</button>
        </div>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(form); }} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
          <div style={{ flex: 1, overflowY: 'auto', padding: '20px 28px' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div>
                  <TkInput mode="text" label="Part Number *" value={String(form.partNumber ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('partNumber', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Part Name *" value={String(form.partName ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('partName', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Description" value={String(form.description ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('description', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Unit Of Measure *" value={String(form.unitOfMeasure ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('unitOfMeasure', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Stock Quantity *" value={String(form.stockQuantity ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('stockQuantity', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Min Stock Level *" value={String(form.minStockLevel ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('minStockLevel', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Unit Price" value={String(form.unitPrice ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('unitPrice', Number(v)))(e.detail)} />
                </div>
                <div>
                  <label style={{ display: 'flex', alignItems: 'center', gap: 8, fontSize: 14 }}>
                    <input type="checkbox" checked={!!form.isActive} onChange={(e) => ((v) => setField('isActive', v))(e.target.checked)} style={{ width: 16, height: 16 }} />
                    Is Active *
                  </label>
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
