import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { VendorService, VendorDto } from '../../../services/vendor.service';

@Component({
  selector: 'app-vendor-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './vendor-list.component.html',
  styleUrls: ['./vendor-list.component.scss']
})
export class VendorListComponent implements OnInit {
  vendors: VendorDto[] = [];
  loading: boolean = false;
  error: string = '';
  currentPage: number = 1;
  pageSize: number = 20;
  totalCount: number = 0;

  constructor(
    private vendorService: VendorService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadVendors();
  }

  loadVendors(): void {
    this.loading = true;
    this.error = '';
    
    this.vendorService.getVendors(true, this.currentPage, this.pageSize)
      .subscribe({
        next: (response) => {
          this.vendors = response.data.vendors;
          this.totalCount = response.data.totalCount;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load vendors';
          this.loading = false;
          console.error(err);
        }
      });
  }

  viewDetails(id: number): void {
    this.router.navigate(['/vendors', id]);
  }

  createNew(): void {
    this.router.navigate(['/vendors', 'new']);
  }
}
