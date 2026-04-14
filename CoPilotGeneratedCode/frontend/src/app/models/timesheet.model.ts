export enum TimesheetStatus {
  Draft = 0,
  Submitted = 1,
  Approved = 2,
  Rejected = 3
}

export interface Timesheet {
  id: number;
  userId: number;
  userName: string;
  date: Date;
  hours: number;
  payTypeId: number;
  payTypeName: string;
  workOrderId?: number;
  workOrderNumber?: string;
  workOrderDescription?: string;
  jobTypeId?: number;
  jobTypeName?: string;
  notes?: string;
  status: TimesheetStatus;
  createdAt: Date;
  updatedAt?: Date;
}

export interface CreateTimesheetRequest {
  date: Date;
  hours: number;
  payTypeId: number;
  workOrderId?: number;
  jobTypeId?: number;
  notes?: string;
}

export interface UpdateTimesheetRequest {
  id: number;
  date: Date;
  hours: number;
  payTypeId: number;
  workOrderId?: number;
  jobTypeId?: number;
  notes?: string;
}

export interface TimesheetFilterParams {
  userId?: number;
  startDate?: Date;
  endDate?: Date;
  status?: string;
  pageNumber?: number;
  pageSize?: number;
}
