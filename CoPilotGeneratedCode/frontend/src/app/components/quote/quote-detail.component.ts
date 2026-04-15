import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { QuoteService } from '../../services/quote.service';
import { Quote, QuoteStatus, QuoteType, QuoteLineItemType } from '../../models/quote.model';

@Component({
  selector: 'app-quote-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './quote-detail.component.html',
  styleUrls: ['./quote-detail.component.scss']
})
export class QuoteDetailComponent implements OnInit {
  quote?: Quote;
  loading = false;
  error = '';
  actionLoading = false;

  QuoteStatus = QuoteStatus;
  QuoteType = QuoteType;
  QuoteLineItemType = QuoteLineItemType;

  constructor(
    private quoteService: QuoteService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) {
        this.loadQuote(id);
      }
    });
  }

  loadQuote(id: number): void {
    this.loading = true;
    this.error = '';
    
    this.quoteService.getQuoteById(id).subscribe({
      next: (quote) => {
        this.quote = quote;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load quote';
        this.loading = false;
        console.error('Error loading quote:', err);
      }
    });
  }

  submitQuote(): void {
    if (!this.quote || this.actionLoading) return;
    
    if (!confirm('Are you sure you want to submit this quote for approval?')) {
      return;
    }
    
    this.actionLoading = true;
    this.quoteService.submitQuote(this.quote.id).subscribe({
      next: () => {
        this.actionLoading = false;
        this.loadQuote(this.quote!.id);
      },
      error: (err) => {
        this.error = 'Failed to submit quote';
        this.actionLoading = false;
        console.error('Error submitting quote:', err);
      }
    });
  }

  approveQuote(): void {
    if (!this.quote || this.actionLoading) return;
    
    if (!confirm('Are you sure you want to approve this quote?')) {
      return;
    }
    
    this.actionLoading = true;
    this.quoteService.approveQuote(this.quote.id).subscribe({
      next: () => {
        this.actionLoading = false;
        this.loadQuote(this.quote!.id);
      },
      error: (err) => {
        this.error = 'Failed to approve quote';
        this.actionLoading = false;
        console.error('Error approving quote:', err);
      }
    });
  }

  rejectQuote(): void {
    if (!this.quote || this.actionLoading) return;
    
    const reason = prompt('Please enter a rejection reason:');
    if (!reason) {
      return;
    }
    
    this.actionLoading = true;
    this.quoteService.rejectQuote(this.quote.id, reason).subscribe({
      next: () => {
        this.actionLoading = false;
        this.loadQuote(this.quote!.id);
      },
      error: (err) => {
        this.error = 'Failed to reject quote';
        this.actionLoading = false;
        console.error('Error rejecting quote:', err);
      }
    });
  }

  editQuote(): void {
    if (!this.quote) return;
    this.router.navigate(['/quotes/edit', this.quote.id]);
  }

  backToList(): void {
    this.router.navigate(['/quotes']);
  }

  getStatusLabel(status: QuoteStatus): string {
    return QuoteStatus[status];
  }

  getQuoteTypeLabel(type: QuoteType): string {
    return QuoteType[type].replace(/([A-Z])/g, ' $1').trim();
  }

  getLineItemTypeLabel(type: QuoteLineItemType): string {
    return QuoteLineItemType[type];
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

  canEdit(): boolean {
    return this.quote?.status === QuoteStatus.Draft;
  }

  canSubmit(): boolean {
    return this.quote?.status === QuoteStatus.Draft;
  }

  canApprove(): boolean {
    return this.quote?.status === QuoteStatus.Submitted;
  }

  canReject(): boolean {
    return this.quote?.status === QuoteStatus.Submitted;
  }
}
