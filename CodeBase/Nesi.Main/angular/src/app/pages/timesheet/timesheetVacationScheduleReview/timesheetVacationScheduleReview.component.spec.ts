/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TimesheetVacationScheduleReviewComponent } from './timesheetVacationScheduleReview.component';

describe('TimesheetVacationScheduleReviewComponent', () => {
  let component: TimesheetVacationScheduleReviewComponent;
  let fixture: ComponentFixture<TimesheetVacationScheduleReviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimesheetVacationScheduleReviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimesheetVacationScheduleReviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
