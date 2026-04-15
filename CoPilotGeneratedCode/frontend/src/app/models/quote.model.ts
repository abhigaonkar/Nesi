export enum QuoteStatus {
  Draft = 0,
  Submitted = 1,
  Approved = 2,
  Rejected = 3,
  CustomerApproved = 4
}

export enum QuoteType {
  TimeAndMaterial = 0,
  FixedPrice = 1,
  CostPlus = 2
}

export enum QuoteLineItemType {
  Labor = 0,
  Material = 1,
  Equipment = 2,
  Miscellaneous = 3
}

export interface Quote {
  id: number;
  quoteNumber: string;
  customerId: number;
  customerName: string;
  quoteType: QuoteType;
  status: QuoteStatus;
  description: string;
  scope: string;
  estimatedStartDate: Date;
  estimatedCompletionDate: Date;
  projectManagerId?: number;
  projectManagerName?: string;
  termsAndConditions: string;
  
  subtotal: number;
  taxRate: number;
  taxAmount: number;
  discountPercent: number;
  discountAmount: number;
  total: number;
  
  approvedBy?: number;
  approvedByName?: string;
  approvedAt?: Date;
  rejectionReason?: string;
  revisionNumber: number;
  
  customerApprovedAt?: Date;
  customerApprovedBy?: string;
  
  workOrderId?: number;
  convertedToWorkOrderAt?: Date;
  
  lineItems: QuoteLineItem[];
  
  createdAt: Date;
  createdBy: string;
}

export interface QuoteLineItem {
  id: number;
  quoteId: number;
  itemType: QuoteLineItemType;
  lineNumber: number;
  description: string;
  
  jobTypeId?: number;
  jobTypeName?: string;
  estimatedHours: number;
  
  partNumber?: string;
  quantity: number;
  
  unitPrice: number;
  total: number;
  notes?: string;
}

export interface CreateQuoteLineItem {
  itemType: QuoteLineItemType;
  description: string;
  jobTypeId?: number;
  estimatedHours: number;
  partNumber?: string;
  quantity: number;
  unitPrice: number;
  notes?: string;
}

export interface CreateQuoteRequest {
  customerId: number;
  quoteType: QuoteType;
  description: string;
  scope: string;
  estimatedStartDate: Date;
  estimatedCompletionDate: Date;
  projectManagerId?: number;
  termsAndConditions: string;
  taxRate: number;
  lineItems: CreateQuoteLineItem[];
}
