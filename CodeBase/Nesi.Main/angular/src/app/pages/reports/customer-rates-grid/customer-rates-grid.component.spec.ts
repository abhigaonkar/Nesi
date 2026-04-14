import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerRatesGridComponent } from './customer-rates-grid.component';

describe('CustomerRatesGridComponent', () => {
  let component: CustomerRatesGridComponent;
  let fixture: ComponentFixture<CustomerRatesGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerRatesGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerRatesGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
