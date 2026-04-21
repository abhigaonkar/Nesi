import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReportService, CustomerRateAnalysisDto } from '../../../services/report.service';

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
        this.customers = response.data || [];
        this.totalCount = response.totalCount || 0;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.sortCustomers();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load customer analysis. Using mock data for demo.';
        this.loadMockData();
        this.loading = false;
        console.error(err);
      }
    });
  }

  loadMockData(): void {
    this.customers = [
      {
        customerId: 1,
        customerName: 'ABC Electric Corp',
        totalJobs: 15,
        totalRevenue: 180000,
        totalCost: 120000,
        averageProfit: 4000,
        profitMargin: 33.3,
        averageJobValue: 12000,
        lastJobDate: new Date('2026-04-15')
      },
      {
        customerId: 2,
        customerName: 'XYZ Manufacturing',
        totalJobs: 25,
        totalRevenue: 375000,
        totalCost: 262500,
        averageProfit: 4500,
        profitMargin: 30.0,
        averageJobValue: 15000,
        lastJobDate: new Date('2026-04-20')
      },
      {
        customerId: 3,
        customerName: 'Tech Solutions Inc',
        totalJobs: 8,
        totalRevenue: 96000,
        totalCost: 72000,
        averageProfit: 3000,
        profitMargin: 25.0,
        averageJobValue: 12000,
        lastJobDate: new Date('2026-04-10')
      },
      {
        customerId: 4,
        customerName: 'BuildRight Construction',
        totalJobs: 12,
        totalRevenue: 144000,
        totalCost: 115200,
        averageProfit: 2400,
        profitMargin: 20.0,
        averageJobValue: 12000,
        lastJobDate: new Date('2026-04-18')
      },
      {
        customerId: 5,
        customerName: 'Green Energy Systems',
        totalJobs: 6,
        totalRevenue: 72000,
        totalCost: 60000,
        averageProfit: 2000,
        profitMargin: 16.7,
        averageJobValue: 12000,
        lastJobDate: new Date('2026-04-05')
      }
    ];
    this.totalCount = 5;
    this.totalPages = 1;
    this.sortCustomers();
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
    alert('Export functionality requires backend implementation');
  }
}
