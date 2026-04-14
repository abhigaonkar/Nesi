/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RootEmployeeDisciplinaryComponent } from './rootEmployeeDisciplinary.component';

describe('RootEmployeeDisciplinaryComponent', () => {
  let component: RootEmployeeDisciplinaryComponent;
  let fixture: ComponentFixture<RootEmployeeDisciplinaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RootEmployeeDisciplinaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RootEmployeeDisciplinaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
