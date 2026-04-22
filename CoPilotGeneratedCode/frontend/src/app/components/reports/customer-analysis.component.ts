import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReportService, CustomerRateAnalysisDto } from '../../services/report.service';

@Component({
  selector: 'app-customer-analysis',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './customer-analysis.component.html',
  styleUrls: ['./customer-analysis.component.scss']
})
export class CustomerAnalysisComponent implements OnInit {
  customers: CustomerRateAnalysisDto[] = [];
  loading: boolean = false;
  error: string = '';

  // Filters
  startDate: string = '';
  endDate: string = '';
  minJobs: number = 1;

  // Sort
  sortField: string = 'profitMargin';
  sortDirection: 'asc' | 'desc' = 'desc';

  // Pagination
  currentPage: number = 1;
  pageSize: number = 20;
  totalCount: number = 0;
  totalPages: number = 0;

  constructor(private reportService: ReportService) {
    // Default to last 12 months
    const now = new Date();
    const yearAgo = new Date(now.getFullYear() - 1, now.getMonth(), now.getDate());
    this.startDate = yearAgo.toISOString().split('T')[0];
    this.endDate = now.toISOString().split('T')[0];
  }

  ngOnInit(): void {
    this.loadReport();
  }

  loadReport(): void {
    this.loading = true;
    this.error = '';

    const start = new Date(this.startDate);
    const end = new Date(this.endDate);

    this.reportService.getCustomerRateAnalysis(start, end, this.minJobs, this.currentPage, this.pageSize).subscribe({
      next: (response: any) => {
        // Backend returns CustomerAnalysisSummaryDto with customers array
        this.customers = response.customers || [];
        this.totalCount = response.totalCustomers || 0;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.sortCustomers();
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load customer analysis: ' + (err.error?.message || err.message || 'Unknown error');
        this.loading = false;
        console.error('Customer analysis error:', err);
      }
    });
  }



  applyFilters(): void {
    this.currentPage = 1;
    this.loadReport();
  }

  sortBy(field: string): void {
    if (this.sortField === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortDirection = 'desc';
    }
    this.sortCustomers();
  }

  sortCustomers(): void {
    this.customers.sort((a: any, b: any) => {
      const aVal = a[this.sortField];
      const bVal = b[this.sortField];
      const direction = this.sortDirection === 'asc' ? 1 : -1;
      
      if (aVal < bVal) return -1 * direction;
      if (aVal > bVal) return 1 * direction;
      return 0;
    });
  }

  getMarginClass(margin: number): string {
    if (margin >= 30) return 'text-success fw-bold';
    if (margin >= 20) return 'text-primary';
    if (margin >= 10) return 'text-warning';
    return 'text-danger';
  }

  getSegment(revenue: number, jobs: number): string {
    if (revenue > 300000) return 'Premium';
    if (revenue > 150000 || jobs > 15) return 'High Value';
    if (revenue > 50000 || jobs > 5) return 'Standard';
    return 'Low Value';
  }

  getSegmentClass(segment: string): string {
    switch (segment) {
      case 'Premium': return 'badge bg-success';
      case 'High Value': return 'badge bg-primary';
      case 'Standard': return 'badge bg-info';
      default: return 'badge bg-secondary';
    }
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadReport();
    }
  }

  exportToExcel(): void {
    this.reportService.exportToExcel('customer-analysis', {
      startDate: this.startDate,
      endDate: this.endDate
    }).subscribe({
      next: (blob: any) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `customer-analysis-${new Date().toISOString().split('T')[0]}.xlsx`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err: any) => {
        alert('Failed to export report. Please try again.');
        console.error(err);
      }
    });
  }

  exportToPDF(): void {
    this.reportService.exportToPDF('customer-analysis', {
      startDate: this.startDate,
      endDate: this.endDate
    }).subscribe({
      next: (blob: any) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `customer-analysis-${new Date().toISOString().split('T')[0]}.pdf`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err: any) => {
        alert('Failed to export report. Please try again.');
        console.error(err);
      }
    });
  }

  getBestMarginCustomer(): string {
    if (!this.customers || this.customers.length === 0) return 'N/A';
    const best = this.customers.reduce((prev, curr) => 
      prev.profitMargin > curr.profitMargin ? prev : curr
    );
    return best?.customerName || 'N/A';
  }

  getMostJobsCustomer(): string {
    if (!this.customers || this.customers.length === 0) return 'N/A';
    const most = this.customers.reduce((prev, curr) => 
      prev.totalJobs > curr.totalJobs ? prev : curr
    );
    return most?.customerName || 'N/A';
  }
}
