import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { QuoteService } from '../../services/quote.service';
import { Quote, QuoteStatus, QuoteType } from '../../models/quote.model';

@Component({
  selector: 'app-quote-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './quote-list.component.html',
  styleUrls: ['./quote-list.component.scss']
})
export class QuoteListComponent implements OnInit {
  quotes: Quote[] = [];
  filteredQuotes: Quote[] = [];
  loading = false;
  error = '';
  
  // Filter properties
  statusFilter: QuoteStatus | 'all' = 'all';
  searchTerm = '';

  QuoteStatus = QuoteStatus;
  QuoteType = QuoteType;

  constructor(
    private quoteService: QuoteService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadQuotes();
  }

  loadQuotes(): void {
    this.loading = true;
    this.error = '';
    
    this.quoteService.getQuotes().subscribe({
      next: (quotes) => {
        this.quotes = quotes;
        this.applyFilters();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load quotes';
        this.loading = false;
        console.error('Error loading quotes:', err);
      }
    });
  }

  applyFilters(): void {
    this.filteredQuotes = this.quotes.filter(quote => {
      const matchesStatus = this.statusFilter === 'all' || quote.status === this.statusFilter;
      const matchesSearch = !this.searchTerm || 
        quote.quoteNumber.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        quote.customerName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        quote.description.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      return matchesStatus && matchesSearch;
    });
  }

  onStatusFilterChange(status: QuoteStatus | 'all'): void {
    this.statusFilter = status;
    this.applyFilters();
  }

  onSearchChange(term: string): void {
    this.searchTerm = term;
    this.applyFilters();
  }

  viewQuote(id: number): void {
    this.router.navigate(['/quotes', id]);
  }

  createQuote(): void {
    this.router.navigate(['/quotes/create']);
  }

  getStatusLabel(status: QuoteStatus): string {
    return QuoteStatus[status];
  }

  getQuoteTypeLabel(type: QuoteType): string {
    return QuoteType[type].replace(/([A-Z])/g, ' $1').trim();
  }

  getStatusClass(status: QuoteStatus): string {
    switch (status) {
      case QuoteStatus.Draft:
        return 'status-draft';
      case QuoteStatus.Submitted:
        return 'status-submitted';
      case QuoteStatus.Approved:
        return 'status-approved';
      case QuoteStatus.Rejected:
        return 'status-rejected';
      case QuoteStatus.CustomerApproved:
        return 'status-customer-approved';
      default:
        return '';
    }
  }
}
