import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

// Report DTOs
export interface JobCostReportDto {
  workOrderId: number;
  workOrderNumber: string;
  customerName: string;
  startDate: Date;
  completionDate?: Date;
  status: string;
  laborCost: number;
  materialCost: number;
  overheadCost: number;
  totalCost: number;
  quotedAmount: number;
  actualAmount: number;
  profitMargin: number;
  profitPercentage: number;
}

export interface IncomeStatementDto {
  periodStart: Date;
  periodEnd: Date;
  revenue: number;
  costOfGoodsSold: number;
  grossProfit: number;
  operatingExpenses: {
    labor: number;
    materials: number;
    overhead: number;
    administrative: number;
    total: number;
  };
  operatingIncome: number;
  otherIncome: number;
  otherExpenses: number;
  netIncome: number;
}

export interface BalanceSheetDto {
  asOfDate: Date;
  assets: {
    currentAssets: {
      cash: number;
      accountsReceivable: number;
      inventory: number;
      total: number;
    };
    fixedAssets: {
      equipment: number;
      vehicles: number;
      accumulatedDepreciation: number;
      total: number;
    };
    totalAssets: number;
  };
  liabilities: {
    currentLiabilities: {
      accountsPayable: number;
      accruedExpenses: number;
      total: number;
    };
    longTermLiabilities: {
      loansPayable: number;
      total: number;
    };
    totalLiabilities: number;
  };
  equity: {
    retainedEarnings: number;
    currentPeriodIncome: number;
    totalEquity: number;
  };
}

export interface ARAgingDto {
  customerId: number;
  customerName: string;
  totalOutstanding: number;
  current: number;        // 0-30 days
  days31to60: number;
  days61to90: number;
  over90Days: number;
}

export interface CustomerRateAnalysisDto {
  customerId: number;
  customerName: string;
  totalJobs: number;
  totalRevenue: number;
  totalCost: number;
  averageProfit: number;
  profitMargin: number;
  averageJobValue: number;
  lastJobDate: Date;
}

export interface InventoryUsageDto {
  materialId: number;
  materialName: string;
  partNumber?: string;
  quantityUsed: number;
  unitOfMeasure: string;
  unitCost: number;
  totalCost: number;
  topWorkOrders: { workOrderNumber: string; quantity: number }[];
}

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = 'http://localhost:5000/api/Report';

  constructor(private http: HttpClient, private apiService: ApiService) {}

  getJobCostReport(
    startDate?: Date,
    endDate?: Date,
    customerId?: number,
    status?: string
  ): Observable<any> {
    let params = new HttpParams();
    
    if (startDate) params = params.set('startDate', startDate.toISOString());
    if (endDate) params = params.set('endDate', endDate.toISOString());
    if (customerId) params = params.set('customerId', customerId.toString());
    if (status) params = params.set('status', status);
    
    return this.http.get<any>(`${this.apiUrl}/job-cost`, { params });
  }

  getIncomeStatement(startDate?: Date, endDate?: Date): Observable<IncomeStatementDto> {
    let params = new HttpParams();
    if (startDate) params = params.set('startDate', startDate.toISOString());
    if (endDate) params = params.set('endDate', endDate.toISOString());
    
    return this.http.get<IncomeStatementDto>(`${this.apiUrl}/income-statement`, { params });
  }

  getBalanceSheet(asOfDate: Date): Observable<BalanceSheetDto> {
    let params = new HttpParams().set('asOfDate', asOfDate.toISOString());
    return this.http.get<BalanceSheetDto>(`${this.apiUrl}/balance-sheet`, { params });
  }

  getARAgingReport(asOfDate?: Date, page?: number, pageSize?: number): Observable<any> {
    let params = new HttpParams();
    if (asOfDate) params = params.set('asOfDate', asOfDate.toISOString());
    if (page) params = params.set('page', page.toString());
    if (pageSize) params = params.set('pageSize', pageSize.toString());
    
    return this.http.get<any>(`${this.apiUrl}/ar-aging`, { params });
  }

  getCustomerRateAnalysis(
    startDate?: Date,
    endDate?: Date,
    minJobs?: number,
    page?: number,
    pageSize?: number
  ): Observable<any> {
    let params = new HttpParams();
    if (startDate) params = params.set('startDate', startDate.toISOString());
    if (endDate) params = params.set('endDate', endDate.toISOString());
    if (minJobs) params = params.set('minJobs', minJobs.toString());
    if (page) params = params.set('page', page.toString());
    if (pageSize) params = params.set('pageSize', pageSize.toString());
    
    return this.http.get<any>(`${this.apiUrl}/customer-analysis`, { params });
  }

  getInventoryUsageReport(
    startDate?: Date,
    endDate?: Date,
    page?: number,
    pageSize?: number
  ): Observable<any> {
    let params = new HttpParams();
    if (startDate) params = params.set('startDate', startDate.toISOString());
    if (endDate) params = params.set('endDate', endDate.toISOString());
    if (page) params = params.set('page', page.toString());
    if (pageSize) params = params.set('pageSize', pageSize.toString());
    
    return this.http.get<any>(`${this.apiUrl}/inventory-usage`, { params });
  }

  exportToExcel(reportType: string, params: any): Observable<Blob> {
    return this.http.post(`${this.apiUrl}/export/excel`, 
      { reportType, ...params },
      { responseType: 'blob' }
    );
  }

  exportToPDF(reportType: string, params: any): Observable<Blob> {
    return this.http.post(`${this.apiUrl}/export/pdf`, 
      { reportType, ...params },
      { responseType: 'blob' }
    );
  }
}
