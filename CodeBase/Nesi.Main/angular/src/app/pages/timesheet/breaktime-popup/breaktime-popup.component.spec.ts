import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BreaktimePopupComponent } from './breaktime-popup.component';

describe('BreaktimePopupComponent', () => {
  let component: BreaktimePopupComponent;
  let fixture: ComponentFixture<BreaktimePopupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BreaktimePopupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BreaktimePopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
