export enum WorkOrderStatus {
  Created = 0,
  Assigned = 1,
  InProgress = 2,
  Complete = 3,
  Cancelled = 4,
  Closed = 5
}

export interface WorkOrder {
  id: number;
  workOrderNumber: string;
  customerId: number;
  customerName: string;
  description: string;
  status: WorkOrderStatus;
  priority: string;
  
  projectManagerId?: number;
  projectManagerName?: string;
  scheduledStartDate?: Date;
  scheduledEndDate?: Date;
  actualStartDate?: Date;
  actualCompletionDate?: Date;
  
  estimatedCost: number;
  actualCost: number;
  
  completedBy?: number;
  completedByName?: string;
  completionNotes?: string;
  
  invoiceGenerated: boolean;
  invoiceAmount?: number;
  invoiceDate?: Date;
  paymentReceived: boolean;
  paymentDate?: Date;
  
  quoteId?: number;
  quoteNumber?: string;
  
  startDate: Date;
  endDate?: Date;
  isActive: boolean;
  createdAt: Date;
  createdBy?: string;
}

export interface WorkOrderAssignment {
  id: number;
  workOrderId: number;
  technicianId: number;
  technicianName: string;
  assignedBy: number;
  assignedByName: string;
  assignedAt: Date;
  role?: string;
  notes?: string;
}

export interface Material {
  id: number;
  workOrderId: number;
  partNumber: string;
  description: string;
  quantity: number;
  unitCost: number;
  totalCost: number;
  purchaseOrderNumber?: string;
  supplier?: string;
  receivedDate?: Date;
  notes?: string;
  createdAt: Date;
}

export interface AssignProjectManagerRequest {
  projectManagerId: number;
  scheduledStartDate?: Date;
  scheduledEndDate?: Date;
}

export interface AssignTechnicianRequest {
  technicianId: number;
  role?: string;
  notes?: string;
}

export interface AddMaterialRequest {
  partNumber: string;
  description: string;
  quantity: number;
  unitCost: number;
  purchaseOrderNumber?: string;
  supplier?: string;
  notes?: string;
}

export interface CompleteWorkOrderRequest {
  completionNotes?: string;
}

export interface GenerateInvoiceRequest {
  invoiceAmount: number;
}

