/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TimesheetExpenseHitoryComponent } from './timesheetExpenseHitory.component';

describe('TimesheetExpenseHitoryComponent', () => {
  let component: TimesheetExpenseHitoryComponent;
  let fixture: ComponentFixture<TimesheetExpenseHitoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimesheetExpenseHitoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimesheetExpenseHitoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
