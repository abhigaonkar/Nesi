/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FvrTableComponent } from './fvrTable.component';

describe('FvrTableComponent', () => {
  let component: FvrTableComponent;
  let fixture: ComponentFixture<FvrTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FvrTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FvrTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
