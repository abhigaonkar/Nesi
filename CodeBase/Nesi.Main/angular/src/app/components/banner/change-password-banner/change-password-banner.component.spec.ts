import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangePasswordBannerComponent } from './change-password-banner.component';

describe('ChangePasswordBannerComponent', () => {
  let component: ChangePasswordBannerComponent;
  let fixture: ComponentFixture<ChangePasswordBannerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChangePasswordBannerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangePasswordBannerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
