/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { WorkOrderSummaryBusinessUnitComponent } from './workOrder-Summary-BusinessUnit.component';

describe('WorkOrderSummaryBusinessUnitComponent', () => {
  let component: WorkOrderSummaryBusinessUnitComponent;
  let fixture: ComponentFixture<WorkOrderSummaryBusinessUnitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkOrderSummaryBusinessUnitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkOrderSummaryBusinessUnitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
