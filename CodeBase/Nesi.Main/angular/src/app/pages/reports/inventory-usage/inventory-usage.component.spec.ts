import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryUsageComponent } from './inventory-usage.component';

describe('InventoryUsageComponent', () => {
  let component: InventoryUsageComponent;
  let fixture: ComponentFixture<InventoryUsageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InventoryUsageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InventoryUsageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
