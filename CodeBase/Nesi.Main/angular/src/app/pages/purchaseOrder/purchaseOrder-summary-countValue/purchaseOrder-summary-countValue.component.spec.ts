/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PurchaseOrderSummaryCountValueComponent } from './purchaseOrder-summary-countValue.component';

describe('PurchaseOrderSummaryCountValueComponent', () => {
  let component: PurchaseOrderSummaryCountValueComponent;
  let fixture: ComponentFixture<PurchaseOrderSummaryCountValueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseOrderSummaryCountValueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseOrderSummaryCountValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
