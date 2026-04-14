import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PartnerSpecialtiesCustomerComponent } from './partner-specialties-customer.component';

describe('PartnerSpecialtiesCustomerComponent', () => {
  let component: PartnerSpecialtiesCustomerComponent;
  let fixture: ComponentFixture<PartnerSpecialtiesCustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PartnerSpecialtiesCustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PartnerSpecialtiesCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
