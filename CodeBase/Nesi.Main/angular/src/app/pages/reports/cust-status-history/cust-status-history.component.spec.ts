import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustStatusHistoryComponent } from './cust-status-history.component';

describe('CustStatusHistoryComponent', () => {
  let component: CustStatusHistoryComponent;
  let fixture: ComponentFixture<CustStatusHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustStatusHistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustStatusHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
