import { Injectable, inject } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface VendorDto {
  id: number;
  vendorNumber: string;
  companyName: string;
  contactName?: string;
  email?: string;
  phone?: string;
  fax?: string;
  website?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
  taxId?: string;
  accountNumber?: string;
  status: string;
  paymentTermsDays?: number;
  creditLimit?: number;
  rating?: number;
  notes?: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt?: Date;
}

export interface CreateVendorCommand {
  companyName: string;
  contactName?: string;
  email?: string;
  phone?: string;
  fax?: string;
  website?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
  taxId?: string;
  accountNumber?: string;
  paymentTermsDays?: number;
  creditLimit?: number;
  rating?: number;
  notes?: string;
  isActive?: boolean;
}

export interface UpdateVendorCommand {
  id: number;
  companyName: string;
  contactName?: string;
  email?: string;
  phone?: string;
  fax?: string;
  website?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
  taxId?: string;
  accountNumber?: string;
  paymentTermsDays?: number;
  creditLimit?: number;
  rating?: number;
  notes?: string;
  isActive?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class VendorService {
  private apiService = inject(ApiService);

  getVendors(activeOnly: boolean = true, pageNumber: number = 1, pageSize: number = 20): Observable<any> {
    let params = new HttpParams()
      .set('activeOnly', activeOnly.toString())
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    
    return this.apiService.get<any>('vendor', params);
  }

  getVendorById(id: number): Observable<VendorDto> {
    return this.apiService.get<VendorDto>(`vendor/${id}`);
  }

  createVendor(vendor: CreateVendorCommand): Observable<number> {
    return this.apiService.post<number>('vendor', vendor);
  }

  updateVendor(vendor: UpdateVendorCommand): Observable<boolean> {
    return this.apiService.put<boolean>(`vendor/${vendor.id}`, vendor);
  }
}
