import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimesheetTransferComponent } from './timesheet-transfer.component';

describe('TimesheetTransferComponent', () => {
  let component: TimesheetTransferComponent;
  let fixture: ComponentFixture<TimesheetTransferComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimesheetTransferComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimesheetTransferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
