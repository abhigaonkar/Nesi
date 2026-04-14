import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WoLineGridComponent } from './wo-line-grid.component';

describe('WoLineGridComponent', () => {
  let component: WoLineGridComponent;
  let fixture: ComponentFixture<WoLineGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WoLineGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WoLineGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
