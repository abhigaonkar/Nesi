import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { WorkOrderService } from '../../services/work-order.service';
import { 
  WorkOrder, 
  WorkOrderStatus,
  AssignProjectManagerRequest,
  AssignTechnicianRequest,
  AddMaterialRequest,
  CompleteWorkOrderRequest,
  GenerateInvoiceRequest
} from '../../models/work-order.model';

@Component({
  selector: 'app-work-order-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './work-order-detail.component.html',
  styleUrls: ['./work-order-detail.component.scss']
})
export class WorkOrderDetailComponent implements OnInit {
  workOrder?: WorkOrder;
  loading = false;
  error = '';
  actionLoading = false;

  // Modal states
  showAssignPMModal = false;
  showAssignTechModal = false;
  showAddMaterialModal = false;
  showCompleteModal = false;
  showInvoiceModal = false;

  // Form fields
  projectManagerId = 0;
  scheduledStartDate = '';
  scheduledEndDate = '';
  
  technicianId = 0;
  technicianRole = '';
  technicianNotes = '';
  
  partNumber = '';
  materialDescription = '';
  quantity = 1;
  unitCost = 0;
  poNumber = '';
  supplier = '';
  materialNotes = '';
  
  completionNotes = '';
  invoiceAmount = 0;

  WorkOrderStatus = WorkOrderStatus;

  // Mock data (would come from API in real app)
  projectManagers = [
    { id: 1, name: 'John Doe' },
    { id: 2, name: 'Jane Smith' },
    { id: 3, name: 'Mike Johnson' }
  ];

  technicians = [
    { id: 4, name: 'Bob Builder' },
    { id: 5, name: 'Alice Technician' },
    { id: 6, name: 'Charlie Expert' }
  ];

