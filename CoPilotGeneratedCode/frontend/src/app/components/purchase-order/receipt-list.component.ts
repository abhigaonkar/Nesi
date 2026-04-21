import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { PurchaseOrderService, PurchaseOrderReceiptDto } from '../../services/purchase-order.service';

@Component({
  selector: 'app-receipt-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './receipt-list.component.html',
  styleUrls: ['./receipt-list.component.scss']
})
export class ReceiptListComponent implements OnInit {
  purchaseOrderId!: number;
  receipts: PurchaseOrderReceiptDto[] = [];
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
      this.purchaseOrderId = id;
      this.loadReceipts();
    }
  }

  loadReceipts(): void {
    this.loading = true;
    this.error = '';
    
    this.purchaseOrderService.getReceipts(this.purchaseOrderId)
      .subscribe({
        next: (response: any) => {
          this.receipts = response.data || [];
          this.loading = false;
        },
        error: (err: any) => {
          this.error = 'Failed to load receipts';
          this.loading = false;
          console.error(err);
        }
      });
  }

  viewReceipt(receiptId: number): void {
    this.router.navigate(['/purchase-orders', this.purchaseOrderId, 'receipts', receiptId]);
  }

  createNewReceipt(): void {
    this.router.navigate(['/purchase-orders', this.purchaseOrderId, 'receipt', 'new']);
  }

  goBack(): void {
    this.router.navigate(['/purchase-orders', this.purchaseOrderId]);
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'draft': return 'badge bg-secondary';
      case 'confirmed': return 'badge bg-success';
      default: return 'badge bg-secondary';
    }
  }
}
