/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TimeSheetVisibleBusinessUnitDropDownComponent } from './visibleBusinessUnitDropDown.component';

describe('VisibleBusinessUnitDropDownComponent', () => {
  let component: TimeSheetVisibleBusinessUnitDropDownComponent;
  let fixture: ComponentFixture<TimeSheetVisibleBusinessUnitDropDownComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimeSheetVisibleBusinessUnitDropDownComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeSheetVisibleBusinessUnitDropDownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
