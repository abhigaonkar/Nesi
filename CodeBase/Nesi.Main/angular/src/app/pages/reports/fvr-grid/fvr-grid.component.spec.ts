import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FvrGridComponent } from './fvr-grid.component';

describe('FvrGridComponent', () => {
  let component: FvrGridComponent;
  let fixture: ComponentFixture<FvrGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FvrGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FvrGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
