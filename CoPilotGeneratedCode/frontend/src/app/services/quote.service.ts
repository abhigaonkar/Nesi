import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Quote, CreateQuoteRequest } from '../models/quote.model';
import { ApiResponse } from '../models/api-response.model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class QuoteService {
  private apiService = inject(ApiService);

  getQuotes(): Observable<Quote[]> {
    return this.apiService.get<ApiResponse<Quote[]>>('quote')
      .pipe(map(response => response.data || []));
  }

  getQuoteById(id: number): Observable<Quote> {
    return this.apiService.get<ApiResponse<Quote>>(`quote/${id}`)
      .pipe(map(response => response.data!));
  }

  createQuote(request: CreateQuoteRequest): Observable<number> {
    return this.apiService.post<ApiResponse<number>>('quote', request)
      .pipe(map(response => response.data!));
  }

  submitQuote(id: number): Observable<boolean> {
    return this.apiService.post<ApiResponse<boolean>>(`quote/${id}/submit`, {})
      .pipe(map(response => response.data!));
  }

  approveQuote(id: number): Observable<boolean> {
    return this.apiService.post<ApiResponse<boolean>>(`quote/${id}/approve`, {})
      .pipe(map(response => response.data!));
  }

  rejectQuote(id: number, reason: string): Observable<boolean> {
    return this.apiService.post<ApiResponse<boolean>>(`quote/${id}/reject`, { reason })
      .pipe(map(response => response.data!));
  }

  customerApproveQuote(id: number): Observable<boolean> {
    return this.apiService.post<ApiResponse<boolean>>(`quote/${id}/customer-approve`, {})
      .pipe(map(response => response.data!));
  }

  convertToWorkOrder(id: number): Observable<number> {
    return this.apiService.post<ApiResponse<number>>(`quote/${id}/convert-to-workorder`, {})
      .pipe(map(response => response.data!));
  }
}
