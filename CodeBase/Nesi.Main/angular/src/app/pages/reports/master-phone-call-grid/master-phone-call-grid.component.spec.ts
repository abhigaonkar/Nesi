import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterPhoneCallGridComponent } from './master-phone-call-grid.component';

describe('MasterPhoneCallGridComponent', () => {
  let component: MasterPhoneCallGridComponent;
  let fixture: ComponentFixture<MasterPhoneCallGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterPhoneCallGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterPhoneCallGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
