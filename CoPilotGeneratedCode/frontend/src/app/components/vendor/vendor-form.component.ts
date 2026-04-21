import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { VendorService, CreateVendorCommand, UpdateVendorCommand } from '../../../services/vendor.service';

@Component({
  selector: 'app-vendor-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './vendor-form.component.html',
  styleUrls: ['./vendor-form.component.scss']
})
export class VendorFormComponent implements OnInit {
  isEditMode: boolean = false;
  vendorId?: number;
  loading: boolean = false;
  saving: boolean = false;
  error: string = '';

  // Form fields
  companyName: string = '';
  contactName: string = '';
  email: string = '';
  phone: string = '';
  fax: string = '';
  website: string = '';
  address: string = '';
  city: string = '';
  state: string = '';
  zipCode: string = '';
  country: string = 'USA';
  taxId: string = '';
  accountNumber: string = '';
  paymentTermsDays: number = 30;
  creditLimit: number = 0;
  rating: number = 0;
  notes: string = '';
  isActive: boolean = true;

  ratingOptions = [0, 1, 2, 3, 4, 5];

  constructor(
    private vendorService: VendorService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.vendorId = Number(id);
      this.loadVendor(this.vendorId);
    }
  }

  loadVendor(id: number): void {
    this.loading = true;
    this.vendorService.getVendorById(id)
      .subscribe({
        next: (response: any) => {
          const vendor = response.data;
          this.companyName = vendor.companyName;
          this.contactName = vendor.contactName || '';
          this.email = vendor.email || '';
          this.phone = vendor.phone || '';
          this.fax = vendor.fax || '';
          this.website = vendor.website || '';
          this.address = vendor.address || '';
          this.city = vendor.city || '';
          this.state = vendor.state || '';
          this.zipCode = vendor.zipCode || '';
          this.country = vendor.country || 'USA';
          this.taxId = vendor.taxId || '';
          this.accountNumber = vendor.accountNumber || '';
          this.paymentTermsDays = vendor.paymentTermsDays || 30;
          this.creditLimit = vendor.creditLimit || 0;
          this.rating = vendor.rating || 0;
          this.notes = vendor.notes || '';
          this.isActive = vendor.isActive;
          this.loading = false;
        },
        error: (err: any) => {
          this.error = 'Failed to load vendor';
          this.loading = false;
          console.error(err);
        }
      });
  }

  saveVendor(): void {
    // Validation
    if (!this.companyName.trim()) {
      this.error = 'Please enter a company name';
      return;
    }

    if (this.email && !this.isValidEmail(this.email)) {
      this.error = 'Please enter a valid email address';
      return;
    }

    this.saving = true;
    this.error = '';

    if (this.isEditMode && this.vendorId) {
      this.updateVendor();
    } else {
      this.createVendor();
    }
  }

  createVendor(): void {
    const command: CreateVendorCommand = {
      companyName: this.companyName.trim(),
      contactName: this.contactName.trim() || undefined,
      email: this.email.trim() || undefined,
      phone: this.phone.trim() || undefined,
      fax: this.fax.trim() || undefined,
      website: this.website.trim() || undefined,
      address: this.address.trim() || undefined,
      city: this.city.trim() || undefined,
      state: this.state.trim() || undefined,
      zipCode: this.zipCode.trim() || undefined,
      country: this.country.trim() || undefined,
      taxId: this.taxId.trim() || undefined,
      accountNumber: this.accountNumber.trim() || undefined,
      paymentTermsDays: this.paymentTermsDays || undefined,
      creditLimit: this.creditLimit || undefined,
      rating: this.rating || undefined,
      notes: this.notes.trim() || undefined,
      isActive: this.isActive
    };

    this.vendorService.createVendor(command)
      .subscribe({
        next: (response: any) => {
          alert('Vendor created successfully');
          this.router.navigate(['/vendors', response.data]);
        },
        error: (err: any) => {
          this.error = 'Failed to create vendor';
          this.saving = false;
          console.error(err);
        }
      });
  }

  updateVendor(): void {
    const command: UpdateVendorCommand = {
      id: this.vendorId!,
      companyName: this.companyName.trim(),
      contactName: this.contactName.trim() || undefined,
      email: this.email.trim() || undefined,
      phone: this.phone.trim() || undefined,
      fax: this.fax.trim() || undefined,
      website: this.website.trim() || undefined,
      address: this.address.trim() || undefined,
      city: this.city.trim() || undefined,
      state: this.state.trim() || undefined,
      zipCode: this.zipCode.trim() || undefined,
      country: this.country.trim() || undefined,
      taxId: this.taxId.trim() || undefined,
      accountNumber: this.accountNumber.trim() || undefined,
      paymentTermsDays: this.paymentTermsDays || undefined,
      creditLimit: this.creditLimit || undefined,
      rating: this.rating || undefined,
      notes: this.notes.trim() || undefined,
      isActive: this.isActive
    };

    this.vendorService.updateVendor(command)
      .subscribe({
        next: () => {
          alert('Vendor updated successfully');
          this.router.navigate(['/vendors', this.vendorId]);
        },
        error: (err: any) => {
          this.error = 'Failed to update vendor';
          this.saving = false;
          console.error(err);
        }
      });
  }

  cancel(): void {
    if (this.isEditMode && this.vendorId) {
      this.router.navigate(['/vendors', this.vendorId]);
    } else {
      this.router.navigate(['/vendors']);
    }
  }

  isValidEmail(email: string): boolean {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
  }
}
