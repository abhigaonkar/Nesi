import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerLoginHistoryComponent } from './customer-login-history.component';

describe('CustomerLoginHistoryComponent', () => {
  let component: CustomerLoginHistoryComponent;
  let fixture: ComponentFixture<CustomerLoginHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerLoginHistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerLoginHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
