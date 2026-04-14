/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { VisibleBusinessUnitDropDownComponent } from './visibleBusinessUnitDropDown.component';

describe('VisibleBusinessUnitDropDownComponent', () => {
  let component: VisibleBusinessUnitDropDownComponent;
  let fixture: ComponentFixture<VisibleBusinessUnitDropDownComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VisibleBusinessUnitDropDownComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VisibleBusinessUnitDropDownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
