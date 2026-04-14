/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { WorkOrderSummarySumtotalComponent } from './workOrder-summary-sumtotal.component';

describe('WorkOrderSummarySumtotalComponent', () => {
  let component: WorkOrderSummarySumtotalComponent;
  let fixture: ComponentFixture<WorkOrderSummarySumtotalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkOrderSummarySumtotalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkOrderSummarySumtotalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
