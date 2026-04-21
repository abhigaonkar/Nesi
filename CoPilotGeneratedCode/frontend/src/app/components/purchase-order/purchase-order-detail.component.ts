import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { PurchaseOrderService, PurchaseOrderDto } from '../../../services/purchase-order.service';

@Component({
  selector: 'app-purchase-order-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './purchase-order-detail.component.html',
  styleUrls: ['./purchase-order-detail.component.scss']
})
export class PurchaseOrderDetailComponent implements OnInit {
  purchaseOrder: PurchaseOrderDto | null = null;
  loading: boolean = false;
  error: string = '';
  
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
    
    this.purchaseOrderService.getPurchaseOrderById(id, true, true)
      .subscribe({
        next: (response: any) => {
          this.purchaseOrder = response.data;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load purchase order';
          this.loading = false;
          console.error(err);
        }
      });
  }

  submitPO(): void {
    if (!this.purchaseOrder) return;
    
    if (confirm('Submit this purchase order for approval?')) {
      this.purchaseOrderService.submitPurchaseOrder(this.purchaseOrder.id)
        .subscribe({
          next: () => {
            alert('Purchase order submitted successfully');
            this.loadPurchaseOrder(this.purchaseOrder!.id);
          },
          error: (err) => {
            alert('Failed to submit purchase order');
            console.error(err);
          }
        });
    }
  }

  approvePO(): void {
    if (!this.purchaseOrder) return;
    
    if (confirm('Approve this purchase order?')) {
      this.purchaseOrderService.approvePurchaseOrder(this.purchaseOrder.id)
        .subscribe({
          next: () => {
            alert('Purchase order approved successfully');
            this.loadPurchaseOrder(this.purchaseOrder!.id);
          },
          error: (err) => {
            alert('Failed to approve purchase order');
            console.error(err);
          }
        });
    }
  }

  rejectPO(): void {
    if (!this.purchaseOrder) return;
    
    const reason = prompt('Enter rejection reason:');
    if (reason) {
      this.purchaseOrderService.rejectPurchaseOrder(this.purchaseOrder.id, reason)
        .subscribe({
          next: () => {
            alert('Purchase order rejected');
            this.loadPurchaseOrder(this.purchaseOrder!.id);
          },
          error: (err) => {
            alert('Failed to reject purchase order');
            console.error(err);
          }
        });
    }
  }

  createReceipt(): void {
    if (!this.purchaseOrder) return;
    this.router.navigate(['/purchase-orders', this.purchaseOrder.id, 'receipt', 'new']);
  }

  viewReceipts(): void {
    if (!this.purchaseOrder) return;
    this.router.navigate(['/purchase-orders', this.purchaseOrder.id, 'receipts']);
  }

  goBack(): void {
    this.router.navigate(['/purchase-orders']);
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'draft': return 'badge bg-secondary';
      case 'pending': return 'badge bg-warning';
      case 'approved': return 'badge bg-success';
      case 'rejected': return 'badge bg-danger';
      case 'received': return 'badge bg-info';
      case 'closed': return 'badge bg-dark';
      default: return 'badge bg-secondary';
    }
  }
}
