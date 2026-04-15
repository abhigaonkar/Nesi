import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Quote, CreateQuoteRequest } from '../models/quote.model';
import { ApiResponse } from '../models/api-response.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class QuoteService {
  private apiUrl = `${environment.apiUrl}/quote`;

  constructor(private http: HttpClient) {}

  getQuotes(): Observable<Quote[]> {
    return this.http.get<ApiResponse<Quote[]>>(this.apiUrl)
      .pipe(map(response => response.data || []));
  }

  getQuoteById(id: number): Observable<Quote> {
    return this.http.get<ApiResponse<Quote>>(`${this.apiUrl}/${id}`)
      .pipe(map(response => response.data!));
  }

  createQuote(request: CreateQuoteRequest): Observable<number> {
    return this.http.post<ApiResponse<number>>(this.apiUrl, request)
      .pipe(map(response => response.data!));
  }

  submitQuote(id: number): Observable<boolean> {
    return this.http.post<ApiResponse<boolean>>(`${this.apiUrl}/${id}/submit`, {})
      .pipe(map(response => response.data!));
  }

  approveQuote(id: number): Observable<boolean> {
    return this.http.post<ApiResponse<boolean>>(`${this.apiUrl}/${id}/approve`, {})
      .pipe(map(response => response.data!));
  }

  rejectQuote(id: number, reason: string): Observable<boolean> {
    return this.http.post<ApiResponse<boolean>>(`${this.apiUrl}/${id}/reject`, { reason })
      .pipe(map(response => response.data!));
  }
}
