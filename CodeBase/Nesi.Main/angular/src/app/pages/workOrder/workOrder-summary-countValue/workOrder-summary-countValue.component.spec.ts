/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { WorkOrderSummaryCountValueComponent } from './workOrder-summary-countValue.component';

describe('WorkOrderSummaryCountValueComponent', () => {
  let component: WorkOrderSummaryCountValueComponent;
  let fixture: ComponentFixture<WorkOrderSummaryCountValueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkOrderSummaryCountValueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkOrderSummaryCountValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
