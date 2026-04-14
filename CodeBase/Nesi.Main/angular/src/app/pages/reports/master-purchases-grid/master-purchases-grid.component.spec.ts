import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterPurchasesGridComponent } from './master-purchases-grid.component';

describe('MasterPurchasesGridComponent', () => {
  let component: MasterPurchasesGridComponent;
  let fixture: ComponentFixture<MasterPurchasesGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterPurchasesGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterPurchasesGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
