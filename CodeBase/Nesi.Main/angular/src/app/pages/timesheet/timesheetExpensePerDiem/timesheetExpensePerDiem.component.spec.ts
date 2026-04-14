/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TimesheetExpensePerDiemComponent } from './timesheetExpensePerDiem.component';

describe('TimesheetExpensePerDiemComponent', () => {
  let component: TimesheetExpensePerDiemComponent;
  let fixture: ComponentFixture<TimesheetExpensePerDiemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimesheetExpensePerDiemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimesheetExpensePerDiemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
