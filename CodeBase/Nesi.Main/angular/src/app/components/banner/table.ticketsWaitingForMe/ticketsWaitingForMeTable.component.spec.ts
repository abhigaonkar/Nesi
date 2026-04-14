/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TicketsWaitingForMeTableComponent } from './ticketsWaitingForMeTable.component';

describe('TicketsWaitingForMeTableComponent', () => {
  let component: TicketsWaitingForMeTableComponent;
  let fixture: ComponentFixture<TicketsWaitingForMeTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketsWaitingForMeTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketsWaitingForMeTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
