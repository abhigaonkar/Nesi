import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerSurveysComponent } from './customer-surveys.component';

describe('CustomerSurveysComponent', () => {
  let component: CustomerSurveysComponent;
  let fixture: ComponentFixture<CustomerSurveysComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerSurveysComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerSurveysComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
