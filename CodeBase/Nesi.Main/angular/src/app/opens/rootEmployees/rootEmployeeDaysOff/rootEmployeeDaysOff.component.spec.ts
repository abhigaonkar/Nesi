/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RootEmployeeDaysOffComponent } from './rootEmployeeDaysOff.component';

describe('RootEmployeeDaysOffComponent', () => {
  let component: RootEmployeeDaysOffComponent;
  let fixture: ComponentFixture<RootEmployeeDaysOffComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RootEmployeeDaysOffComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RootEmployeeDaysOffComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
