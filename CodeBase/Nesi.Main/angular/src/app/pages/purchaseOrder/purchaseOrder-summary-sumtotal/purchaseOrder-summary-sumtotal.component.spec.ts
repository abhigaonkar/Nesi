/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PurchaseOrderSummarySumtotalComponent } from './purchaseOrder-summary-sumtotal.component';

describe('PurchaseOrderSummarySumtotalComponent', () => {
  let component: PurchaseOrderSummarySumtotalComponent;
  let fixture: ComponentFixture<PurchaseOrderSummarySumtotalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseOrderSummarySumtotalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseOrderSummarySumtotalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
