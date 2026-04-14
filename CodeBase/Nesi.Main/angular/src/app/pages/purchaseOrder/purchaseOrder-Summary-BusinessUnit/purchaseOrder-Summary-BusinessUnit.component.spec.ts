/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PurchaseOrderSummaryBusinessUnitComponent } from './purchaseOrder-Summary-BusinessUnit.component';

describe('PurchaseOrderSummaryBusinessUnitComponent', () => {
  let component: PurchaseOrderSummaryBusinessUnitComponent;
  let fixture: ComponentFixture<PurchaseOrderSummaryBusinessUnitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseOrderSummaryBusinessUnitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseOrderSummaryBusinessUnitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