  constructor(
    private workOrderService: WorkOrderService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) {
        this.loadWorkOrder(id);
      }
    });
  }

  loadWorkOrder(id: number): void {
    this.loading = true;
    this.error = '';
    
    this.workOrderService.getWorkOrderById(id).subscribe({
      next: (workOrder) => {
        this.workOrder = workOrder;
        this.invoiceAmount = workOrder.estimatedCost || 0;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load work order';
        this.loading = false;
        console.error('Error loading work order:', err);
      }
    });
  }

  openAssignPMModal(): void {
    this.showAssignPMModal = true;
  }

  closeAssignPMModal(): void {
    this.showAssignPMModal = false;
    this.projectManagerId = 0;
    this.scheduledStartDate = '';
    this.scheduledEndDate = '';
  }

  assignProjectManager(): void {
    if (!this.workOrder || !this.projectManagerId) return;

    const request: AssignProjectManagerRequest = {
      projectManagerId: this.projectManagerId,
      scheduledStartDate: this.scheduledStartDate ? new Date(this.scheduledStartDate) : undefined,
      scheduledEndDate: this.scheduledEndDate ? new Date(this.scheduledEndDate) : undefined
    };

    this.actionLoading = true;
    this.workOrderService.assignProjectManager(this.workOrder.id, request).subscribe({
      next: () => {
        this.actionLoading = false;
        this.closeAssignPMModal();
        this.loadWorkOrder(this.workOrder!.id);
      },
      error: (err) => {
        this.error = 'Failed to assign project manager';
        this.actionLoading = false;
        console.error('Error assigning PM:', err);
      }
    });
  }

  openAssignTechModal(): void {
    this.showAssignTechModal = true;
  }

  closeAssignTechModal(): void {
    this.showAssignTechModal = false;
    this.technicianId = 0;
    this.technicianRole = '';
    this.technicianNotes = '';
  }

  assignTechnician(): void {
    if (!this.workOrder || !this.technicianId) return;

    const request: AssignTechnicianRequest = {
      technicianId: this.technicianId,
      role: this.technicianRole || undefined,
      notes: this.technicianNotes || undefined
    };

    this.actionLoading = true;
    this.workOrderService.assignTechnician(this.workOrder.id, request).subscribe({
      next: () => {
        this.actionLoading = false;
        this.closeAssignTechModal();
        this.loadWorkOrder(this.workOrder!.id);
      },
      error: (err) => {
        this.error = 'Failed to assign technician';
        this.actionLoading = false;
        console.error('Error assigning technician:', err);
      }
    });
  }

  openAddMaterialModal(): void {
    this.showAddMaterialModal = true;
  }

  closeAddMaterialModal(): void {
    this.showAddMaterialModal = false;
    this.partNumber = '';
    this.materialDescription = '';
    this.quantity = 1;
    this.unitCost = 0;
    this.poNumber = '';
    this.supplier = '';
    this.materialNotes = '';
  }

  addMaterial(): void {
    if (!this.workOrder || !this.partNumber || !this.materialDescription) return;

    const request: AddMaterialRequest = {
      partNumber: this.partNumber,
      description: this.materialDescription,
      quantity: this.quantity,
      unitCost: this.unitCost,
      purchaseOrderNumber: this.poNumber || undefined,
      supplier: this.supplier || undefined,
      notes: this.materialNotes || undefined
    };

    this.actionLoading = true;
    this.workOrderService.addMaterial(this.workOrder.id, request).subscribe({
      next: () => {
        this.actionLoading = false;
        this.closeAddMaterialModal();
        this.loadWorkOrder(this.workOrder!.id);
      },
      error: (err) => {
        this.error = 'Failed to add material';
        this.actionLoading = false;
        console.error('Error adding material:', err);
      }
    });
  }

  openCompleteModal(): void {
    this.showCompleteModal = true;
  }

  closeCompleteModal(): void {
    this.showCompleteModal = false;
    this.completionNotes = '';
  }

  completeWorkOrder(): void {
    if (!this.workOrder) return;

    const request: CompleteWorkOrderRequest = {
      completionNotes: this.completionNotes || undefined
    };

    this.actionLoading = true;
    this.workOrderService.completeWorkOrder(this.workOrder.id, request).subscribe({
      next: () => {
        this.actionLoading = false;
        this.closeCompleteModal();
        this.loadWorkOrder(this.workOrder!.id);
      },
      error: (err) => {
        this.error = 'Failed to complete work order';
        this.actionLoading = false;
        console.error('Error completing work order:', err);
      }
    });
  }

  openInvoiceModal(): void {
    this.showInvoiceModal = true;
  }

  closeInvoiceModal(): void {
    this.showInvoiceModal = false;
  }

  generateInvoice(): void {
    if (!this.workOrder || this.invoiceAmount <= 0) return;

    const request: GenerateInvoiceRequest = {
      invoiceAmount: this.invoiceAmount
    };

    this.actionLoading = true;
    this.workOrderService.generateInvoice(this.workOrder.id, request).subscribe({
      next: () => {
        this.actionLoading = false;
        this.closeInvoiceModal();
        this.loadWorkOrder(this.workOrder!.id);
      },
      error: (err) => {
        this.error = 'Failed to generate invoice';
        this.actionLoading = false;
        console.error('Error generating invoice:', err);
      }
    });
  }

  backToList(): void {
    this.router.navigate(['/work-orders']);
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

  canAssignPM(): boolean {
    return this.workOrder?.status === WorkOrderStatus.Created || 
           this.workOrder?.status === WorkOrderStatus.Assigned;
  }

  canAssignTech(): boolean {
    return this.workOrder?.status !== WorkOrderStatus.Complete &&
           this.workOrder?.status !== WorkOrderStatus.Cancelled &&
           this.workOrder?.status !== WorkOrderStatus.Closed;
  }

  canAddMaterial(): boolean {
    return this.workOrder?.status !== WorkOrderStatus.Complete &&
           this.workOrder?.status !== WorkOrderStatus.Cancelled &&
           this.workOrder?.status !== WorkOrderStatus.Closed;
  }

  canComplete(): boolean {
    return this.workOrder?.status === WorkOrderStatus.InProgress;
  }

  canGenerateInvoice(): boolean {
    return this.workOrder?.status === WorkOrderStatus.Complete &&
           !this.workOrder?.invoiceGenerated;
  }
}
