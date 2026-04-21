import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReportService, JobCostReportDto } from '../../../services/report.service';

@Component({
  selector: 'app-job-cost-report',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './job-cost-report.component.html',
  styleUrls: ['./job-cost-report.component.scss']
})
export class JobCostReportComponent implements OnInit {
  reports: JobCostReportDto[] = [];
  loading: boolean = false;
  error: string = '';

  // Filters
  startDate: string = '';
  endDate: string = '';
  status: string = '';
  customerId?: number;

  // Pagination
  currentPage: number = 1;
  pageSize: number = 20;
  totalCount: number = 0;
  totalPages: number = 0;

  // Sorting
  sortField: string = 'orderDate';
  sortDirection: 'asc' | 'desc' = 'desc';

  // Summary calculations
  totalRevenue: number = 0;
  totalCost: number = 0;
  totalProfit: number = 0;
  averageMargin: number = 0;

  constructor(private reportService: ReportService) {
    // Set default dates to current month
    const now = new Date();
    const firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
    this.startDate = firstDay.toISOString().split('T')[0];
    this.endDate = now.toISOString().split('T')[0];
  }

  ngOnInit(): void {
    this.loadReport();
  }

  loadReport(): void {
    this.loading = true;
    this.error = '';

    const start = this.startDate ? new Date(this.startDate) : undefined;
    const end = this.endDate ? new Date(this.endDate) : undefined;

    this.reportService.getJobCostReport(
      start,
      end,
      this.customerId,
      this.status || undefined
    ).subscribe({
      next: (response: any) => {
        // Backend returns JobCostSummaryDto
        this.reports = response.workOrders || [];
        this.totalCount = response.totalWorkOrders || 0;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        
        // Use summary data from backend
        this.totalRevenue = response.totalRevenue || 0;
        this.totalCost = response.totalCost || 0;
        this.totalProfit = response.totalProfit || 0;
        this.averageMargin = response.averageProfitMargin || 0;
        
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load job cost report. Using mock data for demo.';
        this.loadMockData();
        this.loading = false;
        console.error(err);
      }
    });
  }

  loadMockData(): void {
    // Mock data for demonstration
    this.reports = [
      {
        workOrderId: 1,
        workOrderNumber: 'WO-2026-00001',
        customerName: 'ABC Electric Corp',
        startDate: new Date('2026-04-01'),
        completionDate: new Date('2026-04-15'),
        status: 'Completed',
        laborCost: 5000,
        materialCost: 3000,
        overheadCost: 1000,
        totalCost: 9000,
        quotedAmount: 12000,
        actualAmount: 12000,
        profitMargin: 3000,
        profitPercentage: 25
      },
      {
        workOrderId: 2,
        workOrderNumber: 'WO-2026-00002',
        customerName: 'XYZ Manufacturing',
        startDate: new Date('2026-04-05'),
        completionDate: new Date('2026-04-20'),
        status: 'Completed',
        laborCost: 8000,
        materialCost: 5000,
        overheadCost: 1500,
        totalCost: 14500,
        quotedAmount: 18000,
        actualAmount: 18500,
        profitMargin: 4000,
        profitPercentage: 21.6
      },
      {
        workOrderId: 3,
        workOrderNumber: 'WO-2026-00003',
        customerName: 'Tech Solutions Inc',
        startDate: new Date('2026-04-10'),
        status: 'In Progress',
        laborCost: 3000,
        materialCost: 2000,
        overheadCost: 500,
        totalCost: 5500,
        quotedAmount: 8000,
        actualAmount: 0,
        profitMargin: 2500,
        profitPercentage: 31.25
      }
    ];
    this.totalCount = 3;
    this.totalPages = 1;
    this.calculateSummary();
  }

  calculateSummary(): void {
    this.totalRevenue = this.reports.reduce((sum, r) => sum + (r.actualAmount || r.quotedAmount), 0);
    this.totalCost = this.reports.reduce((sum, r) => sum + r.totalCost, 0);
    this.totalProfit = this.totalRevenue - this.totalCost;
    this.averageMargin = this.totalRevenue > 0 ? (this.totalProfit / this.totalRevenue) * 100 : 0;
  }

  applyFilters(): void {
    this.currentPage = 1;
    this.loadReport();
  }

  clearFilters(): void {
    const now = new Date();
    const firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
    this.startDate = firstDay.toISOString().split('T')[0];
    this.endDate = now.toISOString().split('T')[0];
    this.status = '';
    this.customerId = undefined;
    this.loadReport();
  }

  sortBy(field: string): void {
    if (this.sortField === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortDirection = 'asc';
    }
    this.sortReports();
  }

  sortReports(): void {
    this.reports.sort((a: any, b: any) => {
      const aVal = a[this.sortField];
      const bVal = b[this.sortField];
      const direction = this.sortDirection === 'asc' ? 1 : -1;
      
      if (aVal < bVal) return -1 * direction;
      if (aVal > bVal) return 1 * direction;
      return 0;
    });
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadReport();
    }
  }

  exportToExcel(): void {
    this.reportService.exportToExcel('job-cost', {
      startDate: this.startDate,
      endDate: this.endDate,
      status: this.status,
      customerId: this.customerId
    }).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `job-cost-report-${new Date().toISOString().split('T')[0]}.xlsx`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        alert('Export functionality requires backend implementation');
        console.error(err);
      }
    });
  }

  exportToPDF(): void {
    this.reportService.exportToPDF('job-cost', {
      startDate: this.startDate,
      endDate: this.endDate,
      status: this.status,
      customerId: this.customerId
    }).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `job-cost-report-${new Date().toISOString().split('T')[0]}.pdf`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        alert('Export functionality requires backend implementation');
        console.error(err);
      }
    });
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'completed': return 'badge bg-success';
      case 'in progress': return 'badge bg-primary';
      case 'pending': return 'badge bg-warning';
      case 'cancelled': return 'badge bg-danger';
      default: return 'badge bg-secondary';
    }
  }

  getProfitClass(percentage: number): string {
    if (percentage >= 25) return 'text-success fw-bold';
    if (percentage >= 15) return 'text-primary';
    if (percentage >= 5) return 'text-warning';
    return 'text-danger fw-bold';
  }
}
