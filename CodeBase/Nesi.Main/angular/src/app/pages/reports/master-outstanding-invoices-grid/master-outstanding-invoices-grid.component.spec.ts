import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterOutstandingInvoicesGridComponent } from './master-outstanding-invoices-grid.component';

describe('MasterOutstandingInvoicesGridComponent', () => {
  let component: MasterOutstandingInvoicesGridComponent;
  let fixture: ComponentFixture<MasterOutstandingInvoicesGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterOutstandingInvoicesGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterOutstandingInvoicesGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
