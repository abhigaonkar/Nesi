import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutDatatableComponent } from './layout-datatable.component';

describe('LayoutDatatableComponent', () => {
  let component: LayoutDatatableComponent;
  let fixture: ComponentFixture<LayoutDatatableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutDatatableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutDatatableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
