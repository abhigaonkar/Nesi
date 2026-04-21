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
        this.materials = response.data || [];
        this.totalCount = response.totalCount || 0;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.calculateSummary();
        this.sortMaterials();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load inventory usage report. Using mock data for demo.';
        this.loadMockData();
        this.loading = false;
        console.error(err);
      }
    });
  }

  loadMockData(): void {
    this.materials = [
      {
        materialId: 1,
        materialName: 'Copper Wire 12 AWG',
        partNumber: 'CW-12-100',
        quantityUsed: 5000,
        unitOfMeasure: 'FT',
        unitCost: 0.50,
        totalCost: 2500,
        topWorkOrders: [
          { workOrderNumber: 'WO-2026-00001', quantity: 2000 },
          { workOrderNumber: 'WO-2026-00003', quantity: 1500 }
        ]
      },
      {
        materialId: 2,
        materialName: 'Circuit Breaker 20A',
        partNumber: 'CB-20A',
        quantityUsed: 150,
        unitOfMeasure: 'EA',
        unitCost: 8.00,
        totalCost: 1200,
        topWorkOrders: [
          { workOrderNumber: 'WO-2026-00001', quantity: 75 },
          { workOrderNumber: 'WO-2026-00002', quantity: 50 }
        ]
      },
      {
        materialId: 3,
        materialName: 'Conduit PVC 1/2"',
        partNumber: 'PVC-05-10',
        quantityUsed: 800,
        unitOfMeasure: 'FT',
        unitCost: 0.75,
        totalCost: 600,
        topWorkOrders: [
          { workOrderNumber: 'WO-2026-00002', quantity: 400 },
          { workOrderNumber: 'WO-2026-00004', quantity: 300 }
        ]
      },
      {
        materialId: 4,
        materialName: 'Junction Box 4x4',
        partNumber: 'JB-4X4',
        quantityUsed: 200,
        unitOfMeasure: 'EA',
        unitCost: 2.50,
        totalCost: 500,
        topWorkOrders: [
          { workOrderNumber: 'WO-2026-00001', quantity: 100 },
          { workOrderNumber: 'WO-2026-00003', quantity: 60 }
        ]
      },
      {
        materialId: 5,
        materialName: 'Wire Nuts Orange',
        partNumber: 'WN-ORG-100',
        quantityUsed: 1000,
        unitOfMeasure: 'EA',
        unitCost: 0.10,
        totalCost: 100,
        topWorkOrders: [
          { workOrderNumber: 'WO-2026-00001', quantity: 400 },
          { workOrderNumber: 'WO-2026-00002', quantity: 350 }
        ]
      }
    ];
    this.totalCount = 5;
    this.totalPages = 1;
    this.calculateSummary();
    this.sortMaterials();
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
