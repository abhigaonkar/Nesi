import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerService } from '../../services/customer.service';
import { Customer, CreateCustomerRequest, UpdateCustomerRequest } from '../../models/customer.model';

@Component({
  selector: 'app-customer-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './customer-form.component.html',
  styleUrls: ['./customer-form.component.scss']
})
export class CustomerFormComponent implements OnInit {
  isEditMode = false;
  customerId?: number;
  loading = false;
  saving = false;
  error = '';
  
  // Form fields
  name = '';
  businessUnitId?: number;
  contactName = '';
  email = '';
  phone = '';
  address = '';
  creditLimit?: number;
  paymentTermsDays?: number;
  accountManagerId?: number;
  notes = '';

  constructor(
    private customerService: CustomerService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.customerId = +params['id'];
        this.loadCustomer();
      }
    });
  }

  loadCustomer(): void {
    if (!this.customerId) return;
    
    this.loading = true;
    this.error = '';
    
    this.customerService.getCustomerById(this.customerId).subscribe({
      next: (customer) => {
        this.populateForm(customer);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load customer';
        this.loading = false;
        console.error('Error loading customer:', err);
      }
    });
  }

  populateForm(customer: Customer): void {
    this.name = customer.name;
    this.businessUnitId = customer.businessUnitId;
    this.contactName = customer.contactName || '';
    this.email = customer.email || '';
    this.phone = customer.phone || '';
    this.address = customer.address || '';
    this.creditLimit = customer.creditLimit;
    this.paymentTermsDays = customer.paymentTermsDays;
    this.accountManagerId = customer.accountManagerId;
    this.notes = customer.notes || '';
  }

  onSubmit(): void {
    if (!this.validateForm()) {
      return;
    }

    this.saving = true;
    this.error = '';

    if (this.isEditMode && this.customerId) {
      this.updateCustomer();
    } else {
      this.createCustomer();
    }
  }

  createCustomer(): void {
    const request: CreateCustomerRequest = {
      name: this.name,
      businessUnitId: this.businessUnitId,
      contactName: this.contactName || undefined,
      email: this.email || undefined,
      phone: this.phone || undefined,
      address: this.address || undefined,
      creditLimit: this.creditLimit,
      paymentTermsDays: this.paymentTermsDays,
      accountManagerId: this.accountManagerId,
      notes: this.notes || undefined
    };

    this.customerService.createCustomer(request).subscribe({
      next: (customerId) => {
        this.saving = false;
        this.router.navigate(['/customers', customerId]);
      },
      error: (err) => {
        this.error = 'Failed to create customer. Please check your inputs and try again.';
        this.saving = false;
        console.error('Error creating customer:', err);
      }
    });
  }

  updateCustomer(): void {
    if (!this.customerId) return;

    const request: UpdateCustomerRequest = {
      id: this.customerId,
      name: this.name,
      businessUnitId: this.businessUnitId,
      contactName: this.contactName || undefined,
      email: this.email || undefined,
      phone: this.phone || undefined,
      address: this.address || undefined,
      creditLimit: this.creditLimit,
      paymentTermsDays: this.paymentTermsDays,
      accountManagerId: this.accountManagerId,
      notes: this.notes || undefined
    };

    this.customerService.updateCustomer(this.customerId, request).subscribe({
      next: () => {
        this.saving = false;
        this.router.navigate(['/customers', this.customerId]);
      },
      error: (err) => {
        this.error = 'Failed to update customer. Please check your inputs and try again.';
        this.saving = false;
        console.error('Error updating customer:', err);
      }
    });
  }

  validateForm(): boolean {
    if (!this.name || this.name.trim().length === 0) {
      this.error = 'Customer name is required';
      return false;
    }

    if (this.email && !this.isValidEmail(this.email)) {
      this.error = 'Please enter a valid email address';
      return false;
    }

    if (this.creditLimit && this.creditLimit < 0) {
      this.error = 'Credit limit must be positive';
      return false;
    }

    if (this.paymentTermsDays && this.paymentTermsDays < 0) {
      this.error = 'Payment terms must be positive';
      return false;
    }

    return true;
  }

  isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  cancel(): void {
    if (this.customerId) {
      this.router.navigate(['/customers', this.customerId]);
    } else {
      this.router.navigate(['/customers']);
    }
  }
}
