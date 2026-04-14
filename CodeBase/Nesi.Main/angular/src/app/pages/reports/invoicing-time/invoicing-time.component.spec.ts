import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoicingTimeComponent } from './invoicing-time.component';

describe('InvoicingTimeComponent', () => {
  let component: InvoicingTimeComponent;
  let fixture: ComponentFixture<InvoicingTimeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InvoicingTimeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvoicingTimeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
