export interface WorkOrder {
  id: number;
  workOrderNumber: string;
  customerId: number;
  customerName: string;
  description: string;
  startDate: Date;
  endDate?: Date;
  isActive: boolean;
  createdAt: Date;
}
