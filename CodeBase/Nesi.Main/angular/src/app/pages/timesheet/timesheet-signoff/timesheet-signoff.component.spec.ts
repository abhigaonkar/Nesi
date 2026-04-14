import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimesheetSignoffComponent } from './timesheet-signoff.component';

describe('TimesheetSignoffComponent', () => {
  let component: TimesheetSignoffComponent;
  let fixture: ComponentFixture<TimesheetSignoffComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimesheetSignoffComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimesheetSignoffComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
