import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface PurchaseOrderDto {
  id: number;
  purchaseOrderNumber: string;
  vendorId: number;
  vendorName: string;
  workOrderId?: number;
  workOrderNumber?: string;
  requestedBy: number;
  requesterName: string;
  approvedBy?: number;
  approverName?: string;
  orderDate: Date;
  requiredByDate?: Date;
  approvedAt?: Date;
  status: string;
  description?: string;
  notes?: string;
  subTotal: number;
  taxAmount: number;
  shippingCost: number;
  totalAmount: number;
  shippingAddress?: string;
  shippingCity?: string;
  shippingState?: string;
  shippingZipCode?: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt?: Date;
  lineItems?: PurchaseOrderLineItemDto[];
  receipts?: PurchaseOrderReceiptDto[];
}

export interface PurchaseOrderLineItemDto {
  id: number;
  purchaseOrderId: number;
  lineNumber: number;
  description: string;
  partNumber?: string;
  quantity: number;
  unitOfMeasure: string;
  unitPrice: number;
  totalPrice: number;
  quantityReceived: number;
  remainingQuantity: number;
  isFullyReceived: boolean;
  notes?: string;
  createdAt: Date;
}

export interface PurchaseOrderReceiptDto {
  id: number;
  purchaseOrderId: number;
  receiptNumber: string;
  receivedDate: Date;
  receivedBy: number;
  receiverName: string;
  status: string;
  notes?: string;
  packingSlipNumber?: string;
  createdAt: Date;
}

export interface CreatePurchaseOrderCommand {
  vendorId: number;
  workOrderId?: number;
  orderDate: Date;
  requiredByDate?: Date;
  description?: string;
  notes?: string;
  shippingAddress?: string;
  shippingCity?: string;
  shippingState?: string;
  shippingZipCode?: string;
  taxRate?: number;
  shippingCost?: number;
  lineItems: {
    lineNumber: number;
    description: string;
    partNumber?: string;
    quantity: number;
    unitOfMeasure: string;
    unitPrice: number;
  }[];
}

export interface UpdatePurchaseOrderCommand {
  id: number;
  vendorId: number;
  workOrderId?: number;
  requiredByDate?: Date;
  description?: string;
  notes?: string;
  shippingAddress?: string;
  shippingCity?: string;
  shippingState?: string;
  shippingZipCode?: string;
  taxRate?: number;
  shippingCost?: number;
}

export interface CreateReceiptCommand {
  purchaseOrderId: number;
  receivedDate: Date;
  packingSlipNumber?: string;
  notes?: string;
  items: {
    purchaseOrderLineItemId: number;
    quantityReceived: number;
    condition?: string;
    notes?: string;
    hasDiscrepancy?: boolean;
    discrepancyReason?: string;
  }[];
}

@Injectable({
  providedIn: 'root'
})
export class PurchaseOrderService {
  private apiUrl = '/api/purchaseorder';

  constructor(private http: HttpClient, private apiService: ApiService) {}

  getPurchaseOrders(
    vendorId?: number,
    workOrderId?: number,
    status?: string,
    pageNumber: number = 1,
    pageSize: number = 20
  ): Observable<any> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    
    if (vendorId) params = params.set('vendorId', vendorId.toString());
    if (workOrderId) params = params.set('workOrderId', workOrderId.toString());
    if (status) params = params.set('status', status);
    
    return this.apiService.get<any>(`${this.apiUrl}`, params);
  }

  getPurchaseOrderById(id: number, includeLineItems: boolean = true, includeReceipts: boolean = false): Observable<PurchaseOrderDto> {
    let params = new HttpParams()
      .set('includeLineItems', includeLineItems.toString())
      .set('includeReceipts', includeReceipts.toString());
    
    return this.apiService.get<PurchaseOrderDto>(`${this.apiUrl}/${id}`, params);
  }

  createPurchaseOrder(po: CreatePurchaseOrderCommand): Observable<number> {
    return this.apiService.post<number>(this.apiUrl, po);
  }

  updatePurchaseOrder(po: UpdatePurchaseOrderCommand): Observable<boolean> {
    return this.apiService.put<boolean>(`${this.apiUrl}/${po.id}`, po);
  }

  submitPurchaseOrder(id: number): Observable<boolean> {
    return this.apiService.post<boolean>(`${this.apiUrl}/${id}/submit`, {});
  }

  approvePurchaseOrder(id: number): Observable<boolean> {
    return this.apiService.post<boolean>(`${this.apiUrl}/${id}/approve`, {});
  }

  rejectPurchaseOrder(id: number, reason: string): Observable<boolean> {
    return this.apiService.post<boolean>(`${this.apiUrl}/${id}/reject`, { reason });
  }

  createReceipt(id: number, receipt: CreateReceiptCommand): Observable<number> {
    return this.apiService.post<number>(`${this.apiUrl}/${id}/receipts`, receipt);
  }

  getReceipts(id: number): Observable<PurchaseOrderReceiptDto[]> {
    return this.apiService.get<PurchaseOrderReceiptDto[]>(`${this.apiUrl}/${id}/receipts`);
  }

  validateInvoice(id: number, invoiceTotal: number, lineItems: any[], tolerancePercentage: number = 5.0): Observable<any> {
    return this.apiService.post<any>(`${this.apiUrl}/${id}/validate-invoice`, {
      invoiceTotal,
      lineItems,
      tolerancePercentage
    });
  }
}
