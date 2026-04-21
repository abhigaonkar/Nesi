import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { PurchaseOrderService, PurchaseOrderDto } from '../../../services/purchase-order.service';

@Component({
  selector: 'app-purchase-order-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './purchase-order-list.component.html',
  styleUrls: ['./purchase-order-list.component.scss']
})
export class PurchaseOrderListComponent implements OnInit {
  purchaseOrders: PurchaseOrderDto[] = [];
  loading: boolean = false;
  error: string = '';
  currentPage: number = 1;
  pageSize: number = 20;
  totalCount: number = 0;

  constructor(
    private purchaseOrderService: PurchaseOrderService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadPurchaseOrders();
  }

  loadPurchaseOrders(): void {
    this.loading = true;
    this.error = '';
    
    this.purchaseOrderService.getPurchaseOrders(undefined, undefined, undefined, this.currentPage, this.pageSize)
      .subscribe({
        next: (response: any) => {
          this.purchaseOrders = response.data.purchaseOrders;
          this.totalCount = response.data.totalCount;
          this.loading = false;
        },
        error: (err: any) => {
          this.error = 'Failed to load purchase orders';
          this.loading = false;
          console.error(err);
        }
      });
  }

  viewDetails(id: number): void {
    this.router.navigate(['/purchase-orders', id]);
  }

  createNew(): void {
    this.router.navigate(['/purchase-orders', 'new']);
  }

  submitPO(id: number): void {
    this.purchaseOrderService.submitPurchaseOrder(id).subscribe({
      next: () => {
        alert('Purchase order submitted successfully');
        this.loadPurchaseOrders();
      },
      error: (err: any) => {
        alert('Failed to submit purchase order');
        console.error(err);
      }
    });
  }

  approvePO(id: number): void {
    this.purchaseOrderService.approvePurchaseOrder(id).subscribe({
      next: () => {
        alert('Purchase order approved successfully');
        this.loadPurchaseOrders();
      },
      error: (err: any) => {
        alert('Failed to approve purchase order');
        console.error(err);
      }
    });
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
