/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EmployeePrivilegeUserSwitchingComponent } from './employee-privilege-user-switching.component';

describe('EmployeePrivilegeUserSwitchingComponent', () => {
  let component: EmployeePrivilegeUserSwitchingComponent;
  let fixture: ComponentFixture<EmployeePrivilegeUserSwitchingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeePrivilegeUserSwitchingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeePrivilegeUserSwitchingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
