import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReportService, ARAgingDto } from '../../services/report.service';

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
        // Backend returns ArAgingSummaryDto with customers array
        this.agingData = response.customers || [];
        this.totalCount = response.totalCustomers || 0;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        
        // Use summary from backend if available
        if (response.totalOutstanding !== undefined) {
          this.totalOutstanding = response.totalOutstanding;
          this.totalCurrent = response.currentAmount;
          this.total31to60 = response.days31To60;
          this.total61to90 = response.days61To90;
          this.totalOver90 = response.over90Days;
        } else {
          this.calculateTotals();
        }
        
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load AR aging report: ' + (err.error?.message || err.message || 'Unknown error');
        this.loading = false;
        console.error('AR aging error:', err);
      }
    });
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
    this.reportService.exportToExcel('ar-aging', {
      asOfDate: this.asOfDate
    }).subscribe({
      next: (blob: any) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `ar-aging-report-${new Date().toISOString().split('T')[0]}.xlsx`;
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
    this.reportService.exportToPDF('ar-aging', {
      asOfDate: this.asOfDate
    }).subscribe({
      next: (blob: any) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `ar-aging-report-${new Date().toISOString().split('T')[0]}.pdf`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err: any) => {
        alert('Failed to export report. Please try again.');
        console.error(err);
      }
    });
  }
}
