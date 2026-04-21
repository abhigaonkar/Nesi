import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { VendorService, VendorDto } from '../../../services/vendor.service';

@Component({
  selector: 'app-vendor-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './vendor-detail.component.html',
  styleUrls: ['./vendor-detail.component.scss']
})
export class VendorDetailComponent implements OnInit {
  vendor: VendorDto | null = null;
  loading: boolean = false;
  error: string = '';
  
  constructor(
    private vendorService: VendorService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.loadVendor(id);
    }
  }

  loadVendor(id: number): void {
    this.loading = true;
    this.error = '';
    
    this.vendorService.getVendorById(id)
      .subscribe({
        next: (response: any) => {
          this.vendor = response.data;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load vendor';
          this.loading = false;
          console.error(err);
        }
      });
  }

  goBack(): void {
    this.router.navigate(['/vendors']);
  }

  edit(): void {
    if (this.vendor) {
      this.router.navigate(['/vendors', this.vendor.id, 'edit']);
    }
  }
}
