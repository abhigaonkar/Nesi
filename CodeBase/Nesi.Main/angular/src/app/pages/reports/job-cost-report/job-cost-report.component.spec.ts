import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobCostReportComponent } from './job-cost-report.component';

describe('JobCostReportComponent', () => {
  let component: JobCostReportComponent;
  let fixture: ComponentFixture<JobCostReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobCostReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobCostReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
