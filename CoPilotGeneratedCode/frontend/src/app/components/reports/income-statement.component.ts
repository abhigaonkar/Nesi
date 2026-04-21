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
        this.statement = response.data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load income statement. Using mock data for demo.';
        this.loadMockData();
        this.loading = false;
        console.error(err);
      }
    });
  }

  loadMockData(): void {
    this.statement = {
      periodStart: new Date(this.startDate),
      periodEnd: new Date(this.endDate),
      revenue: 125000,
      costOfGoodsSold: 75000,
      grossProfit: 50000,
      operatingExpenses: {
        labor: 25000,
        materials: 15000,
        overhead: 8000,
        administrative: 7000,
        total: 55000
      },
      operatingIncome: -5000,
      otherIncome: 2000,
      otherExpenses: 1000,
      netIncome: -4000
    };
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
