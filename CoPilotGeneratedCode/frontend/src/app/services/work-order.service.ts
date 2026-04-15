import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';
import { ApiResponse } from '../models/api-response.model';
import { 
  WorkOrder, 
  AssignProjectManagerRequest,
  AssignTechnicianRequest,
  AddMaterialRequest,
  CompleteWorkOrderRequest,
  GenerateInvoiceRequest
} from '../models/work-order.model';

@Injectable({
  providedIn: 'root'
})
export class WorkOrderService {
  private apiService = inject(ApiService);

  getWorkOrders(): Observable<WorkOrder[]> {
    return this.apiService.get<ApiResponse<WorkOrder[]>>('workorder')
      .pipe(map(response => response.data || []));
  }

  getWorkOrderById(id: number): Observable<WorkOrder> {
    return this.apiService.get<ApiResponse<WorkOrder>>(`workorder/${id}`)
      .pipe(map(response => response.data!));
  }

  assignProjectManager(id: number, request: AssignProjectManagerRequest): Observable<boolean> {
    return this.apiService.post<ApiResponse<boolean>>(`workorder/${id}/assign-manager`, request)
      .pipe(map(response => response.data!));
  }

  assignTechnician(id: number, request: AssignTechnicianRequest): Observable<number> {
    return this.apiService.post<ApiResponse<number>>(`workorder/${id}/assign-technician`, request)
      .pipe(map(response => response.data!));
  }

  addMaterial(id: number, request: AddMaterialRequest): Observable<number> {
    return this.apiService.post<ApiResponse<number>>(`workorder/${id}/materials`, request)
      .pipe(map(response => response.data!));
  }

  completeWorkOrder(id: number, request: CompleteWorkOrderRequest): Observable<boolean> {
    return this.apiService.post<ApiResponse<boolean>>(`workorder/${id}/complete`, request)
      .pipe(map(response => response.data!));
  }

  generateInvoice(id: number, request: GenerateInvoiceRequest): Observable<boolean> {
    return this.apiService.post<ApiResponse<boolean>>(`workorder/${id}/invoice`, request)
      .pipe(map(response => response.data!));
  }
}

