/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EmployeeDaysOffVacationRequestComponent } from './employee-days-off-vacation-request.component';

describe('EmployeeDaysOffVacationRequestComponent', () => {
  let component: EmployeeDaysOffVacationRequestComponent;
  let fixture: ComponentFixture<EmployeeDaysOffVacationRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDaysOffVacationRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDaysOffVacationRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
