/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { Timesheet_Shop_ProjectsComponent } from './timesheet_Shop_Projects.component';

describe('Timesheet_Shop_ProjectsComponent', () => {
  let component: Timesheet_Shop_ProjectsComponent;
  let fixture: ComponentFixture<Timesheet_Shop_ProjectsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Timesheet_Shop_ProjectsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Timesheet_Shop_ProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
