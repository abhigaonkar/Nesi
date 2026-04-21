import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReportService, ARAgingDto } from '../../../services/report.service';

@Component({
  selector: 'app-ar-aging-report',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ar-aging-report.component.html',
  styleUrls: ['./ar-aging-report.component.scss']
})
export class ARAgingReportComponent implements OnInit {
  agingData: ARAgingDto[] = [];
  loading: boolean = false;
  error: string = '';

  asOfDate: string = new Date().toISOString().split('T')[0];

  // Summary totals
  totalOutstanding: number = 0;
  totalCurrent: number = 0;
  total31to60: number = 0;
  total61to90: number = 0;
  totalOver90: number = 0;

  // Pagination
  currentPage: number = 1;
  pageSize: number = 20;
  totalCount: number = 0;
  totalPages: number = 0;

  constructor(private reportService: ReportService) {}

  ngOnInit(): void {
    this.loadReport();
  }

  loadReport(): void {
    this.loading = true;
    this.error = '';

    const asOf = new Date(this.asOfDate);

    this.reportService.getARAgingReport(asOf, this.currentPage, this.pageSize).subscribe({
      next: (response: any) => {
        this.agingData = response.data || [];
        this.totalCount = response.totalCount || 0;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.calculateTotals();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load AR aging report. Using mock data for demo.';
        this.loadMockData();
        this.loading = false;
        console.error(err);
      }
    });
  }

  loadMockData(): void {
    this.agingData = [
      {
        customerId: 1,
        customerName: 'ABC Electric Corp',
        totalOutstanding: 15000,
        current: 10000,
        days31to60: 3000,
        days61to90: 2000,
        over90Days: 0
      },
      {
        customerId: 2,
        customerName: 'XYZ Manufacturing',
        totalOutstanding: 25000,
        current: 20000,
        days31to60: 0,
        days61to90: 5000,
        over90Days: 0
      },
      {
        customerId: 3,
        customerName: 'Tech Solutions Inc',
        totalOutstanding: 8000,
        current: 0,
        days31to60: 0,
        days61to90: 3000,
        over90Days: 5000
      },
      {
        customerId: 4,
        customerName: 'BuildRight Construction',
        totalOutstanding: 12000,
        current: 12000,
        days31to60: 0,
        days61to90: 0,
        over90Days: 0
      }
    ];
    this.totalCount = 4;
    this.totalPages = 1;
    this.calculateTotals();
  }

  calculateTotals(): void {
    this.totalOutstanding = this.agingData.reduce((sum, d) => sum + d.totalOutstanding, 0);
    this.totalCurrent = this.agingData.reduce((sum, d) => sum + d.current, 0);
    this.total31to60 = this.agingData.reduce((sum, d) => sum + d.days31to60, 0);
    this.total61to90 = this.agingData.reduce((sum, d) => sum + d.days61to90, 0);
    this.totalOver90 = this.agingData.reduce((sum, d) => sum + d.over90Days, 0);
  }

  getPercentOfTotal(amount: number): number {
    return this.totalOutstanding > 0 ? (amount / this.totalOutstanding) * 100 : 0;
  }

  getRiskClass(days90Plus: number): string {
    if (days90Plus > 5000) return 'text-danger fw-bold';
    if (days90Plus > 0) return 'text-warning';
    return 'text-success';
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

  exportToPDF(): void {
    alert('Export functionality requires backend implementation');
  }
}
