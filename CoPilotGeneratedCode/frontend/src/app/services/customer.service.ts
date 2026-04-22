import { Injectable, inject } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { 
  Customer, 
  CreateCustomerRequest, 
  UpdateCustomerRequest,
  AddCustomerAddressRequest,
  AddCustomerContactRequest,
  AddCustomerNoteRequest,
  GetCustomersResult
} from '../models/customer.model';
import { ApiResponse } from '../models/api-response.model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private apiService = inject(ApiService);

  getCustomers(
    activeOnly: boolean = true,
    businessUnitId?: number,
    accountManagerId?: number,
    pageNumber: number = 1,
    pageSize: number = 20
  ): Observable<GetCustomersResult> {
    let params = new HttpParams()
      .set('activeOnly', activeOnly.toString())
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    
    if (businessUnitId) {
      params = params.set('businessUnitId', businessUnitId.toString());
    }
    if (accountManagerId) {
      params = params.set('accountManagerId', accountManagerId.toString());
    }

    return this.apiService.get<ApiResponse<GetCustomersResult>>('customer', params)
      .pipe(map(response => response.data!));
  }

  getCustomerById(id: number, includeDetails: boolean = false): Observable<Customer> {
    let params = new HttpParams();
    if (includeDetails) {
      params = params.set('includeDetails', 'true');
    }

    return this.apiService.get<ApiResponse<Customer>>(`customer/${id}`, params)
      .pipe(map(response => response.data!));
  }

  searchCustomers(searchTerm: string): Observable<Customer[]> {
    const params = new HttpParams().set('searchTerm', searchTerm);
    return this.apiService.get<ApiResponse<Customer[]>>('customer/search', params)
      .pipe(map(response => response.data || []));
  }

  createCustomer(request: CreateCustomerRequest): Observable<number> {
    return this.apiService.post<ApiResponse<number>>('customer', request)
      .pipe(map(response => response.data!));
  }

  updateCustomer(id: number, request: UpdateCustomerRequest): Observable<boolean> {
    return this.apiService.put<ApiResponse<boolean>>(`customer/${id}`, request)
      .pipe(map(response => response.data!));
  }

  deleteCustomer(id: number): Observable<boolean> {
    return this.apiService.delete<ApiResponse<boolean>>(`customer/${id}`)
      .pipe(map(response => response.data!));
  }

  activateCustomer(id: number): Observable<boolean> {
    return this.apiService.post<ApiResponse<boolean>>(`customer/${id}/activate`, {})
      .pipe(map(response => response.data!));
  }

  addAddress(request: AddCustomerAddressRequest): Observable<number> {
    return this.apiService.post<ApiResponse<number>>(
      `customer/${request.customerId}/addresses`, 
      request
    ).pipe(map(response => response.data!));
  }

  addContact(request: AddCustomerContactRequest): Observable<number> {
    return this.apiService.post<ApiResponse<number>>(
      `customer/${request.customerId}/contacts`, 
      request
    ).pipe(map(response => response.data!));
  }

  addNote(request: AddCustomerNoteRequest): Observable<number> {
    return this.apiService.post<ApiResponse<number>>(
      `customer/${request.customerId}/notes`, 
      request
    ).pipe(map(response => response.data!));
  }
}
