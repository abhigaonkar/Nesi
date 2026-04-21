import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReportService, IncomeStatementDto } from '../../../services/report.service';

@Component({
  selector: 'app-income-statement',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './income-statement.component.html',
  styleUrls: ['./income-statement.component.scss']
})
export class IncomeStatementComponent implements OnInit {
  statement?: IncomeStatementDto;
  loading: boolean = false;
  error: string = '';

  // Filters
  periodType: string = 'month';
  startDate: string = '';
  endDate: string = '';

  constructor(private reportService: ReportService) {
    this.setDefaultPeriod();
  }

  ngOnInit(): void {
    this.loadStatement();
  }

  setDefaultPeriod(): void {
    const now = new Date();
    if (this.periodType === 'month') {
      const firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
      this.startDate = firstDay.toISOString().split('T')[0];
      this.endDate = now.toISOString().split('T')[0];
    } else if (this.periodType === 'quarter') {
      const quarter = Math.floor(now.getMonth() / 3);
      const firstDay = new Date(now.getFullYear(), quarter * 3, 1);
      this.startDate = firstDay.toISOString().split('T')[0];
      this.endDate = now.toISOString().split('T')[0];
    } else if (this.periodType === 'year') {
      const firstDay = new Date(now.getFullYear(), 0, 1);
      this.startDate = firstDay.toISOString().split('T')[0];
      this.endDate = now.toISOString().split('T')[0];
    }
  }

  onPeriodTypeChange(): void {
    this.setDefaultPeriod();
    this.loadStatement();
  }

  loadStatement(): void {
    this.loading = true;
    this.error = '';

    const start = new Date(this.startDate);
    const end = new Date(this.endDate);

    this.reportService.getIncomeStatement(start, end).subscribe({
      next: (response: any) => {
        // Backend returns IncomeStatementDto directly
        this.statement = response;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load income statement: ' + (err.error?.message || err.message || 'Unknown error');
        this.loading = false;
        console.error('Income statement error:', err);
      }
    });
  }



  getPercentOfRevenue(amount: number): number {
    return this.statement && this.statement.revenue > 0
      ? (amount / this.statement.revenue) * 100
      : 0;
  }

  exportToExcel(): void {
    alert('Export functionality requires backend implementation');
  }

  exportToPDF(): void {
    alert('Export functionality requires backend implementation');
  }
}
