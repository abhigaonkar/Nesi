import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterQuoteGridComponent } from './master-quote-grid.component';

describe('MasterQuoteGridComponent', () => {
  let component: MasterQuoteGridComponent;
  let fixture: ComponentFixture<MasterQuoteGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterQuoteGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterQuoteGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
