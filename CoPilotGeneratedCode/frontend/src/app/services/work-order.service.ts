import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { ApiResponse } from '../models/api-response.model';
import { WorkOrder } from '../models/work-order.model';

@Injectable({
  providedIn: 'root'
})
export class WorkOrderService {
  private apiService = inject(ApiService);

  getWorkOrders(): Observable<ApiResponse<WorkOrder[]>> {
    return this.apiService.get<ApiResponse<WorkOrder[]>>('workorder');
  }
}
