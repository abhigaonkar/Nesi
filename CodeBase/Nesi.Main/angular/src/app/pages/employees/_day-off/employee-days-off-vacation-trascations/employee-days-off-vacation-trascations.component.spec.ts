/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EmployeeDaysOffVacationTrascationsComponent } from './employee-days-off-vacation-trascations.component';

describe('EmployeeDaysOffVacationTrascationsComponent', () => {
  let component: EmployeeDaysOffVacationTrascationsComponent;
  let fixture: ComponentFixture<EmployeeDaysOffVacationTrascationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDaysOffVacationTrascationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDaysOffVacationTrascationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
