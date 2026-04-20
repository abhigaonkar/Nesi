import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerService } from '../../services/customer.service';
import { Customer, AddressType } from '../../models/customer.model';

@Component({
  selector: 'app-customer-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss']
})
export class CustomerDetailComponent implements OnInit {
  customer?: Customer;
  customerId!: number;
  loading = false;
  error = '';

  AddressType = AddressType;

  constructor(
    private customerService: CustomerService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.customerId = +params['id'];
      this.loadCustomer();
    });
  }

  loadCustomer(): void {
    this.loading = true;
    this.error = '';
    
    this.customerService.getCustomerById(this.customerId, true).subscribe({
      next: (customer) => {
        this.customer = customer;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load customer details';
        this.loading = false;
        console.error('Error loading customer:', err);
      }
    });
  }

  editCustomer(): void {
    this.router.navigate(['/customers/edit', this.customerId]);
  }

  backToList(): void {
    this.router.navigate(['/customers']);
  }

  deleteCustomer(): void {
    if (confirm(`Are you sure you want to delete customer "${this.customer?.name}"?`)) {
      this.customerService.deleteCustomer(this.customerId).subscribe({
        next: () => {
          this.router.navigate(['/customers']);
        },
        error: (err) => {
          alert('Failed to delete customer');
          console.error('Error deleting customer:', err);
        }
      });
    }
  }

  activateCustomer(): void {
    this.customerService.activateCustomer(this.customerId).subscribe({
      next: () => {
        this.loadCustomer();
      },
      error: (err) => {
        alert('Failed to activate customer');
        console.error('Error activating customer:', err);
      }
    });
  }

  getAddressTypeLabel(type: AddressType): string {
    switch (type) {
      case AddressType.Billing:
        return 'Billing';
      case AddressType.Shipping:
        return 'Shipping';
      case AddressType.Both:
        return 'Billing & Shipping';
      default:
        return 'Unknown';
    }
  }

  formatDate(date: Date | undefined): string {
    if (!date) return '-';
    return new Date(date).toLocaleDateString();
  }

  formatCurrency(amount: number | undefined): string {
    if (amount === undefined || amount === null) return '-';
    return `$${amount.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
  }
}
