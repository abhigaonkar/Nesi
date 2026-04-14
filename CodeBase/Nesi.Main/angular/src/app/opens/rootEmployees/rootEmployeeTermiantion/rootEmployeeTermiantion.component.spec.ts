/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RootEmployeeTermiantionComponent } from './rootEmployeeTermiantion.component';

describe('RootEmployeeTermiantionComponent', () => {
  let component: RootEmployeeTermiantionComponent;
  let fixture: ComponentFixture<RootEmployeeTermiantionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RootEmployeeTermiantionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RootEmployeeTermiantionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
