import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PurchaseOrderService, PurchaseOrderDto, CreateReceiptCommand } from '../../services/purchase-order.service';

@Component({
  selector: 'app-receipt-create',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './receipt-create.component.html',
  styleUrls: ['./receipt-create.component.scss']
})
export class ReceiptCreateComponent implements OnInit {
  purchaseOrder: PurchaseOrderDto | null = null;
  loading: boolean = false;
  saving: boolean = false;
  error: string = '';

  receiptDate: string = new Date().toISOString().split('T')[0];
  packingSlipNumber: string = '';
  notes: string = '';

  receiptItems: Array<{
    lineItemId: number;
    description: string;
    orderedQty: number;
    remainingQty: number;
    receivedQty: number;
    condition: string;
    hasDiscrepancy: boolean;
    discrepancyReason: string;
    itemNotes: string;
  }> = [];

  constructor(
    private purchaseOrderService: PurchaseOrderService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.loadPurchaseOrder(id);
    }
  }

  loadPurchaseOrder(id: number): void {
    this.loading = true;
    this.error = '';
    
    this.purchaseOrderService.getPurchaseOrderById(id, true, false)
      .subscribe({
        next: (response: any) => {
          this.purchaseOrder = response.data;
          this.initializeReceiptItems();
          this.loading = false;
        },
        error: (err: any) => {
          this.error = 'Failed to load purchase order';
          this.loading = false;
          console.error(err);
        }
      });
  }

  initializeReceiptItems(): void {
    if (!this.purchaseOrder?.lineItems) return;

    this.receiptItems = this.purchaseOrder.lineItems
      .filter((item: any) => item.remainingQuantity > 0)
      .map((item: any) => ({
        lineItemId: item.id,
        description: item.description,
        orderedQty: item.quantity,
        remainingQty: item.remainingQuantity,
        receivedQty: item.remainingQuantity, // Default to full remaining
        condition: 'Good',
        hasDiscrepancy: false,
        discrepancyReason: '',
        itemNotes: ''
      }));
  }

  onDiscrepancyChange(item: any): void {
    if (!item.hasDiscrepancy) {
      item.discrepancyReason = '';
    }
  }

  createReceipt(): void {
    if (!this.purchaseOrder) return;

    // Validate
    if (this.receiptItems.length === 0) {
      alert('No items to receive');
      return;
    }

    if (this.receiptItems.some(item => item.receivedQty <= 0)) {
      alert('All items must have a quantity greater than 0');
      return;
    }

    if (this.receiptItems.some(item => item.hasDiscrepancy && !item.discrepancyReason)) {
      alert('Please provide a reason for all discrepancies');
      return;
    }

    this.saving = true;
    this.error = '';

    const command: CreateReceiptCommand = {
      purchaseOrderId: this.purchaseOrder.id,
      receivedDate: new Date(this.receiptDate),
      packingSlipNumber: this.packingSlipNumber || undefined,
      notes: this.notes || undefined,
      items: this.receiptItems.map(item => ({
        purchaseOrderLineItemId: item.lineItemId,
        quantityReceived: item.receivedQty,
        condition: item.condition,
        notes: item.itemNotes || undefined,
        hasDiscrepancy: item.hasDiscrepancy,
        discrepancyReason: item.discrepancyReason || undefined
      }))
    };

    this.purchaseOrderService.createReceipt(this.purchaseOrder.id, command)
      .subscribe({
        next: () => {
          alert('Receipt created successfully');
          this.router.navigate(['/purchase-orders', this.purchaseOrder!.id]);
        },
        error: (err: any) => {
          this.error = 'Failed to create receipt';
          this.saving = false;
          console.error(err);
        }
      });
  }

  cancel(): void {
    if (this.purchaseOrder) {
      this.router.navigate(['/purchase-orders', this.purchaseOrder.id]);
    }
  }
}
