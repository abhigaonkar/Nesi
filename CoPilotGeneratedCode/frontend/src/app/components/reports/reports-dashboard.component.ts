import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';

interface ReportCard {
  title: string;
  description: string;
  icon: string;
  route: string;
  color: string;
}

@Component({
  selector: 'app-reports-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './reports-dashboard.component.html',
  styleUrls: ['./reports-dashboard.component.scss']
})
export class ReportsDashboardComponent {
  reports: ReportCard[] = [
    {
      title: 'Job Cost Analysis',
      description: 'Track profitability and costs for each work order',
      icon: 'bi-graph-up',
      route: '/reports/job-cost',
      color: 'primary'
    },
    {
      title: 'Income Statement',
      description: 'Revenue, expenses, and net income for a period',
      icon: 'bi-currency-dollar',
      route: '/reports/income-statement',
      color: 'success'
    },
    {
      title: 'Balance Sheet',
      description: 'Assets, liabilities, and equity at a point in time',
      icon: 'bi-bank',
      route: '/reports/balance-sheet',
      color: 'info'
    },
    {
      title: 'AR Aging',
      description: 'Outstanding receivables by age bucket',
      icon: 'bi-clock-history',
      route: '/reports/ar-aging',
      color: 'warning'
    },
    {
      title: 'Customer Analysis',
      description: 'Profitability and rate analysis by customer',
      icon: 'bi-people',
      route: '/reports/customer-analysis',
      color: 'secondary'
    },
    {
      title: 'Inventory Usage',
      description: 'Material consumption and costs',
      icon: 'bi-box-seam',
      route: '/reports/inventory-usage',
      color: 'danger'
    }
  ];

  constructor(private router: Router) {}

  navigateToReport(route: string): void {
    this.router.navigate([route]);
  }
}
