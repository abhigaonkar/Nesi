import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { QuoteService } from '../../services/quote.service';
import { 
  CreateQuoteRequest, 
  CreateQuoteLineItem, 
  QuoteType, 
  QuoteLineItemType 
} from '../../models/quote.model';

@Component({
  selector: 'app-quote-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './quote-form.component.html',
  styleUrls: ['./quote-form.component.scss']
})
export class QuoteFormComponent implements OnInit {
  quoteId?: number;
  isEditMode = false;
  loading = false;
  error = '';

  // Form fields
  customerId = 0;
  quoteType: QuoteType = QuoteType.TimeAndMaterial;
  description = '';
  scope = '';
  estimatedStartDate = '';
  estimatedCompletionDate = '';
  projectManagerId?: number;
  termsAndConditions = 'Standard terms and conditions apply.';
  taxRate = 0.08; // 8% default tax rate
  
  lineItems: CreateQuoteLineItem[] = [];
  
  // Calculated totals
  subtotal = 0;
  taxAmount = 0;
  total = 0;

  // Enums for template
  QuoteType = QuoteType;
  QuoteLineItemType = QuoteLineItemType;
  
  // Available options (would come from API in real app)
  customers = [
    { id: 1, name: 'Acme Corporation' },
    { id: 2, name: 'TechStart Inc' },
    { id: 3, name: 'Global Industries' }
  ];
  
  projectManagers = [
    { id: 1, name: 'John Doe' },
    { id: 2, name: 'Jane Smith' },
    { id: 3, name: 'Mike Johnson' }
  ];
  
  jobTypes = [
    { id: 1, name: 'Senior Developer' },
    { id: 2, name: 'Junior Developer' },
    { id: 3, name: 'Project Manager' },
    { id: 4, name: 'QA Tester' }
  ];

  constructor(
    private quoteService: QuoteService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.quoteId = +params['id'];
        this.isEditMode = true;
        this.loadQuote();
      } else {
        this.addLineItem();
      }
    });
  }

  loadQuote(): void {
    if (!this.quoteId) return;
    
    this.loading = true;
    this.quoteService.getQuoteById(this.quoteId).subscribe({
      next: (quote) => {
        this.customerId = quote.customerId;
        this.quoteType = quote.quoteType;
        this.description = quote.description;
        this.scope = quote.scope;
        this.estimatedStartDate = new Date(quote.estimatedStartDate).toISOString().split('T')[0];
        this.estimatedCompletionDate = new Date(quote.estimatedCompletionDate).toISOString().split('T')[0];
        this.projectManagerId = quote.projectManagerId;
        this.termsAndConditions = quote.termsAndConditions;
        this.taxRate = quote.taxRate;
        
        this.lineItems = quote.lineItems.map(li => ({
          itemType: li.itemType,
          description: li.description,
          jobTypeId: li.jobTypeId,
          estimatedHours: li.estimatedHours,
          partNumber: li.partNumber,
          quantity: li.quantity,
          unitPrice: li.unitPrice,
          notes: li.notes
        }));
        
        this.calculateTotals();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load quote';
        this.loading = false;
        console.error('Error loading quote:', err);
      }
    });
  }

  addLineItem(): void {
    this.lineItems.push({
      itemType: QuoteLineItemType.Labor,
      description: '',
      jobTypeId: undefined,
      estimatedHours: 0,
      partNumber: undefined,
      quantity: 1,
      unitPrice: 0,
      notes: undefined
    });
  }

  removeLineItem(index: number): void {
    this.lineItems.splice(index, 1);
    this.calculateTotals();
  }

  onLineItemChange(): void {
    this.calculateTotals();
  }

  calculateTotals(): void {
    this.subtotal = this.lineItems.reduce((sum, item) => {
      const lineTotal = this.getLineItemTotal(item);
      return sum + lineTotal;
    }, 0);
    
    this.taxAmount = this.subtotal * this.taxRate;
    this.total = this.subtotal + this.taxAmount;
  }

  getLineItemTotal(item: CreateQuoteLineItem): number {
    if (item.itemType === QuoteLineItemType.Labor) {
      return item.estimatedHours * item.unitPrice;
    }
    return item.quantity * item.unitPrice;
  }

  saveQuote(): void {
    if (!this.validateForm()) {
      return;
    }

    const request: CreateQuoteRequest = {
      customerId: this.customerId,
      quoteType: this.quoteType,
      description: this.description,
      scope: this.scope,
      estimatedStartDate: new Date(this.estimatedStartDate),
      estimatedCompletionDate: new Date(this.estimatedCompletionDate),
      projectManagerId: this.projectManagerId,
      termsAndConditions: this.termsAndConditions,
      taxRate: this.taxRate,
      lineItems: this.lineItems
    };

    this.loading = true;
    this.error = '';

    this.quoteService.createQuote(request).subscribe({
      next: (quoteId) => {
        this.loading = false;
        this.router.navigate(['/quotes', quoteId]);
      },
      error: (err) => {
        this.error = 'Failed to save quote';
        this.loading = false;
        console.error('Error saving quote:', err);
      }
    });
  }

  validateForm(): boolean {
    if (!this.customerId) {
      this.error = 'Please select a customer';
      return false;
    }
    
    if (!this.description.trim()) {
      this.error = 'Please enter a description';
      return false;
    }
    
    if (!this.scope.trim()) {
      this.error = 'Please enter the scope';
      return false;
    }
    
    if (!this.estimatedStartDate) {
      this.error = 'Please select an estimated start date';
      return false;
    }
    
    if (!this.estimatedCompletionDate) {
      this.error = 'Please select an estimated completion date';
      return false;
    }
    
    if (new Date(this.estimatedStartDate) >= new Date(this.estimatedCompletionDate)) {
      this.error = 'Completion date must be after start date';
      return false;
    }
    
    if (this.lineItems.length === 0) {
      this.error = 'Please add at least one line item';
      return false;
    }
    
    for (let i = 0; i < this.lineItems.length; i++) {
      const item = this.lineItems[i];
      if (!item.description.trim()) {
        this.error = `Line item ${i + 1}: Description is required`;
        return false;
      }
      
      if (item.itemType === QuoteLineItemType.Labor) {
        if (!item.jobTypeId) {
          this.error = `Line item ${i + 1}: Job Type is required for Labor items`;
          return false;
        }
        if (item.estimatedHours <= 0) {
          this.error = `Line item ${i + 1}: Hours must be greater than 0`;
          return false;
        }
      }
      
      if (item.itemType !== QuoteLineItemType.Labor && item.quantity <= 0) {
        this.error = `Line item ${i + 1}: Quantity must be greater than 0`;
        return false;
      }
      
      if (item.unitPrice <= 0) {
        this.error = `Line item ${i + 1}: Unit price must be greater than 0`;
        return false;
      }
    }
    
    return true;
  }

  cancel(): void {
    this.router.navigate(['/quotes']);
  }

  getQuoteTypeLabel(type: QuoteType): string {
    return QuoteType[type].replace(/([A-Z])/g, ' $1').trim();
  }

  getLineItemTypeLabel(type: QuoteLineItemType): string {
    return QuoteLineItemType[type];
  }
}
