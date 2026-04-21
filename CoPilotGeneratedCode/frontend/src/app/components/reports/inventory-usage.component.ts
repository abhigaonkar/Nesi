import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReportService, InventoryUsageDto } from '../../../services/report.service';

@Component({
  selector: 'app-inventory-usage',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './inventory-usage.component.html',
  styleUrls: ['./inventory-usage.component.scss']
})
export class InventoryUsageComponent implements OnInit {
  materials: InventoryUsageDto[] = [];
  loading: boolean = false;
  error: string = '';

  // Filters
  startDate: string = '';
  endDate: string = '';

  // Sort
  sortField: string = 'totalCost';
  sortDirection: 'asc' | 'desc' = 'desc';

  // Pagination
  currentPage: number = 1;
  pageSize: number = 20;
  totalCount: number = 0;
  totalPages: number = 0;

  // Summary
  totalQuantity: number = 0;
  totalCost: number = 0;

  constructor(private reportService: ReportService) {
    // Default to current month
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

    const start = new Date(this.startDate);
    const end = new Date(this.endDate);

    this.reportService.getInventoryUsageReport(start, end, this.currentPage, this.pageSize).subscribe({
      next: (response: any) => {
        // Backend returns InventoryUsageSummaryDto with materials array
        this.materials = response.materials || [];
        this.totalCount = response.totalMaterials || 0;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.calculateSummary();
        this.sortMaterials();
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load inventory usage report: ' + (err.error?.message || err.message || 'Unknown error');
        this.loading = false;
        console.error('Inventory usage error:', err);
      }
    });
  }



  calculateSummary(): void {
    this.totalCost = this.materials.reduce((sum, m) => sum + m.totalCost, 0);
    // Note: Can't simply sum quantities due to different units of measure
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
    this.sortMaterials();
  }

  sortMaterials(): void {
    this.materials.sort((a: any, b: any) => {
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
    alert('Export functionality requires backend implementation');
  }
}
