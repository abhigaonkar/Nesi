import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseOrderCreditCardPurchasesComponent } from './purchase-order-credit-card-purchases.component';

describe('PurchaseOrderCreditCardPurchasesComponent', () => {
  let component: PurchaseOrderCreditCardPurchasesComponent;
  let fixture: ComponentFixture<PurchaseOrderCreditCardPurchasesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseOrderCreditCardPurchasesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseOrderCreditCardPurchasesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
