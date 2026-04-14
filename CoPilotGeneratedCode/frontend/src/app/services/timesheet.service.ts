import { Injectable, inject } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { ApiResponse, PagedResult } from '../models/api-response.model';
import { 
  Timesheet, 
  CreateTimesheetRequest, 
  UpdateTimesheetRequest,
  TimesheetFilterParams 
} from '../models/timesheet.model';

@Injectable({
  providedIn: 'root'
})
export class TimesheetService {
  private apiService = inject(ApiService);

  getTimesheets(filters: TimesheetFilterParams): Observable<ApiResponse<PagedResult<Timesheet>>> {
    let params = new HttpParams();

    if (filters.userId) {
      params = params.set('userId', filters.userId.toString());
    }
    if (filters.startDate) {
      params = params.set('startDate', filters.startDate.toISOString());
    }
    if (filters.endDate) {
      params = params.set('endDate', filters.endDate.toISOString());
    }
    if (filters.status) {
      params = params.set('status', filters.status);
    }
    if (filters.pageNumber) {
      params = params.set('pageNumber', filters.pageNumber.toString());
    }
    if (filters.pageSize) {
      params = params.set('pageSize', filters.pageSize.toString());
    }

    return this.apiService.get<ApiResponse<PagedResult<Timesheet>>>('timesheet', params);
  }

  getTimesheetById(id: number): Observable<ApiResponse<Timesheet>> {
    return this.apiService.get<ApiResponse<Timesheet>>(`timesheet/${id}`);
  }

  createTimesheet(request: CreateTimesheetRequest): Observable<ApiResponse<number>> {
    return this.apiService.post<ApiResponse<number>>('timesheet', request);
  }

  updateTimesheet(request: UpdateTimesheetRequest): Observable<ApiResponse<boolean>> {
    return this.apiService.put<ApiResponse<boolean>>(`timesheet/${request.id}`, request);
  }

  submitTimesheet(id: number): Observable<ApiResponse<boolean>> {
    return this.apiService.post<ApiResponse<boolean>>(`timesheet/${id}/submit`, {});
  }

  approveTimesheet(id: number): Observable<ApiResponse<boolean>> {
    return this.apiService.post<ApiResponse<boolean>>(`timesheet/${id}/approve`, {});
  }

  rejectTimesheet(id: number): Observable<ApiResponse<boolean>> {
    return this.apiService.post<ApiResponse<boolean>>(`timesheet/${id}/reject`, {});
  }
}
