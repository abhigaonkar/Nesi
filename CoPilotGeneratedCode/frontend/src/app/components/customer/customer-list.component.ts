import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CustomerService } from '../../services/customer.service';
import { Customer } from '../../models/customer.model';

@Component({
  selector: 'app-customer-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.scss']
})
export class CustomerListComponent implements OnInit {
  customers: Customer[] = [];
  loading = false;
  error = '';
  
  // Pagination
  currentPage = 1;
  pageSize = 20;
  totalCount = 0;
  totalPages = 0;
  
  // Filters
  activeOnly = true;
  searchTerm = '';

  constructor(
    private customerService: CustomerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadCustomers();
  }

  loadCustomers(): void {
    this.loading = true;
    this.error = '';
    
    if (this.searchTerm && this.searchTerm.length > 0) {
      this.customerService.searchCustomers(this.searchTerm).subscribe({
        next: (customers) => {
          this.customers = customers;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to search customers';
          this.loading = false;
          console.error('Error searching customers:', err);
        }
      });
    } else {
      this.customerService.getCustomers(
        this.activeOnly, 
        undefined, 
        undefined, 
        this.currentPage, 
        this.pageSize
      ).subscribe({
        next: (result) => {
          this.customers = result.customers;
          this.totalCount = result.totalCount;
          this.totalPages = result.totalPages;
          this.currentPage = result.pageNumber;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load customers';
          this.loading = false;
          console.error('Error loading customers:', err);
        }
      });
    }
  }

  onSearchChange(): void {
    this.currentPage = 1;
    this.loadCustomers();
  }

  onActiveFilterChange(): void {
    this.currentPage = 1;
    this.loadCustomers();
  }

  viewCustomer(id: number): void {
    this.router.navigate(['/customers', id]);
  }

  editCustomer(id: number): void {
    this.router.navigate(['/customers/edit', id]);
  }

  createCustomer(): void {
    this.router.navigate(['/customers/create']);
  }

  deleteCustomer(customer: Customer): void {
    if (confirm(`Are you sure you want to delete customer "${customer.name}"?`)) {
      this.customerService.deleteCustomer(customer.id).subscribe({
        next: () => {
          this.loadCustomers();
        },
        error: (err) => {
          alert('Failed to delete customer');
          console.error('Error deleting customer:', err);
        }
      });
    }
  }

  activateCustomer(customer: Customer): void {
    this.customerService.activateCustomer(customer.id).subscribe({
      next: () => {
        this.loadCustomers();
      },
      error: (err) => {
        alert('Failed to activate customer');
        console.error('Error activating customer:', err);
      }
    });
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadCustomers();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadCustomers();
    }
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadCustomers();
    }
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 5;
    let startPage = Math.max(1, this.currentPage - Math.floor(maxPagesToShow / 2));
    let endPage = Math.min(this.totalPages, startPage + maxPagesToShow - 1);
    
    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    
    return pages;
  }
}
