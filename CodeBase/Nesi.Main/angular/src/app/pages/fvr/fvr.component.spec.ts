/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FvrComponent } from './fvr.component';

describe('FvrComponent', () => {
  let component: FvrComponent;
  let fixture: ComponentFixture<FvrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FvrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FvrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
