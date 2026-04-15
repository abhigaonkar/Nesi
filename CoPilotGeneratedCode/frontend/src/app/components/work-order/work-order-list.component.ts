import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { WorkOrderService } from '../../services/work-order.service';
import { WorkOrder, WorkOrderStatus } from '../../models/work-order.model';

@Component({
  selector: 'app-work-order-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './work-order-list.component.html',
  styleUrls: ['./work-order-list.component.scss']
})
export class WorkOrderListComponent implements OnInit {
  workOrders: WorkOrder[] = [];
  loading = false;
  error = '';
  
  WorkOrderStatus = WorkOrderStatus;

  constructor(
    private workOrderService: WorkOrderService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadWorkOrders();
  }

  loadWorkOrders(): void {
    this.loading = true;
    this.error = '';
    
    this.workOrderService.getWorkOrders().subscribe({
      next: (workOrders) => {
        this.workOrders = workOrders;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load work orders';
        this.loading = false;
        console.error('Error loading work orders:', err);
      }
    });
  }

  viewWorkOrder(id: number): void {
    this.router.navigate(['/work-orders', id]);
  }

  getStatusLabel(status: WorkOrderStatus): string {
    return WorkOrderStatus[status];
  }

  getStatusClass(status: WorkOrderStatus): string {
    switch (status) {
      case WorkOrderStatus.Created:
        return 'status-created';
      case WorkOrderStatus.Assigned:
        return 'status-assigned';
      case WorkOrderStatus.InProgress:
        return 'status-in-progress';
      case WorkOrderStatus.Complete:
        return 'status-complete';
      case WorkOrderStatus.Cancelled:
        return 'status-cancelled';
      case WorkOrderStatus.Closed:
        return 'status-closed';
      default:
        return '';
    }
  }
}
