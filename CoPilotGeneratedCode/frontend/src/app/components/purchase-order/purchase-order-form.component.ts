import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PurchaseOrderService, CreatePurchaseOrderCommand, UpdatePurchaseOrderCommand } from '../../../services/purchase-order.service';
import { VendorService, VendorDto } from '../../../services/vendor.service';

interface LineItemForm {
  id?: number;
  partNumber: string;
  description: string;
  quantity: number;
  unitOfMeasure: string;
  unitPrice: number;
  totalPrice: number;
}

@Component({
  selector: 'app-purchase-order-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './purchase-order-form.component.html',
  styleUrls: ['./purchase-order-form.component.scss']
})
export class PurchaseOrderFormComponent implements OnInit {
  isEditMode: boolean = false;
  purchaseOrderId?: number;
  loading: boolean = false;
  saving: boolean = false;
  error: string = '';

  // Form fields
  vendorId: number = 0;
  workOrderId?: number;
  orderDate: string = new Date().toISOString().split('T')[0];
  requiredByDate: string = '';
  description: string = '';
  notes: string = '';
  shippingAddress: string = '';
  shippingCity: string = '';
  shippingState: string = '';
  shippingZipCode: string = '';
  taxRate: number = 0;
  shippingCost: number = 0;

  // Line items
  lineItems: LineItemForm[] = [];

  // Vendors list
  vendors: VendorDto[] = [];

  // Calculated totals
  subTotal: number = 0;
  taxAmount: number = 0;
  totalAmount: number = 0;

  constructor(
    private purchaseOrderService: PurchaseOrderService,
    private vendorService: VendorService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadVendors();
    
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.purchaseOrderId = Number(id);
      this.loadPurchaseOrder(this.purchaseOrderId);
    } else {
      // Initialize with one empty line item
      this.addLineItem();
    }
  }

  loadVendors(): void {
    this.vendorService.getVendors(true, 1, 100)
      .subscribe({
        next: (response: any) => {
          this.vendors = response.data || [];
        },
        error: (err) => {
          console.error('Failed to load vendors', err);
        }
      });
  }

  loadPurchaseOrder(id: number): void {
    this.loading = true;
    this.purchaseOrderService.getPurchaseOrderById(id, true, false)
      .subscribe({
        next: (response: any) => {
          const po = response.data;
          this.vendorId = po.vendorId;
          this.workOrderId = po.workOrderId;
          this.orderDate = po.orderDate?.split('T')[0] || '';
          this.requiredByDate = po.requiredByDate?.split('T')[0] || '';
          this.description = po.description || '';
          this.notes = po.notes || '';
          this.shippingAddress = po.shippingAddress || '';
          this.shippingCity = po.shippingCity || '';
          this.shippingState = po.shippingState || '';
          this.shippingZipCode = po.shippingZipCode || '';
          this.taxRate = po.taxRate || 0;
          this.shippingCost = po.shippingCost || 0;
          
          this.lineItems = po.lineItems?.map((item: any) => ({
            id: item.id,
            partNumber: item.partNumber || '',
            description: item.description,
            quantity: item.quantity,
            unitOfMeasure: item.unitOfMeasure,
            unitPrice: item.unitPrice,
            totalPrice: item.totalPrice
          })) || [];
          
          this.calculateTotals();
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load purchase order';
          this.loading = false;
          console.error(err);
        }
      });
  }

  addLineItem(): void {
    this.lineItems.push({
      partNumber: '',
      description: '',
      quantity: 1,
      unitOfMeasure: 'EA',
      unitPrice: 0,
      totalPrice: 0
    });
  }

  removeLineItem(index: number): void {
    if (this.lineItems.length > 1) {
      this.lineItems.splice(index, 1);
      this.calculateTotals();
    }
  }

  onLineItemChange(item: LineItemForm): void {
    item.totalPrice = item.quantity * item.unitPrice;
    this.calculateTotals();
  }

  calculateTotals(): void {
    this.subTotal = this.lineItems.reduce((sum, item) => sum + item.totalPrice, 0);
    this.taxAmount = this.subTotal * (this.taxRate / 100);
    this.totalAmount = this.subTotal + this.taxAmount + this.shippingCost;
  }

  onTaxOrShippingChange(): void {
    this.calculateTotals();
  }

  savePurchaseOrder(): void {
    // Validation
    if (!this.vendorId || this.vendorId === 0) {
      this.error = 'Please select a vendor';
      return;
    }

    if (!this.orderDate) {
      this.error = 'Please enter an order date';
      return;
    }

    if (this.lineItems.length === 0) {
      this.error = 'Please add at least one line item';
      return;
    }

    if (this.lineItems.some(item => !item.description || item.quantity <= 0 || item.unitPrice < 0)) {
      this.error = 'Please complete all line item fields with valid values';
      return;
    }

    this.saving = true;
    this.error = '';

    if (this.isEditMode && this.purchaseOrderId) {
      this.updatePurchaseOrder();
    } else {
      this.createPurchaseOrder();
    }
  }

  createPurchaseOrder(): void {
    const command: CreatePurchaseOrderCommand = {
      vendorId: this.vendorId,
      workOrderId: this.workOrderId,
      orderDate: new Date(this.orderDate),
      requiredByDate: this.requiredByDate ? new Date(this.requiredByDate) : undefined,
      description: this.description || undefined,
      notes: this.notes || undefined,
      shippingAddress: this.shippingAddress || undefined,
      shippingCity: this.shippingCity || undefined,
      shippingState: this.shippingState || undefined,
      shippingZipCode: this.shippingZipCode || undefined,
      taxRate: this.taxRate,
      shippingCost: this.shippingCost,
      lineItems: this.lineItems.map((item, index) => ({
        lineNumber: index + 1,
        partNumber: item.partNumber || undefined,
        description: item.description,
        quantity: item.quantity,
        unitOfMeasure: item.unitOfMeasure,
        unitPrice: item.unitPrice
      }))
    };

    this.purchaseOrderService.createPurchaseOrder(command)
      .subscribe({
        next: (response: any) => {
          alert('Purchase order created successfully');
          this.router.navigate(['/purchase-orders', response.data]);
        },
        error: (err) => {
          this.error = 'Failed to create purchase order';
          this.saving = false;
          console.error(err);
        }
      });
  }

  updatePurchaseOrder(): void {
    const command: UpdatePurchaseOrderCommand = {
      id: this.purchaseOrderId!,
      vendorId: this.vendorId,
      workOrderId: this.workOrderId,
      requiredByDate: this.requiredByDate ? new Date(this.requiredByDate) : undefined,
      description: this.description || undefined,
      notes: this.notes || undefined,
      shippingAddress: this.shippingAddress || undefined,
      shippingCity: this.shippingCity || undefined,
      shippingState: this.shippingState || undefined,
      shippingZipCode: this.shippingZipCode || undefined,
      taxRate: this.taxRate,
      shippingCost: this.shippingCost
    };

    this.purchaseOrderService.updatePurchaseOrder(command)
      .subscribe({
        next: () => {
          alert('Purchase order updated successfully');
          this.router.navigate(['/purchase-orders', this.purchaseOrderId]);
        },
        error: (err) => {
          this.error = 'Failed to update purchase order';
          this.saving = false;
          console.error(err);
        }
      });
  }

  cancel(): void {
    if (this.isEditMode && this.purchaseOrderId) {
      this.router.navigate(['/purchase-orders', this.purchaseOrderId]);
    } else {
      this.router.navigate(['/purchase-orders']);
    }
  }
}
